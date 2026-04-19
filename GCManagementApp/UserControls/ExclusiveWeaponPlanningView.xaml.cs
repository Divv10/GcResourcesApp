using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using MaterialDesignThemes.Wpf;
using PixelLab.Common;
using PixelLab.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for ExclusiveWeaponPlanningView.xaml
    /// </summary>
    public partial class ExclusiveWeaponPlanningView : UserControl, INotifyPropertyChanged
    {
        public ICommand AddNewEWCommand { get; }
        public ICommand EditEWCommand { get; }
        public ICommand DeleteEWCommand { get; }

        public int EWOrbGain => ExclusiveWeaponMaterials.EwOrbWeeklyTotal;
        public int EWOrbClassGain => ExclusiveWeaponMaterials.EwOrbClassWeeklyTotal;

        public Inventory Inventory => ProfileGrowth.Profile.MaterialsInventory;
       
        private ObservableCollection<EwPlan> _ewPlans;
        public ObservableCollection<EwPlan> EwPlans
        {
            get => _ewPlans;
            set => SetProperty(ref _ewPlans, value);
        }

        public int WeeklyEwMatsIncome
        {
            get => ProfileGrowth.Profile.Settings.WeeklyEwMatsIncome;
            set
            {
                ProfileGrowth.Profile.Settings.WeeklyEwMatsIncome = value;
                RecalculateDaysReady();
                OnPropertyChanged(nameof(WeeklyEwMatsIncome));
            }
        }

        public int WeeklyEwMatsOrbIncome
        {
            get => ProfileGrowth.Profile.Settings.WeeklyEWOrbIncome;
            set
            {
                ProfileGrowth.Profile.Settings.WeeklyEWOrbIncome = value;
                RecalculateDaysReady();
                OnPropertyChanged(nameof(WeeklyEwMatsOrbIncome));
            }
        }

        public string CalculationLog { get; set; }

        public ExclusiveWeaponPlanningView()
        {
            InitializeComponent();
            DataContext = this;
            AddNewEWCommand = new RelayCommand(AddNewEw);
            EditEWCommand = new RelayCommand(EditEwPlan);
            DeleteEWCommand = new RelayCommand(DeleteEwPlan);

            EwPlans = new ObservableCollection<EwPlan>();
            foreach (var ew in ProfileGrowth.Profile.EwPlans)
            {
                ew.Hero = Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == ew.HeroName && h.HeroType == ew.HeroType);
                var phg = ProfileGrowth.Heroes.Where(p => p.Hero.HeroName == ew.HeroName && p.Hero.HeroType == ew.HeroType).FirstOrDefault();
                ew.IsEwOwned = phg.Equipment.IsExclusiveWeaponOwned;
                ew.CurrentLevel = phg.Equipment.IsExclusiveWeaponOwned ? phg.Equipment.ExclusiveWeaponUpgrade : 0;

                if (ew.CurrentLevel == ew.DesiredLevel && ew.CurrentLevel != 0 && ew.IsEwOwned)
                {
                    ew.WeaponCost = new ExclusiveWeaponCost(0, 0, 0);
                }
                else
                {
                    ew.WeaponCost = ExclusiveWeaponCosts.CalculateCost(ew.IsEwOwned ? ew.CurrentLevel : -1, ew.DesiredLevel, ew.CurrentPartialLevel);
                }

                EwPlans.Add(ew);
            }

            RecalculateDaysReady();

            this.IsVisibleChanged += (_, e) =>
            {
                if ((bool)e.NewValue == true)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        ReloadCurrentHeroGrowth();
                        RecalculateDaysReady();
                        OnPropertyChanged("");
                    }));
                }
            };

            ProfileManager.ProfileChanged += (sender, ae) =>
            {
                EwPlans = new ObservableCollection<EwPlan>();
                foreach (var ew in ProfileGrowth.Profile.EwPlans)
                {
                    ew.Hero = Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == ew.HeroName && h.HeroType == ew.HeroType);
                    var phg = ProfileGrowth.Heroes.Where(p => p.Hero.HeroName == ew.HeroName && p.Hero.HeroType == ew.HeroType).FirstOrDefault();
                    ew.CurrentLevel = phg.Equipment.IsExclusiveWeaponOwned ? phg.Equipment.ExclusiveWeaponUpgrade : 0;

                    if (ew.CurrentLevel == ew.DesiredLevel && ew.CurrentLevel != 0 && ew.IsEwOwned)
                    {
                        ew.WeaponCost = new ExclusiveWeaponCost(0, 0, 0);
                    }
                    else
                    {
                        ew.WeaponCost = ExclusiveWeaponCosts.CalculateCost(ew.IsEwOwned ? ew.CurrentLevel : -1, ew.DesiredLevel, ew.CurrentPartialLevel);
                    }

                    EwPlans.Add(ew);
                }

                RecalculateDaysReady();
                OnPropertyChanged(string.Empty);
            };
        }

        private async void AddNewEw(object param)
        {
            var view = new EditEwPlan();
            var result = await DialogHost.Show(view, "EwPlanningDialog");
            if ((bool)result)
            {
                var ewPlan = new EwPlan();
                ewPlan.Hero = view.SelectedHero;
                ewPlan.HeroName = view.SelectedHero.HeroName;
                ewPlan.HeroType = view.SelectedHero.HeroType;
                ewPlan.CurrentLevel = view.CurrentEwLevel;
                ewPlan.CurrentPartialLevel = view.CurrentProgress;
                ewPlan.DesiredLevel = view.DesiredEwLevel;
                ewPlan.IsEwOwned = ProfileGrowth.Heroes.FirstOrDefault(x => x.Hero.HeroName == ewPlan.HeroName && x.Hero.HeroType == ewPlan.HeroType).Equipment.IsExclusiveWeaponOwned;

                if (ewPlan.CurrentLevel == ewPlan.DesiredLevel && ewPlan.CurrentLevel != 0 && ewPlan.IsEwOwned)
                {
                    ewPlan.WeaponCost = new ExclusiveWeaponCost(0, 0, 0);
                }
                else
                {
                    ewPlan.WeaponCost = ExclusiveWeaponCosts.CalculateCost(ewPlan.IsEwOwned ? ewPlan.CurrentLevel : -1, ewPlan.DesiredLevel, ewPlan.CurrentPartialLevel);
                }

                EwPlans.Add(ewPlan);

                RecalculateDaysReady();
            };
        }

        private async void EditEwPlan(object param)
        {
            var ep = (EwPlan)param;

            var view = new EditEwPlan();

            view.SelectedHero = view.HeroesCollection.FirstOrDefault(h => h.HeroName == ep.Hero.HeroName && h.HeroType == ep.Hero.HeroType);
            view.CurrentEwLevel = ep.CurrentLevel;
            view.DesiredEwLevel = ep.DesiredLevel;
            view.CurrentProgress = ep.CurrentPartialLevel;

            var result = await DialogHost.Show(view, "EwPlanningDialog");
            if ((bool)result)
            {
                var ewPlan = new EwPlan();
                ewPlan.Hero = view.SelectedHero;
                ewPlan.HeroName = view.SelectedHero.HeroName;
                ewPlan.HeroType = view.SelectedHero.HeroType;
                ewPlan.CurrentLevel = view.CurrentEwLevel;
                ewPlan.CurrentPartialLevel = view.CurrentProgress;
                ewPlan.DesiredLevel = view.DesiredEwLevel;
                ewPlan.IsEwOwned = ep.IsEwOwned;

                if (ewPlan.CurrentLevel == ewPlan.DesiredLevel && ewPlan.CurrentLevel != 0 && ewPlan.IsEwOwned)
                {
                    ewPlan.WeaponCost = new ExclusiveWeaponCost(0, 0, 0);
                }
                else
                {
                    ewPlan.WeaponCost = ExclusiveWeaponCosts.CalculateCost(ewPlan.IsEwOwned ? ewPlan.CurrentLevel : -1, ewPlan.DesiredLevel, ewPlan.CurrentPartialLevel);
                }

                EwPlans.Insert(EwPlans.IndexOf(ep), ewPlan);
                EwPlans.Remove(ep);

                RecalculateDaysReady();
            };
        }

        private void DeleteEwPlan(object param)
        {
            var ewPlan = (EwPlan)param;
            if (EwPlans.Contains(ewPlan))
            {
                EwPlans.Remove(ewPlan);
            }

            RecalculateDaysReady();
        }

        private void ReloadCurrentHeroGrowth()
        {
            foreach (var ew in ProfileGrowth.Profile.EwPlans)
            {
                ew.Hero = Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == ew.HeroName && h.HeroType == ew.HeroType);
                var phg = ProfileGrowth.Heroes.Where(p => p.Hero.HeroName == ew.HeroName && p.Hero.HeroType == ew.HeroType).FirstOrDefault();

                ew.CurrentLevel = phg.Equipment.IsExclusiveWeaponOwned ? phg.Equipment.ExclusiveWeaponUpgrade : 0;

                if (ew.CurrentLevel == ew.DesiredLevel && ew.CurrentLevel != 0 && ew.IsEwOwned)
                {
                    ew.WeaponCost = new ExclusiveWeaponCost(0, 0, 0);
                }
                else
                {
                    ew.WeaponCost = ExclusiveWeaponCosts.CalculateCost(ew.IsEwOwned ? ew.CurrentLevel : -1, ew.DesiredLevel, ew.CurrentPartialLevel);
                }
            }

            RecalculateDaysReady();
        }

        private void RecalculateDaysReady()
        {
            var currentEwMats = Inventory.EwMats;
            var profileInventory = ProfileGrowth.Profile.MaterialsInventory;
            var currentInventory = new Inventory()
            {
                AssaultEWOrb = profileInventory.AssaultEWOrb,
                TankEWOrb = profileInventory.TankEWOrb,
                RangerEWOrb = profileInventory.RangerEWOrb,
                HealerEWOrb = profileInventory.HealerEWOrb,
                MageEWOrb = profileInventory.MageEWOrb,
            };

            double dailyEwMats = 0;

            if (WeeklyEwMatsIncome > 0) 
            {
                dailyEwMats = WeeklyEwMatsIncome / 7;
            } else
            {
                dailyEwMats = ExclusiveWeaponMaterials.EwOrbWeeklyTotal / 7;
            }

            foreach (var ew in EwPlans)
            {
                ew.DaysForEW = (double)(ew.WeaponCost.EwMatsCost - currentEwMats) / dailyEwMats;
                currentEwMats -= ew.WeaponCost.EwMatsCost;
                ew.EwMatsNeeded = currentEwMats;

                CalculateEWOrbMaterials(ew, ref currentInventory, ew.Hero.HeroClass);
            }

            ProfileGrowth.Profile.EwPlans = new List<EwPlan>(EwPlans);
            ProfileGrowth.Profile.SaveToJson();
        }

        private void CalculateEWOrbMaterials(EwPlan ew, ref Inventory currentInventory, HeroClass heroClass)
        {
            double dailyOrbClassEwMat = 0;

            if (WeeklyEwMatsOrbIncome > 0)
            {
                dailyOrbClassEwMat = WeeklyEwMatsOrbIncome / 7;
            } else
            {
                dailyOrbClassEwMat = ExclusiveWeaponMaterials.EwOrbClassWeeklyTotal;
            }
            ew.DaysForEWOrb = (double)(ew.WeaponCost.EwOrbCost - currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.EW }]) / dailyOrbClassEwMat;
            
            currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.EW }] -= ew.WeaponCost.EwOrbCost;
            ew.EwOrbMatsNeeded = currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.EW }];
        }

        private void ReorderListBox_ReorderRequested(object sender, ReorderEventArgs e)
        {
            var reorderListBox = (ReorderListBox)e.OriginalSource;

            var draggingElement = (EwPlan)reorderListBox.ItemContainerGenerator.ItemFromContainer(e.ItemContainer);
            var toElement = (EwPlan)reorderListBox.ItemContainerGenerator.ItemFromContainer(e.ToContainer);

            EwPlans.Move(EwPlans.IndexOf(draggingElement), EwPlans.IndexOf(toElement));

            RecalculateDaysReady();
        }

        #region PC

        public event PropertyChangedEventHandler PropertyChanged = null!;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> raiser)
        {
            var propName = ((MemberExpression)raiser?.Body!)?.Member.Name;
            OnPropertyChanged(propName!);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null!)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        #endregion

    }
}
