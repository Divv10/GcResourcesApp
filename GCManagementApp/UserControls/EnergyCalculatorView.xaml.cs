using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for EnergyCalculatorView.xaml
    /// </summary>
    public partial class EnergyCalculatorView : UserControl, INotifyPropertyChanged
    {
        public int EnergyShopAds
        {
            get => ProfileGrowth.Profile.Settings.EnergyAdsShop;
            set 
            { 
                ProfileGrowth.Profile.Settings.EnergyAdsShop = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public int VulcaRanksClear
        {
            get => ProfileGrowth.Profile.Settings.VulcaRanksClear;
            set
            {
                ProfileGrowth.Profile.Settings.VulcaRanksClear = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public VulcanusRankEnum VulcanusRank
        {
            get => ProfileGrowth.Profile.Settings.VulcaRankTier;
            set
            {
                ProfileGrowth.Profile.Settings.VulcaRankTier = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsEnergyPackBought
        {
            get => ProfileGrowth.Profile.Settings.IsExtraEnergyBought;
            set
            {
                ProfileGrowth.Profile.Settings.IsExtraEnergyBought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsEnergySupportPackageBought
        {
            get => ProfileGrowth.Profile.Settings.EnergySupportPackageBought;
            set
            {
                ProfileGrowth.Profile.Settings.EnergySupportPackageBought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsEnergySupportPackage2Bought
        {
            get => ProfileGrowth.Profile.Settings.EnergySupportPackage2Bought;
            set
            {
                ProfileGrowth.Profile.Settings.EnergySupportPackage2Bought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsDailySpecUpPackBought
        {
            get => ProfileGrowth.Profile.Settings.DailySpecUpPackageBought;
            set
            {
                ProfileGrowth.Profile.Settings.DailySpecUpPackageBought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsSpecialEnergyPackPremiumBought
        {
            get => ProfileGrowth.Profile.Settings.SpecialEnergyPremiumPackageBought;
            set
            {
                ProfileGrowth.Profile.Settings.SpecialEnergyPremiumPackageBought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsSpecialEnergyPackBasicBought
        {
            get => ProfileGrowth.Profile.Settings.SpecialEnergyBasicPackageBought;
            set
            {
                ProfileGrowth.Profile.Settings.SpecialEnergyBasicPackageBought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsEnergySupplyPackageBought
        {
            get => ProfileGrowth.Profile.Settings.EnergySupplyPackageBought;
            set
            {
                ProfileGrowth.Profile.Settings.EnergySupplyPackageBought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsEnergyForGemsBought
        {
            get => ProfileGrowth.Profile.Settings.EnergyFromGemsBought;
            set
            {
                ProfileGrowth.Profile.Settings.EnergyFromGemsBought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public int TotalEnergyBought
        {
            get => ProfileGrowth.Profile.Settings.TotalEnergyBought;
            set
            {
                ProfileGrowth.Profile.Settings.TotalEnergyBought = value;
                OnPropertyChanged(string.Empty);
            }
        }        

        public bool OverrideBlueGems
        {
            get => ProfileGrowth.Profile.Settings.OverrideBlueGems;
            set
            {
                ProfileGrowth.Profile.Settings.OverrideBlueGems = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public int CustomBlueGems
        {
            get => ProfileGrowth.Profile.Settings.CustomBlueGems;
            set
            {
                ProfileGrowth.Profile.Settings.CustomBlueGems = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public SellRarityTypeEnum SellRarityType
        {
            get => ProfileGrowth.Profile.Settings.RaritySellUpgradeType;
            set
            {
                ProfileGrowth.Profile.Settings.RaritySellUpgradeType = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsAnniGemReset
        {
            get => ProfileGrowth.Profile.Settings.IsAnnihilationGemReset;
            set
            {
                ProfileGrowth.Profile.Settings.IsAnnihilationGemReset = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsDDGemReset
        {
            get => ProfileGrowth.Profile.Settings.IsDefenseGemReset;
            set
            {
                ProfileGrowth.Profile.Settings.IsDefenseGemReset = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsLabGemReset
        {
            get => ProfileGrowth.Profile.Settings.IsLabGemReset;
            set
            {
                ProfileGrowth.Profile.Settings.IsLabGemReset = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsDailyEntryPackageBought
        {
            get => ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought;
            set
            {
                ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsDailyEntryPackageEssentialBought
        {
            get => ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought;
            set
            {
                ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought = value;
                OnPropertyChanged(string.Empty);
            }
        }        

        public bool IsSingleStageAHFarming
        {
            get => ProfileGrowth.Profile.Settings.IsSingleStageAHFarming;
            set
            {
                ProfileGrowth.Profile.Settings.IsSingleStageAHFarming = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool IsECBoostActive
        {
            get => ProfileGrowth.Profile.Settings.IsECBoostActive;
            set
            {
                ProfileGrowth.Profile.Settings.IsECBoostActive = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public int DailyECFromExpeditions
        {
            get => ProfileGrowth.Profile.Settings.DailyECFromExpeditions;
            set
            {
                ProfileGrowth.Profile.Settings.DailyECFromExpeditions = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public int WeeklyAdditionalEC
        {
            get => ProfileGrowth.Profile.Settings.WeeklyAdditionalEC;
            set
            {
                ProfileGrowth.Profile.Settings.WeeklyAdditionalEC = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public int AdditionalGCCubes
        {
            get => ProfileGrowth.Profile.Settings.WeeklyAdditionalGCCubes;
            set
            {
                ProfileGrowth.Profile.Settings.WeeklyAdditionalGCCubes = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public int CrystalsAdditionalWeekly
        {
            get => ProfileGrowth.Profile.Settings.WeeklyAdditionalDivineCrystals;
            set
            {
                ProfileGrowth.Profile.Settings.WeeklyAdditionalDivineCrystals = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public int MaxGCFromBGShop
        {
            get => ProfileGrowth.Profile.Settings.MaxGCFromBG;
            set
            {
                ProfileGrowth.Profile.Settings.MaxGCFromBG = value;
                OnPropertyChanged(string.Empty);
            }
        }

        public bool BuyingCrystalsFor70BG
        {
            get => ProfileGrowth.Profile.Settings.Buying70BGCrystals;
            set
            {
                ProfileGrowth.Profile.Settings.Buying70BGCrystals = value;
                OnPropertyChanged(string.Empty);
            }
        }

        // Binding purposes (displaying value as the switch + switching value saves new value in the profile file).
        public bool IsWeeklyBoVPackBought
        {
            get => ProfileGrowth.Profile.Settings.IsWeeklyBoVPackBought;
            set
            {
                ProfileGrowth.Profile.Settings.IsWeeklyBoVPackBought = value;
                OnPropertyChanged(string.Empty);
            }
        }
        public int BoVFromMercShop
        {
            get => ProfileGrowth.Profile.Settings.BoVFromMercShop;
            set
            {
                ProfileGrowth.Profile.Settings.BoVFromMercShop = value;
                OnPropertyChanged(string.Empty);
            }
        }
        public Dictionary<VulcanusRankEnum, int> VulcanusRanks { get; } = ((VulcanusRankEnum[])Enum.GetValues(typeof(VulcanusRankEnum))).ToDictionary(e => e, e => (int)e);
        public int VulcanusDivisionEnergy => (int)VulcanusRank;
        public int VulcanusTotalEnergy => Energy.EnergyVulcanus;
        public int ExtraEnergyFromPack => TotalEnergyBought;
        public int EnergyTotalWeekly => Energy.EnergyTotalWeekly;
        public int TotalEnergyFromAllPacks => Energy.TotalEnergyFromPack;
        public int ECFromEnergy => EvolutionCube.ECFromEnergy;
        public int ECFromLab => EvolutionCube.ECFromLab;
        public int ECFromExpedition => EvolutionCube.ECFromExpeditions;
        public int ECFromOther => EvolutionCube.ECFromOtherSources;
        public int ECTotalWeekly => EvolutionCube.TotalECWeekly;
        public int AHTickets => (int)Math.Floor((double)ECTotalWeekly / 120f);
        public int AHRuns => (int)Math.Floor((double)AHTickets / 3f);
        public int AHOres => AHRuns * (IsSingleStageAHFarming ? 9 : 12);
        public double PurpleCrystals => Math.Round(AHOres * 0.0026, 2);
        public double BlueCrystals => Math.Round(AHOres * 0.4029, 2);
        public double GreenCrystals => Math.Round(AHOres * 0.5947, 2);
        public int BoVFromEnergy => BoV.BoVFromEnergy;
        public int BoVFromLab => BoV.BoVFromLab;
        // Just for binding purposes (displaying the value in the textbox)
        public int BoVFromItemShop => BoV.BoVFromItemShop;
        public int BoVTotalWeekly => BoV.TotalBoVWeekly;
        public int GrowthEssenceFromEnergy => GrowthEssences.GrowthEssenceFromEnergy;
        public int GrowthEssenceFromDefenceMode => GrowthEssences.GrowthEssenceFromDefenceMode; 
        public int GrowthEssencesTotalWeekly => GrowthEssences.GrowthEssenceTotalWeekly;
        public double SoulEssencesSIWeeksNeeded => GrowthEssences.WeeksForFullSi;
        public double SoulEssencesCSWeeksNeeded => GrowthEssences.WeeksForFullChaser;
        public int BGFromDefenseMode => BlueGems.BlueGemsFromDefenceMode;
        public int BGFromEnergy => BlueGems.BlueGemsFromEnergy;
        public int BlueGemsWeeklyTotal => OverrideBlueGems ? CustomBlueGems : BlueGems.BlueGemsWeeklyTotal;
        public double BGFromSellingHeroes => BlueGems.SellRarityTypes.FirstOrDefault(t => t.Type == SellRarityType)?.Sell ?? 0;
        public int GrowthCubesFromBg => GrowthCubes.CubesFromBlueGems;
        public int GrowthCubesFromBgCost => GrowthCubes.CubesFromBlueGemsCost;
        public int GrowthCubesFromAnni => GrowthCubes.CubesFromAnni;
        public int GrowthCubesTotalWeekly => GrowthCubes.CubesTotalWeekly;
        public double GrowthCubesCSSrWeeksNeeded => GrowthCubes.WeeksForFullChaser;
        public double GrowthCubesCSNoEventWeeksNeeded => GrowthCubes.WeeksForFullChaserSr;
        public double GrowthCubesSIWeeksNeeded => GrowthCubes.WeeksForFullSi;
        public double GrowthCubesSISrWeeksNeeded => GrowthCubes.WeeksForFullSiSr;
        public double GrowthCubesSINoEventWeeksNeeded => GrowthCubes.WeeksForFullSiNoEvent;
        public int CrystalsFromBG => DivineCrystals.CrystalsFromBlueGems;
        public int CrystalsFromAnni => DivineCrystals.CrystalsFromAnni;
        public int CrystalsFromConv => DivineCrystals.CrystalsFromConvertion;
        public int CrystalsTotalWeekly => DivineCrystals.CrystalsTotalWeekly;
        public int CrystalsForFullDescent => DivineCrystals.CrystalsForFullDescent;
        public double CrystalsWeeksForFullDescent => DivineCrystals.WeeksForFullDescent;
        public int CrystalsCheapWeekly => DivineCrystals.CheapCrystalsWeekly;
        public int CrystalsCheapCost => DivineCrystals.CheapCrystalsCost;
        public int CrystalsExpensiveWeekly => DivineCrystals.ExpensiveCrystalsWeekly;
        public int CrystalsExpensiveCost => DivineCrystals.ExpensiveCrystalsCost;

        public EnergyCalculatorView()
        {
            InitializeComponent();
            DataContext = this;

            ProfileManager.ProfileChanged += (sender, ae) =>
            {
                OnPropertyChanged(string.Empty);
            };
        }

        #region PC

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> raiser)
        {
            var propName = ((MemberExpression)raiser?.Body)?.Member.Name;
            OnPropertyChanged(propName);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;

        }

        #endregion
    }
}
