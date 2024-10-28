using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json.Linq;
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
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for PlanningView.xaml
    /// </summary>
    public partial class PlanningView : UserControl, INotifyPropertyChanged
    {
        public ICommand AddNewHeroCommand { get; }
        public ICommand EditHeroCommand { get; }
        public ICommand DeleteHeroCommand { get; }
        public ICommand CopyDetailsCommand { get; }

        public Inventory Inventory => ProfileGrowth.Profile.MaterialsInventory;

        public int GCGain => GrowthCubes.CubesTotalWeekly / 7;
        public int GEGain => GrowthEssences.GrowthEssenceTotalWeekly / 7 ;
        public int GrowthCubesFromBg => Inventory.GrowthCubesFromBlueGems;

        private ObservableCollection<HeroPlan> _heroPlans;
        public ObservableCollection<HeroPlan> HeroPlans
        {
            get => _heroPlans;
            set => SetProperty(ref _heroPlans, value);
        }
        
        public int DailyGoldIncome
        {
            get => ProfileGrowth.Profile.Settings.DailyGoldIncome;
            set
            {
                ProfileGrowth.Profile.Settings.DailyGoldIncome = value;
                RecalculateDaysReady();
                OnPropertyChanged(nameof(DailyGoldIncome));
            }
        }

        public string CalculationLog { get; set; }
        private StringBuilder calcLog;

        public PlanningView()
        {
            InitializeComponent();
            DataContext = this;
            AddNewHeroCommand = new RelayCommand(AddNewHero);
            EditHeroCommand = new RelayCommand(EditHeroPlan);
            DeleteHeroCommand = new RelayCommand(DeleteHeroPlan);
            CopyDetailsCommand = new RelayCommand(CopyDetails);

            HeroPlans = new ObservableCollection<HeroPlan>();
            foreach (var hp in ProfileGrowth.Profile.HeroPlans)
            {
                hp.Hero = Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == hp.HeroName && h.HeroType == hp.HeroType);
                var phg = ProfileGrowth.Heroes.Where(p => p.Hero.HeroName == hp.HeroName && p.Hero.HeroType == hp.HeroType).FirstOrDefault();

                if (hp.HeroName == HeroEnum.Custom)
                {
                    hp.Hero = new Hero(hp.HeroName, hp.HeroType, hp.HeroClass, hp.HeroAttribute);
                    hp.CurrentGrowth = new GrowthPlan();
                }
                else
                    hp.CurrentGrowth = new GrowthPlan() { TranscendenceLevel = phg.TranscendenceLevel, ChaserLevel = phg.ChaserLevel, SiLevel = phg.SiLevel, IsCoreOpen = phg.IsCoreOpen, TraitsOpen = phg.TraitsOpen };

                if (hp.Hero.HeroType == Enums.HeroType.T)
                {
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max(0, hp.CurrentGrowth.TranscendenceLevel);
                    hp.CurrentGrowth.ChaserLevel = Math.Max(20, hp.CurrentGrowth.ChaserLevel);
                }
                else if (hp.Hero.HeroType == Enums.HeroType.S)
                {
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max((int)Static.StaticValues.MaxTranscendenceLevel, hp.CurrentGrowth.TranscendenceLevel);
                    hp.CurrentGrowth.ChaserLevel = Math.Max((int)Static.StaticValues.MaxClLevel, hp.CurrentGrowth.ChaserLevel);
                    hp.CurrentGrowth.SiLevel = Math.Max((int)Static.StaticValues.MaxSiLevel, hp.CurrentGrowth.SiLevel);
                    hp.CurrentGrowth.TraitsOpen = 27;
                }

                hp.TCost = TranscendingCosts.CalculateCost(hp.CurrentGrowth.TranscendenceLevel, hp.DesiredGrowth.TranscendenceLevel);
                hp.ClLevelCost = ChaserLevelingCosts.CalculateCost(hp.CurrentGrowth.ChaserLevel, hp.DesiredGrowth.ChaserLevel);
                //hp.SiLevelCost = SiLevelingCosts.CalculateCost(hp.CurrentGrowth.SiLevel, hp.DesiredGrowth.SiLevel);
                hp.SiLevelCost = SiLevelingCosts.CalculateCostWithTraits(hp.CurrentGrowth.SiLevel, hp.DesiredGrowth.SiLevel, hp.CurrentGrowth.TraitsOpen, hp.DesiredGrowth.TraitsOpen, hp.CurrentGrowth.IsCoreOpen, hp.DesiredGrowth.IsCoreOpen);

                RecalculateSiCubesCost(hp, phg);

                HeroPlans.Add(hp);
            }

            RecalculateDaysReady();

            this.IsVisibleChanged += (_, _) => 
            {
                OnPropertyChanged(nameof(GEGain));
                OnPropertyChanged(nameof(GCGain));
                OnPropertyChanged(nameof(GrowthCubesFromBg));
                ReloadCurrentHeroGrowth();
                RecalculateDaysReady();
            };

            ProfileManager.ProfileChanged += (sender, ae) =>
            {
                HeroPlans = new ObservableCollection<HeroPlan>();
                foreach (var hp in ProfileGrowth.Profile.HeroPlans)
                {
                    hp.Hero = Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == hp.HeroName && h.HeroType == hp.HeroType);
                    var phg = ProfileGrowth.Heroes.Where(p => p.Hero.HeroName == hp.HeroName && p.Hero.HeroType == hp.HeroType).FirstOrDefault();
                    if (hp.HeroName == HeroEnum.Custom)
                    {
                        hp.Hero = new Hero(hp.HeroName, hp.HeroType, hp.HeroClass, hp.HeroAttribute);
                        hp.CurrentGrowth = new GrowthPlan();
                    }
                    else
                        hp.CurrentGrowth = new GrowthPlan() { TranscendenceLevel = phg.TranscendenceLevel, ChaserLevel = phg.ChaserLevel, SiLevel = phg.SiLevel, IsCoreOpen = phg.IsCoreOpen, TraitsOpen = phg.TraitsOpen };

                    if (hp.Hero.HeroType == Enums.HeroType.T)
                    {
                        hp.CurrentGrowth.TranscendenceLevel = Math.Max(0, hp.CurrentGrowth.TranscendenceLevel);
                        hp.CurrentGrowth.ChaserLevel = Math.Max(20, hp.CurrentGrowth.ChaserLevel);
                    }
                    else if (hp.Hero.HeroType == Enums.HeroType.S)
                    {
                        hp.CurrentGrowth.TranscendenceLevel = Math.Max((int)Static.StaticValues.MaxTranscendenceLevel, hp.CurrentGrowth.TranscendenceLevel);
                        hp.CurrentGrowth.ChaserLevel = Math.Max((int)Static.StaticValues.MaxClLevel, hp.CurrentGrowth.ChaserLevel);
                        hp.CurrentGrowth.SiLevel = Math.Max((int)Static.StaticValues.MaxSiLevel, hp.CurrentGrowth.SiLevel);
                        hp.CurrentGrowth.TraitsOpen = 27;
                    }

                    hp.TCost = TranscendingCosts.CalculateCost(hp.CurrentGrowth.TranscendenceLevel, hp.DesiredGrowth.TranscendenceLevel);
                    hp.ClLevelCost = ChaserLevelingCosts.CalculateCost(hp.CurrentGrowth.ChaserLevel, hp.DesiredGrowth.ChaserLevel);
                    hp.SiLevelCost = SiLevelingCosts.CalculateCostWithTraits(hp.CurrentGrowth.SiLevel, hp.DesiredGrowth.SiLevel, hp.CurrentGrowth.TraitsOpen, hp.DesiredGrowth.TraitsOpen, hp.CurrentGrowth.IsCoreOpen, hp.DesiredGrowth.IsCoreOpen);

                    hp.SiLevelCost.GrowthCubesCost += hp.ClLevelCost.GrowthCubesCost;
                    hp.SiLevelCost.GeCost += hp.ClLevelCost.GrowthEssencesCost;

                    RecalculateSiCubesCost(hp, phg);

                    HeroPlans.Add(hp);
                }

                RecalculateDaysReady();
                OnPropertyChanged(string.Empty);
            };
        }

        private void CopyDetails(object param)
        {
            System.Windows.Clipboard.SetText(CalculationLog);
        }

        private async void AddNewHero(object param)
        {
            var view = new EditHeroPlan();

            var result = await DialogHost.Show(view, "PlanningDialog");
            if ((bool)result)
            {
                var hp = view.HeroPlan;
                hp.Hero = view.SelectedHero;
                hp.HeroName = view.SelectedHero.HeroName;
                hp.HeroType = view.SelectedHero.HeroType;
                hp.HeroAttribute = view.SelectedHero.HeroAttribute;
                hp.HeroClass = view.SelectedHero.HeroClass;

                var ownedHeroGrowth = ProfileGrowth.Heroes.FirstOrDefault(h => h.Hero.HeroName == hp.HeroName && h.Hero.HeroType == hp.HeroType);
                hp.CurrentGrowth = new GrowthPlan()
                {
                    TranscendenceLevel = ownedHeroGrowth?.TranscendenceLevel ?? (hp.HeroType == Enums.HeroType.SR ? 0 : 6),
                    ChaserLevel = ownedHeroGrowth?.ChaserLevel ?? (hp.HeroType == Enums.HeroType.SR ? 0 : 20),
                    SiLevel = ownedHeroGrowth?.SiLevel ?? 0,
                    TraitsOpen = ownedHeroGrowth?.TraitsOpen ?? 0,
                    IsCoreOpen = ownedHeroGrowth?.IsCoreOpen ?? false,
                };

                if (hp.HeroType == Enums.HeroType.T)
                {
                    hp.CurrentGrowth.ChaserLevel = Math.Max(20, hp.CurrentGrowth.ChaserLevel);
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max(0, hp.CurrentGrowth.TranscendenceLevel);
                }
                else if (hp.Hero.HeroType == Enums.HeroType.S)
                {
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max((int)Static.StaticValues.MaxTranscendenceLevel, hp.CurrentGrowth.TranscendenceLevel);
                    hp.CurrentGrowth.ChaserLevel = Math.Max((int)Static.StaticValues.MaxClLevel, hp.CurrentGrowth.ChaserLevel);
                    hp.CurrentGrowth.SiLevel = Math.Max((int)Static.StaticValues.MaxSiLevel, hp.CurrentGrowth.SiLevel);
                    hp.CurrentGrowth.TraitsOpen = 27;
                }

                if (hp.DesiredGrowth.TranscendenceLevel < hp.CurrentGrowth.TranscendenceLevel)
                    hp.DesiredGrowth.TranscendenceLevel = hp.CurrentGrowth.TranscendenceLevel;

                if (hp.DesiredGrowth.ChaserLevel < hp.CurrentGrowth.ChaserLevel)
                    hp.DesiredGrowth.ChaserLevel = hp.CurrentGrowth.ChaserLevel;

                if (hp.DesiredGrowth.SiLevel < hp.CurrentGrowth.SiLevel)
                    hp.DesiredGrowth.SiLevel = hp.CurrentGrowth.SiLevel;

                if (hp.DesiredGrowth.TraitsOpen < hp.CurrentGrowth.TraitsOpen)                
                    hp.DesiredGrowth.TraitsOpen = hp.CurrentGrowth.TraitsOpen;
                

                hp.TCost = TranscendingCosts.CalculateCost(hp.CurrentGrowth.TranscendenceLevel, hp.DesiredGrowth.TranscendenceLevel);
                hp.ClLevelCost = ChaserLevelingCosts.CalculateCost(hp.CurrentGrowth.ChaserLevel, hp.DesiredGrowth.ChaserLevel);
                hp.SiLevelCost = SiLevelingCosts.CalculateCostWithTraits(hp.CurrentGrowth.SiLevel, hp.DesiredGrowth.SiLevel, hp.CurrentGrowth.TraitsOpen, hp.DesiredGrowth.TraitsOpen, hp.CurrentGrowth.IsCoreOpen, hp.DesiredGrowth.IsCoreOpen);

                hp.SiLevelCost.GrowthCubesCost += hp.ClLevelCost.GrowthCubesCost;
                hp.SiLevelCost.GeCost += hp.ClLevelCost.GrowthEssencesCost;

                RecalculateSiCubesCost(hp, ownedHeroGrowth);

                HeroPlans.Add(hp);

                if (hp.HeroType == HeroType.T && hp.HeroName != HeroEnum.Custom)
                {
                    var baseHero = ProfileGrowth.Heroes.FirstOrDefault(x => x.Hero.HeroName == hp.HeroName && x.Hero.HeroType == HeroType.SR);
                    if (baseHero == null)
                    {
                        hp.BaseHeroNeedToBeBuilt = true;
                    }
                    else
                    {
                        hp.BaseHeroNeedToBeBuilt = baseHero.TranscendenceLevel < 1 || baseHero.ChaserLevel < 20;
                        if (hp.BaseHeroNeedToBeBuilt && HeroPlans.FirstOrDefault(x => x.Hero.HeroName == hp.HeroName && x.Hero.HeroType == HeroType.SR) is HeroPlan baseHeroPlan && baseHeroPlan != null)
                        {
                            hp.BaseHeroNeedToBeBuilt = HeroPlans.IndexOf(baseHeroPlan) > HeroPlans.IndexOf(hp);
                        }
                    }
                }

                if (hp.BaseHeroNeedToBeBuilt)
                {
                    result = await DialogHost.Show(new MessageBoxDialog() { DisplayText = Properties.Resources.YouHaveAddedTHero }, "PlanningDialog");
                    if ((bool)result)
                    {
                        var heroPlan = new HeroPlan();
                        heroPlan.Hero = ProfileGrowth.Heroes.FirstOrDefault(h => h.Hero.HeroType == HeroType.SR && h.Hero.HeroName == hp.HeroName)?.Hero;
                        heroPlan.HeroName = hp.HeroName;
                        heroPlan.HeroType = HeroType.SR;

                        ownedHeroGrowth = ProfileGrowth.Heroes.FirstOrDefault(h => h.Hero.HeroName == heroPlan.HeroName && h.Hero.HeroType == heroPlan.HeroType);
                        heroPlan.CurrentGrowth = new GrowthPlan()
                        {
                            TranscendenceLevel = ownedHeroGrowth?.TranscendenceLevel ?? (heroPlan.HeroType == Enums.HeroType.SR ? 0 : 0),
                            ChaserLevel = ownedHeroGrowth?.ChaserLevel ?? (heroPlan.HeroType == Enums.HeroType.SR ? 0 : 20),
                            SiLevel = ownedHeroGrowth?.SiLevel ?? 0,
                        };
                        heroPlan.DesiredGrowth = new GrowthPlan()
                        {
                            TranscendenceLevel = 6,
                            ChaserLevel = 20,
                            SiLevel = heroPlan.CurrentGrowth.SiLevel,
                        };

                        if (heroPlan.DesiredGrowth.TranscendenceLevel < heroPlan.CurrentGrowth.TranscendenceLevel)
                            heroPlan.DesiredGrowth.TranscendenceLevel = heroPlan.CurrentGrowth.TranscendenceLevel;

                        if (heroPlan.DesiredGrowth.ChaserLevel < heroPlan.CurrentGrowth.ChaserLevel)
                            heroPlan.DesiredGrowth.ChaserLevel = heroPlan.CurrentGrowth.ChaserLevel;

                        heroPlan.TCost = TranscendingCosts.CalculateCost(heroPlan.CurrentGrowth.TranscendenceLevel, heroPlan.DesiredGrowth.TranscendenceLevel);
                        heroPlan.ClLevelCost = ChaserLevelingCosts.CalculateCost(heroPlan.CurrentGrowth.ChaserLevel, heroPlan.DesiredGrowth.ChaserLevel);
                        heroPlan.SiLevelCost = SiLevelingCosts.CalculateCostWithTraits(heroPlan.CurrentGrowth.SiLevel, heroPlan.DesiredGrowth.SiLevel, heroPlan.CurrentGrowth.TraitsOpen, heroPlan.DesiredGrowth.TraitsOpen, heroPlan.CurrentGrowth.IsCoreOpen, heroPlan.DesiredGrowth.IsCoreOpen);

                        HeroPlans.Insert(HeroPlans.IndexOf(hp), heroPlan);
                    }
                }

                RecalculateDaysReady();
            }
        }

        private async void EditHeroPlan(object param)
        {
            var heroPlan = (HeroPlan)param;
            var editableHeroPlan = new HeroPlan()
            {
                HeroName = heroPlan.HeroName,
                HeroType = heroPlan.HeroType,
                Hero = heroPlan.Hero,
                CurrentGrowth = new GrowthPlan()
                {
                    TranscendenceLevel = heroPlan.CurrentGrowth.TranscendenceLevel,
                    ChaserLevel = heroPlan.CurrentGrowth.ChaserLevel,
                    SiLevel = heroPlan.CurrentGrowth.SiLevel,
                    IsCoreOpen = (new[] { 0, 5, 10 }).Any(l => l == heroPlan.CurrentGrowth.SiLevel) && heroPlan.CurrentGrowth.IsCoreOpen,
                    TraitsOpen = heroPlan.CurrentGrowth.TraitsOpen,
                },
                DesiredGrowth = new GrowthPlan()
                {
                    TranscendenceLevel = heroPlan.DesiredGrowth.TranscendenceLevel,
                    ChaserLevel = heroPlan.DesiredGrowth.ChaserLevel,
                    SiLevel = heroPlan.DesiredGrowth.SiLevel,
                    DupesForSi = heroPlan.DesiredGrowth.DupesForSi,
                    IsCoreOpen = heroPlan.DesiredGrowth.IsCoreOpen,
                    HeroSpecificSiCubesOwned = heroPlan.DesiredGrowth.HeroSpecificSiCubesOwned,
                    TraitsOpen = heroPlan.DesiredGrowth.TraitsOpen,
                },
            };

            var sh = Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == heroPlan.HeroName && h.HeroType == heroPlan.HeroType);
            var view = new EditHeroPlan()
            {                
                SelectedHero = sh ?? new Hero(HeroEnum.Custom, HeroType.SR, HeroClass.Assault, HeroAttribute.Blue),
                HeroPlan = editableHeroPlan,
                Step = WizardStep.Second,
            };

            var result = await DialogHost.Show(view, "PlanningDialog");
            if ((bool)result)
            {
                var hp = view.HeroPlan;
                hp.Hero = view.SelectedHero;
                hp.HeroName = view.SelectedHero.HeroName;
                hp.HeroType = view.SelectedHero.HeroType;

                var ownedHeroGrowth = ProfileGrowth.Heroes.FirstOrDefault(h => h.Hero.HeroName == hp.HeroName && h.Hero.HeroType == hp.HeroType);
                if (ownedHeroGrowth != null)
                {
                    hp.CurrentGrowth = new GrowthPlan()
                    {
                        TranscendenceLevel = ownedHeroGrowth.TranscendenceLevel,
                        ChaserLevel = ownedHeroGrowth.ChaserLevel,
                        SiLevel = ownedHeroGrowth.SiLevel,
                        TraitsOpen = ownedHeroGrowth.TraitsOpen,
                        IsCoreOpen = ownedHeroGrowth.IsCoreOpen,
                    };
                }
                else 
                {
                    hp.CurrentGrowth = new GrowthPlan();
                }

                if (hp.HeroType == Enums.HeroType.T)
                {
                    hp.CurrentGrowth.ChaserLevel = Math.Max(20, hp.CurrentGrowth.ChaserLevel);
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max(0, hp.CurrentGrowth.TranscendenceLevel);
                }
                else if (hp.Hero.HeroType == Enums.HeroType.S)
                {
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max((int)Static.StaticValues.MaxTranscendenceLevel, hp.CurrentGrowth.TranscendenceLevel);
                    hp.CurrentGrowth.ChaserLevel = Math.Max((int)Static.StaticValues.MaxClLevel, hp.CurrentGrowth.ChaserLevel);
                    hp.CurrentGrowth.SiLevel = Math.Max((int)Static.StaticValues.MaxSiLevel, hp.CurrentGrowth.SiLevel);
                    hp.CurrentGrowth.TraitsOpen = 27;
                }

                if (hp.DesiredGrowth.TranscendenceLevel < hp.CurrentGrowth.TranscendenceLevel)
                    hp.DesiredGrowth.TranscendenceLevel = hp.CurrentGrowth.TranscendenceLevel;

                if (hp.DesiredGrowth.ChaserLevel < hp.CurrentGrowth.ChaserLevel)
                    hp.DesiredGrowth.ChaserLevel = hp.CurrentGrowth.ChaserLevel;

                if (hp.DesiredGrowth.SiLevel < hp.CurrentGrowth.SiLevel)
                    hp.DesiredGrowth.SiLevel = hp.CurrentGrowth.SiLevel;

                hp.TCost = TranscendingCosts.CalculateCost(hp.CurrentGrowth.TranscendenceLevel, hp.DesiredGrowth.TranscendenceLevel);
                hp.ClLevelCost = ChaserLevelingCosts.CalculateCost(hp.CurrentGrowth.ChaserLevel, hp.DesiredGrowth.ChaserLevel);
                hp.SiLevelCost = SiLevelingCosts.CalculateCostWithTraits(hp.CurrentGrowth.SiLevel, hp.DesiredGrowth.SiLevel, hp.CurrentGrowth.TraitsOpen, hp.DesiredGrowth.TraitsOpen, hp.CurrentGrowth.IsCoreOpen, hp.DesiredGrowth.IsCoreOpen);

                // This is right
                hp.SiLevelCost.GrowthCubesCost += hp.ClLevelCost.GrowthCubesCost;
                hp.SiLevelCost.GeCost += hp.ClLevelCost.GrowthEssencesCost;

                RecalculateSiCubesCost(hp, ownedHeroGrowth);

                HeroPlans.Insert(HeroPlans.IndexOf(heroPlan), hp);
                HeroPlans.Remove(heroPlan);

                RecalculateDaysReady();
            }
        }

        private void RecalculateSiCubesCost(HeroPlan hp, HeroGrowth ownedHeroGrowth)
        {
            if (hp.HeroType == Enums.HeroType.T && (ownedHeroGrowth?.IsOwned == false || hp.HeroName == HeroEnum.Custom))
            {
                hp.SiLevelCost.GrowthCubesCost += 250;
            }

            hp.SiLevelCost.GrowthCubesCost = hp.SiLevelCost.GrowthCubesCost - (hp.DesiredGrowth.DupesForSi * 250) - hp.DesiredGrowth.HeroSpecificSiCubesOwned;
            if (hp.SiLevelCost.GrowthCubesCost < 0)
            {
                hp.SiLevelCost.GrowthCubesCost = 0;
            }

            if ((hp.DesiredGrowth.SiLevel > hp.CurrentGrowth.SiLevel && (new[] { 5, 10 }).Any(l => l == hp.DesiredGrowth.SiLevel) && hp.DesiredGrowth.IsCoreOpen) ||
                (hp.DesiredGrowth.SiLevel == hp.CurrentGrowth.SiLevel && (new[] { 5, 10 }).Any(l => l == hp.DesiredGrowth.SiLevel) && hp.DesiredGrowth.IsCoreOpen && !hp.CurrentGrowth.IsCoreOpen))
            {
                //hp.SiLevelCost.SiCubesCost += 250;
            }

            if ((new[] { 5, 10 }).Any(l => l == hp.CurrentGrowth.SiLevel) && hp.CurrentGrowth.IsCoreOpen)
            {
                //hp.SiLevelCost.SiCubesCost -= 250;
            }

            hp.SiLevelCost = new SiLevelCost(hp.SiLevelCost.Level, hp.SiLevelCost.GrowthCubesCost < 0 ? 0 : hp.SiLevelCost.GrowthCubesCost, hp.SiLevelCost.GeCost, hp.SiLevelCost.GoldCost);
        }

        private void DeleteHeroPlan(object param)
        {
            var heroPlan = (HeroPlan)param;
            if (HeroPlans.Contains(heroPlan))
            {
                HeroPlans.Remove(heroPlan);
            }

            RecalculateDaysReady();
        }

        private void ReloadCurrentHeroGrowth()
        {
            foreach (var hp in ProfileGrowth.Profile.HeroPlans)
            {
                hp.Hero = Hero.GetHeroesCollection.FirstOrDefault(h => h.HeroName == hp.HeroName && h.HeroType == hp.HeroType);
                var phg = ProfileGrowth.Heroes.Where(p => p.Hero.HeroName == hp.HeroName && p.Hero.HeroType == hp.HeroType).FirstOrDefault();

                if (hp.HeroName == HeroEnum.Custom)
                {
                    hp.Hero = new Hero(hp.HeroName, hp.HeroType, hp.HeroClass, hp.HeroAttribute);
                    hp.CurrentGrowth = new GrowthPlan();
                    phg = new HeroGrowth();
                }

                hp.CurrentGrowth.SiLevel = phg.SiLevel;
                hp.CurrentGrowth.ChaserLevel = phg.ChaserLevel;
                hp.CurrentGrowth.TranscendenceLevel = phg.TranscendenceLevel;
                hp.CurrentGrowth.IsCoreOpen = phg.IsCoreOpen;

                if (hp.Hero.HeroType == Enums.HeroType.T)
                {
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max(6, phg.TranscendenceLevel);
                    hp.CurrentGrowth.ChaserLevel = Math.Max(20, phg.ChaserLevel);
                }
                else if (hp.Hero.HeroType == Enums.HeroType.S)
                {
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max((int)Static.StaticValues.MaxTranscendenceLevel, phg.TranscendenceLevel);
                    hp.CurrentGrowth.ChaserLevel = Math.Max((int)Static.StaticValues.MaxClLevel, phg.ChaserLevel);
                    hp.CurrentGrowth.SiLevel = Math.Max((int)Static.StaticValues.MaxSiLevel, phg.SiLevel);
                    hp.CurrentGrowth.TraitsOpen = 27;
                }

                hp.TCost = TranscendingCosts.CalculateCost(hp.CurrentGrowth.TranscendenceLevel, hp.DesiredGrowth.TranscendenceLevel);
                hp.ClLevelCost = ChaserLevelingCosts.CalculateCost(hp.CurrentGrowth.ChaserLevel, hp.DesiredGrowth.ChaserLevel);
                hp.SiLevelCost = SiLevelingCosts.CalculateCostWithTraits(hp.CurrentGrowth.SiLevel, hp.DesiredGrowth.SiLevel, hp.CurrentGrowth.TraitsOpen, hp.DesiredGrowth.TraitsOpen, hp.CurrentGrowth.IsCoreOpen, hp.DesiredGrowth.IsCoreOpen);
                
                hp.SiLevelCost.GrowthCubesCost += hp.ClLevelCost.GrowthCubesCost;
                hp.SiLevelCost.GeCost += hp.ClLevelCost.GrowthEssencesCost;

                RecalculateSiCubesCost(hp, phg);
            }
        }

        private void RecalculateDaysReady()
        {
            var profileInventory = ProfileGrowth.Profile.MaterialsInventory;
            var currentInventory = new Inventory()
            {
                GrowthCubes = profileInventory.GrowthCubes + profileInventory.GrowthCubesFromBlueGems,
                AssaultGE = profileInventory.AssaultGE,
                RangerGE = profileInventory.RangerGE,
                MageGE = profileInventory.MageGE,
                TankGE = profileInventory.TankGE,
                HealerGE = profileInventory.HealerGE,

                Gold = profileInventory.Gold,
            };

            double GrowthCubesLootingDays = 0;
            double GeLootingDays = 0;

            calcLog = new StringBuilder();

            foreach (var hp in HeroPlans)
            {      
                if (hp.HeroType == HeroType.T)
                {
                    var baseHero = ProfileGrowth.Heroes.FirstOrDefault(x => x.Hero.HeroName == hp.HeroName && x.Hero.HeroType == HeroType.SR);
                    if (baseHero == null)
                    {
                        hp.BaseHeroNeedToBeBuilt = true;
                    }
                    else
                    {
                        hp.BaseHeroNeedToBeBuilt = baseHero.TranscendenceLevel < 6 || baseHero.ChaserLevel < 20;
                        if (hp.BaseHeroNeedToBeBuilt && HeroPlans.FirstOrDefault(x => x.Hero.HeroName == hp.HeroName && x.Hero.HeroType == HeroType.SR) is HeroPlan baseHeroPlan && baseHeroPlan != null)
                        {
                            hp.BaseHeroNeedToBeBuilt = HeroPlans.IndexOf(baseHeroPlan) > HeroPlans.IndexOf(hp);
                        }
                    }
                }

                calcLog.AppendLine("==========================================================================================\r\n");
                calcLog.AppendLine($"Doing calculations for: {hp.ToString()}\r\n");

                calcLog.AppendLine("Inventory Before:");
                calcLog.AppendLine($"Growth Cubes: {currentInventory.GrowthCubes}\r\n");

                hp.ChaserCubesNeeded = currentInventory.GrowthCubes;

                double daysForGCBase = currentInventory.GrowthCubes >= 0 ? 0 : -1 * currentInventory.GrowthCubes / ((double)GrowthCubes.CubesTotalWeekly / 7);

                if (hp.SiLevelCost?.GrowthCubesCost > 0 && hp.SiLevelCost?.GrowthCubesCost > currentInventory.GrowthCubes)
                    hp.DaysForSi = (double)(hp.SiLevelCost.GrowthCubesCost - currentInventory.GrowthCubes) / ((double)GrowthCubes.CubesTotalWeekly / 7) + (daysForGCBase < 0 ? 0 : daysForGCBase);
                else
                    hp.DaysForSi = 0;
                
                calcLog.AppendLine($"Growth Cubes cost: {hp.SiLevelCost?.GrowthCubesCost}. Growth Cubes owned: {currentInventory.GrowthCubes}");
                if (hp.SiLevelCost?.GrowthCubesCost - currentInventory.GrowthCubes <= 0)
                {
                    calcLog.AppendLine("Growth Cubes ready!\r\n");
                }
                else
                {
                    calcLog.AppendLine($"Growth Cubes days to farm: {hp.SiLevelCost.GrowthCubesCost} - {currentInventory.GrowthCubes} / {(double)GrowthCubes.CubesTotalWeekly / 7} = {hp.DaysForSi})\r\n");
                }

                currentInventory.GrowthCubes -= hp.SiLevelCost?.GrowthCubesCost ?? 0;
                hp.SiNeeded = currentInventory.GrowthCubes;

                hp.GoldCost = hp.TCost.GoldCost + hp.SiLevelCost.GoldCost + hp.ClLevelCost.GoldCost;
                hp.DaysForGold = (double)(hp.GoldCost - currentInventory.Gold) / ((double)DailyGoldIncome);
                currentInventory.Gold -= hp.GoldCost;
                hp.GoldNeeded = currentInventory.Gold;

                var GeNeeded = 0;
                Calculate(hp, ref currentInventory, ref GeNeeded, hp.Hero.HeroClass);

                calcLog.AppendLine("Inventory After:");
                calcLog.AppendLine($"Growth Cubes: {currentInventory.GrowthCubes}\r\n");

                calcLog.AppendLine($"Growth Essence: \t\t\tAssault: {currentInventory.AssaultGE} \t\t\tRanger: {currentInventory.RangerGE} \t\t\tMage: {currentInventory.MageGE} \t\t\tTank: {currentInventory.TankGE} \t\t\tHealer: {currentInventory.HealerGE}\r\n");

                GeLootingDays = Math.Max(hp.DaysForGE, GeLootingDays);
                calcLog.AppendLine($"GE will be ready after: {GeLootingDays}\r\n");

                if (hp.DaysForSi > 0)
                    GrowthCubesLootingDays = hp.DaysForSi;

                calcLog.AppendLine($"Growth cubes will be ready after: {GrowthCubesLootingDays}\r\n");
            }

            ProfileGrowth.Profile.HeroPlans = new List<HeroPlan>(HeroPlans);
            ProfileGrowth.Profile.SaveToJson();

            CalculationLog = calcLog.ToString();
        }

        private void Calculate(HeroPlan hp, ref Inventory currentInventory, ref int GENeeded, HeroClass heroClass)
        {

            hp.DaysForGE = (double)(hp.SiLevelCost.GeCost - currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.GE }]) / ((double)GrowthEssences.GrowthEssenceTotalWeekly / 7);

            calcLog.AppendLine($"GE cost: {hp.SiLevelCost.GeCost}. GE owned: {Math.Max(currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.GE }], 0)}");
            if (hp.SiLevelCost.GeCost - Math.Max(currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.GE }], 0) <= 0)
            {
                calcLog.AppendLine("GE ready!\r\n");
            }
            else
            {
                calcLog.AppendLine($"GE days to farm: {hp.SiLevelCost.GeCost} - {currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.GE }]} / {(double)GrowthEssences.GrowthEssenceTotalWeekly / 7} = {hp.DaysForGE}\r\n");
            }

            currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.GE }] -= hp.SiLevelCost.GeCost;
            hp.GeNeeded = currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.GE }];


        }

        private void ReorderListBox_ReorderRequested(object sender, ReorderEventArgs e)
        {
            var reorderListBox = (ReorderListBox)e.OriginalSource;

            var draggingElement = (HeroPlan)reorderListBox.ItemContainerGenerator.ItemFromContainer(e.ItemContainer);
            var toElement = (HeroPlan)reorderListBox.ItemContainerGenerator.ItemFromContainer(e.ToContainer);

            HeroPlans.Move(HeroPlans.IndexOf(draggingElement), HeroPlans.IndexOf(toElement));

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
