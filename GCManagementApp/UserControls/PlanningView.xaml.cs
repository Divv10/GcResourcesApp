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

        public int SIGain => SoulImprintCubes.CubesTotalWeekly / 7;
        public int ACGain => AwakeningCubes.TotalCubesWeekly / 7;
        public int SEGain => SoulEssences.SoulEssencesTotalWeekly / 7 ;
        public int CCGain => ChaserCrystals.ChaserCrystalsTotalWeekly / 7 ;
        public int SiCubesFromBg => Inventory.SiCubesFromBlueGems;
        public int SiCubesFromAnni => Inventory.SiCubesFromAnniCoins;

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
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max(6, hp.CurrentGrowth.TranscendenceLevel);
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
                OnPropertyChanged(nameof(ACGain));
                OnPropertyChanged(nameof(SEGain));
                OnPropertyChanged(nameof(SIGain));
                OnPropertyChanged(nameof(CCGain)); 
                OnPropertyChanged(nameof(SiCubesFromAnni));
                OnPropertyChanged(nameof(SiCubesFromBg));
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
                        hp.CurrentGrowth.TranscendenceLevel = Math.Max(6, hp.CurrentGrowth.TranscendenceLevel);
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
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max(6, hp.CurrentGrowth.TranscendenceLevel);
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
                        hp.BaseHeroNeedToBeBuilt = baseHero.TranscendenceLevel < 6 || baseHero.ChaserLevel < 20;
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
                            TranscendenceLevel = ownedHeroGrowth?.TranscendenceLevel ?? (heroPlan.HeroType == Enums.HeroType.SR ? 0 : 6),
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
                    hp.CurrentGrowth.TranscendenceLevel = Math.Max(6, hp.CurrentGrowth.TranscendenceLevel);
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
                hp.SiLevelCost.SiCubesCost += 250;
            }

            hp.SiLevelCost.SiCubesCost = hp.SiLevelCost.SiCubesCost - (hp.DesiredGrowth.DupesForSi * 250) - hp.DesiredGrowth.HeroSpecificSiCubesOwned;
            if (hp.SiLevelCost.SiCubesCost < 0)
            {
                hp.SiLevelCost.SiCubesCost = 0;
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

            hp.SiLevelCost = new SiLevelCost(hp.SiLevelCost.Level, hp.SiLevelCost.SiCubesCost < 0 ? 0 : hp.SiLevelCost.SiCubesCost, hp.SiLevelCost.SeCost, hp.SiLevelCost.GoldCost);
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

                RecalculateSiCubesCost(hp, phg);
            }
        }

        private void RecalculateDaysReady()
        {
            var profileInventory = ProfileGrowth.Profile.MaterialsInventory;
            var currentInventory = new Inventory()
            {
                ChaserCubes = profileInventory.ChaserCubes,
                AssaultCC = profileInventory.AssaultCC,
                RangerCC = profileInventory.RangerCC,
                MageCC = profileInventory.MageCC,
                TankCC = profileInventory.TankCC,
                HealerCC = profileInventory.HealerCC,

                SiCubes = profileInventory.SiCubes + profileInventory.SiCubesFromBlueGems + profileInventory.SiCubesFromAnniCoins,
                AssaultSE = profileInventory.AssaultSE,
                RangerSE = profileInventory.RangerSE,
                MageSE = profileInventory.MageSE,
                TankSE = profileInventory.TankSE,
                HealerSE = profileInventory.HealerSE,

                AssaultAC = profileInventory.AssaultAC,
                RangerAC = profileInventory.RangerAC,
                MageAC = profileInventory.MageAC,
                TankAC = profileInventory.TankAC,
                HealerAC = profileInventory.HealerAC,

                Gold = profileInventory.Gold,
            };

            double ccAndSiCubesLootingDays = 0;
            double acLootingDays = 0;
            double ccAndSeLootingDays = 0;

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
                calcLog.AppendLine($"Chaser Cubes: {currentInventory.ChaserCubes}\r\n");

                calcLog.AppendLine($"SI Cubes: {currentInventory.SiCubes}\r\n");

                calcLog.AppendLine($"Awakening Cubes: \t\t\tAssault: {currentInventory.AssaultAC} \t\t\tRanger: {currentInventory.RangerAC} \t\t\tMage: {currentInventory.MageAC} \t\t\tTank: {currentInventory.TankAC} \t\t\tHealer: {currentInventory.HealerAC}\r\n");

                calcLog.AppendLine($"Chaser Crystals: \t\t\tAssault: {currentInventory.AssaultCC} \t\t\tRanger: {currentInventory.RangerCC} \t\t\tMage: {currentInventory.MageCC} \t\t\tTank: {currentInventory.TankCC} \t\t\tHealer: {currentInventory.HealerCC}\r\n");

                calcLog.AppendLine($"Soul Essence: \t\t\tAssault: {currentInventory.AssaultSE} \t\t\tRanger: {currentInventory.RangerSE} \t\t\tMage: {currentInventory.MageSE} \t\t\tTank: {currentInventory.TankSE} \t\t\tHealer: {currentInventory.HealerSE}\r\n");

                currentInventory.ChaserCubes -= hp.ClLevelCost.ChaserCubesCost;
                hp.ChaserCubesNeeded = currentInventory.ChaserCubes;

                double daysForCCBase = currentInventory.ChaserCubes >= 0 ? 0 : -1 * currentInventory.ChaserCubes / ((double)ChaserCubes.TotalCubesWeekly / 7);
                if (hp.ClLevelCost.ChaserCubesCost > 0 && hp.ClLevelCost.ChaserCubesCost > currentInventory.ChaserCubes)
                    hp.DaysForChaserCubes = daysForCCBase + ccAndSiCubesLootingDays;
                else
                    hp.DaysForChaserCubes = 0;


                calcLog.AppendLine($"Chaser Cubes cost: {hp.ClLevelCost.ChaserCubesCost}. Chaser Cubes owned: {currentInventory.ChaserCubes}");
                if (hp.ClLevelCost.ChaserCubesCost - currentInventory.ChaserCubes <= 0)
                {
                    calcLog.AppendLine("Chaser Cubes ready!\r\n");
                    //hp.DaysForChaserCubes = 0;
                }
                else
                {
                    calcLog.AppendLine($"Chaser Cubes days to farm: {hp.ClLevelCost.ChaserCubesCost} / {(double)ChaserCubes.TotalCubesWeekly / 7} = {daysForCCBase} (+ CC and SI days from prev heroes ({ccAndSiCubesLootingDays}): {daysForCCBase + ccAndSiCubesLootingDays}\r\n");
                }


                if (hp.SiLevelCost?.SiCubesCost > 0 && hp.SiLevelCost?.SiCubesCost > currentInventory.SiCubes)
                    hp.DaysForSi = (double)(hp.SiLevelCost.SiCubesCost - currentInventory.SiCubes) / ((double)SoulImprintCubes.CubesTotalWeekly / 7) + (daysForCCBase < 0 ? 0 : daysForCCBase);
                else
                    hp.DaysForSi = 0;
                
                calcLog.AppendLine($"SI Cubes cost: {hp.SiLevelCost?.SiCubesCost}. SI Cubes owned: {currentInventory.SiCubes}");
                if (hp.SiLevelCost?.SiCubesCost - currentInventory.SiCubes <= 0)
                {
                    calcLog.AppendLine("SI Cubes ready!\r\n");
                }
                else
                {
                    calcLog.AppendLine($"SI Cubes days to farm: {hp.SiLevelCost.SiCubesCost} - {currentInventory.SiCubes} / {(double)SoulImprintCubes.CubesTotalWeekly / 7} = {hp.DaysForSi} (+ Chaser cubes days: {(daysForCCBase < 0 ? 0 : daysForCCBase)})\r\n");
                }

                currentInventory.SiCubes -= hp.SiLevelCost?.SiCubesCost ?? 0;
                hp.SiNeeded = currentInventory.SiCubes;

                hp.GoldCost = hp.TCost.GoldCost + hp.SiLevelCost.GoldCost + hp.ClLevelCost.GoldCost;
                hp.DaysForGold = (double)(hp.GoldCost - currentInventory.Gold) / ((double)DailyGoldIncome);
                currentInventory.Gold -= hp.GoldCost;
                hp.GoldNeeded = currentInventory.Gold;

                var SeNeeded = 0;
                Calculate(hp, ref currentInventory, ref SeNeeded, hp.Hero.HeroClass);

                calcLog.AppendLine("Inventory After:");
                calcLog.AppendLine($"SI Cubes: {currentInventory.SiCubes}\r\n");

                calcLog.AppendLine($"Awakening Cubes: \t\t\tAssault: {currentInventory.AssaultAC} \t\t\tRanger: {currentInventory.RangerAC} \t\t\tMage: {currentInventory.MageAC} \t\t\tTank: {currentInventory.TankAC} \t\t\tHealer: {currentInventory.HealerAC}\r\n");

                calcLog.AppendLine($"Chaser Crystals: \t\t\tAssault: {currentInventory.AssaultCC} \t\t\tRanger: {currentInventory.RangerCC} \t\t\tMage: {currentInventory.MageCC} \t\t\tTank: {currentInventory.TankCC} \t\t\tHealer: {currentInventory.HealerCC}\r\n");

                calcLog.AppendLine($"Soul Essence: \t\t\tAssault: {currentInventory.AssaultSE} \t\t\tRanger: {currentInventory.RangerSE} \t\t\tMage: {currentInventory.MageSE} \t\t\tTank: {currentInventory.TankSE} \t\t\tHealer: {currentInventory.HealerSE}\r\n");

                if (hp.DaysForAC > 0)
                {
                    hp.DaysForAC += acLootingDays;
                }

                acLootingDays = hp.DaysForAC;
                calcLog.AppendLine($"AC will be ready after: {acLootingDays}\r\n");

                if (hp.DaysForCC > 0)
                {
                    hp.DaysForCC += ccAndSeLootingDays;
                }

                if (hp.DaysForSE > 0)
                    hp.DaysForSE += hp.DaysForCC > 0 ? hp.DaysForCC : ccAndSeLootingDays;

                ccAndSeLootingDays = Math.Max(Math.Max(hp.DaysForSE, hp.DaysForCC), ccAndSeLootingDays);
                calcLog.AppendLine($"CC & SE will be ready after: {ccAndSeLootingDays}\r\n");

                if (hp.DaysForSi > 0)
                    ccAndSiCubesLootingDays = hp.DaysForSi;

                calcLog.AppendLine($"C & SI cubes will be ready after: {ccAndSiCubesLootingDays}\r\n");
            }

            ProfileGrowth.Profile.HeroPlans = new List<HeroPlan>(HeroPlans);
            ProfileGrowth.Profile.SaveToJson();

            CalculationLog = calcLog.ToString();
        }

        private void Calculate(HeroPlan hp, ref Inventory currentInventory, ref int SeNeeded, HeroClass heroClass)
        {
            hp.DaysForAC = (double)(hp.TCost.AwakeningCubesCost - Math.Max(currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.AC }], 0)) / ((double)AwakeningCubes.TotalCubesWeekly / 7);

            calcLog.AppendLine($"AC cost: {hp.TCost.AwakeningCubesCost}. AC owned: {Math.Max(currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.AC }], 0)}");
            if (hp.TCost.AwakeningCubesCost - Math.Max(currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.AC }], 0) <= 0)
            {
                calcLog.AppendLine("AC ready!\r\n");
            }
            else
            {
                calcLog.AppendLine($"AC days to farm: {hp.TCost.AwakeningCubesCost} - {Math.Max(currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.AC }], 0)} / {(double)AwakeningCubes.TotalCubesWeekly / 7} = {hp.DaysForAC}\r\n");
            }

            currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.AC}] -= hp.TCost.AwakeningCubesCost;
            hp.AcNeeded = currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.AC }];

            hp.DaysForCC =(double)(hp.ClLevelCost.ChaserCrystalsCost - currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.CC }]) / ((double)ChaserCrystals.ChaserCrystalsTotalWeekly / 7);

            calcLog.AppendLine($"CC cost: {hp.ClLevelCost.ChaserCrystalsCost}. CC owned: {currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.CC }]}");
            if (hp.ClLevelCost.ChaserCrystalsCost - currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.CC }] <= 0)
            {
                calcLog.AppendLine("CC ready!\r\n");
            }
            else
            {
                calcLog.AppendLine($"CC days to farm: {hp.ClLevelCost.ChaserCrystalsCost} - {currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.CC }]} / {(double)ChaserCrystals.ChaserCrystalsTotalWeekly / 7} = {hp.DaysForCC}\r\n");
            }

            currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.CC }] -= hp.ClLevelCost.ChaserCrystalsCost;
            hp.CcNeeded = currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.CC }];

            SeNeeded = hp.SiLevelCost.SeCost - currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.SE }];
            calcLog.AppendLine($"SE cost: {hp.SiLevelCost.SeCost}. SE owned: {currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.SE }]}");
            if (SeNeeded > 0)
            {
                calcLog.AppendLine($"Need {SeNeeded} SE");
                int seAvailable = currentInventory.CraftableSoulEssences > SeNeeded ? SeNeeded : currentInventory.CraftableSoulEssences;

                calcLog.AppendLine($"Available {seAvailable} SE from CC craft");

                currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.SE }] += seAvailable;

                currentInventory.AssaultCC = Math.Max(currentInventory.AssaultCC - seAvailable, 0);
                currentInventory.RangerCC = Math.Max(currentInventory.RangerCC - seAvailable, 0);
                currentInventory.MageCC = Math.Max(currentInventory.MageCC - seAvailable, 0);
                currentInventory.TankCC = Math.Max(currentInventory.TankCC - seAvailable, 0);
                currentInventory.HealerCC = Math.Max(currentInventory.HealerCC - seAvailable, 0);

                calcLog.AppendLine($"Chaser Crystals after crafting SE: \t\t\tAssault: {currentInventory.AssaultCC} \t\t\tRanger: {currentInventory.RangerCC} \t\t\tMage: {currentInventory.MageCC} \t\t\tTank: {currentInventory.TankCC} \t\t\tHealer: {currentInventory.HealerCC}\r\n");
            }

            var seToCraft = Math.Max(hp.SiLevelCost.SeCost - Math.Max(currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.SE }], 0), 0);
            calcLog.AppendLine($"Remaining SE to farm: {seToCraft}");
            
            var totalCcToFarm = 0;
            if (seToCraft > 0)
            {
                var cc = new[] { currentInventory.AssaultCC, currentInventory.RangerCC, currentInventory.MageCC, currentInventory.TankCC, currentInventory.HealerCC };
                for (int i = 0; i < cc.Length; i++)
                {
                    cc[i] -= seToCraft;
                    if (cc[i] < 0)
                    {
                        totalCcToFarm += Math.Abs(cc[i]);
                        cc[i] = 0;
                    }
                }

                currentInventory.AssaultCC = cc[0];
                currentInventory.RangerCC = cc[1];
                currentInventory.MageCC = cc[2];
                currentInventory.TankCC = cc[3];
                currentInventory.HealerCC = cc[4];
            }

            calcLog.AppendLine($"Needed CC to farm: {totalCcToFarm}");

            hp.DaysForSE = (double)totalCcToFarm / ((double)ChaserCrystals.ChaserCrystalsTotalWeekly / 7d);
            //hp.DaysForSE = (int)Math.Ceiling((double)(hp.SiLevelCost.SeCost - currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.SE }]) / ((double)SoulEssences.SoulEssencesTotalWeekly / 7));
            currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.SE }] -= hp.SiLevelCost.SeCost;
            hp.SeNeeded = currentInventory[new InventoryType { heroClass = heroClass, materialType = MaterialType.SE }];
            calcLog.AppendLine($"SE days to farm: {totalCcToFarm} / {(double)ChaserCrystals.ChaserCrystalsTotalWeekly / 7d} = {hp.DaysForSE}\r\n");
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
