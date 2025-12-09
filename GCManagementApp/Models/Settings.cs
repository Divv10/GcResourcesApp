using GCManagementApp.Enums;
using GCManagementApp.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GCManagementApp.Models
{
    [Serializable]
    public class Settings : NotifyPropertyChanged
    {
        private int _energyAdsShop;
        public int EnergyAdsShop
        {
            get => _energyAdsShop;
            set => SetProperty(ref _energyAdsShop, value);
        }

        private VulcanusRankReward _vulcRankTier;
        public VulcanusRankReward VulcRankTier
        {
            get => _vulcRankTier;
            set => SetProperty(ref _vulcRankTier, value);
        }
        

        private bool _isExtraEnergyBought;
        public bool IsExtraEnergyBought
        {
            get => _isExtraEnergyBought;
            set => SetProperty(ref _isExtraEnergyBought, value);
        }

        private int _totalEnergyBought;
        public int TotalEnergyBought
        {
            get => _totalEnergyBought;
            set => SetProperty(ref _totalEnergyBought, value);
        }

        private bool _energySupportPackageBought;
        public bool EnergySupportPackageBought
        {
            get => _energySupportPackageBought;
            set => SetProperty(ref _energySupportPackageBought, value);
        }

        private bool _energySupportPackage2Bought;
        public bool EnergySupportPackage2Bought
        {
            get => _energySupportPackage2Bought;
            set => SetProperty(ref _energySupportPackage2Bought, value);
        }

        private bool _dailySpecUpPackageBought;
        public bool DailySpecUpPackageBought
        {
            get => _dailySpecUpPackageBought;
            set => SetProperty(ref _dailySpecUpPackageBought, value);
        }

        private bool _specialEnergyPremiumPackageBought;
        public bool SpecialEnergyPremiumPackageBought
        {
            get => _specialEnergyPremiumPackageBought;
            set => SetProperty(ref _specialEnergyPremiumPackageBought, value);
        }

        private bool _specialEnergyBasicPackageBought;
        public bool SpecialEnergyBasicPackageBought
        {
            get => _specialEnergyBasicPackageBought;
            set => SetProperty(ref _specialEnergyBasicPackageBought, value);
        }

        private bool _energySupplyPackageBought;
        public bool EnergySupplyPackageBought
        {
            get => _energySupplyPackageBought;
            set => SetProperty(ref _energySupplyPackageBought, value);
        }

        private bool _energyFromGemsBought;
        public bool EnergyFromGemsBought
        {
            get => _energyFromGemsBought;
            set => SetProperty(ref _energyFromGemsBought, value);
        }

        private bool _overrideBlueGems;
        public bool OverrideBlueGems
        {
            get => _overrideBlueGems;
            set => SetProperty(ref _overrideBlueGems, value);
        }

        private int _customBlueGems;
        public int CustomBlueGems
        {
            get => _customBlueGems;
            set => SetProperty(ref _customBlueGems, value);
        }

        private bool _isAnnihilationGemReset;
        public bool IsAnnihilationGemReset
        {
            get => _isAnnihilationGemReset;
            set => SetProperty(ref _isAnnihilationGemReset, value);
        }

        private bool _isDefenseGemReset;
        public bool IsDefenseGemReset
        {
            get => _isDefenseGemReset;
            set => SetProperty(ref _isDefenseGemReset, value);
        }

        private bool _isLabGemReset;
        public bool IsLabGemReset
        {
            get => _isLabGemReset;
            set => SetProperty(ref _isLabGemReset, value);
        }

        private bool _isDailyEntryPackageBought;
        public bool IsDailyEntryPackageBought
        {
            get => _isDailyEntryPackageBought;
            set => SetProperty(ref _isDailyEntryPackageBought, value);
        }

        private bool _isDailyEntryPackageEssentialBought;
        public bool IsDailyEntryPackageEssentialBought
        {
            get => _isDailyEntryPackageEssentialBought;
            set => SetProperty(ref _isDailyEntryPackageEssentialBought, value);
        }

        private SellRarityTypeEnum _sellRarityType;
        public SellRarityTypeEnum RaritySellUpgradeType
        {
            get => _sellRarityType;
            set => SetProperty(ref _sellRarityType, value);
        }

        private int _dailyGoldIncome;
        public int DailyGoldIncome
        {
            get => _dailyGoldIncome;
            set => SetProperty(ref _dailyGoldIncome, value);
        }

        private int _weeklyEwMatsIncome;
        public int WeeklyEwMatsIncome
        {
            get => _weeklyEwMatsIncome;
            set => SetProperty(ref _weeklyEwMatsIncome, value);
        }

        private int _heroTrainingHealerLevel;
        public int HeroTrainingHealerLevel
        {
            get => _heroTrainingHealerLevel;
            set => SetProperty(ref _heroTrainingHealerLevel, value);
        }

        private int _heroTrainingHealerDesiredLevel;
        public int HeroTrainingHealerDesiredLevel
        {
            get => _heroTrainingHealerDesiredLevel;
            set => SetProperty(ref _heroTrainingHealerDesiredLevel, value);
        }

        private int _heroTrainingAssaultLevel;
        public int HeroTrainingAssaultLevel
        {
            get => _heroTrainingAssaultLevel;
            set => SetProperty(ref _heroTrainingAssaultLevel, value);
        }

        private int _heroTrainingAssaultDesiredLevel;
        public int HeroTrainingAssaultDesiredLevel
        {
            get => _heroTrainingAssaultDesiredLevel;
            set => SetProperty(ref _heroTrainingAssaultDesiredLevel, value);
        }

        private int _heroTrainingMageLevel;
        public int HeroTrainingMageLevel
        {
            get => _heroTrainingMageLevel;
            set => SetProperty(ref _heroTrainingMageLevel, value);
        }

        private int _heroTrainingMageDesiredLevel;
        public int HeroTrainingMageDesiredLevel
        {
            get => _heroTrainingMageDesiredLevel;
            set => SetProperty(ref _heroTrainingMageDesiredLevel, value);
        }

        private int _heroTrainingRangerLevel;
        public int HeroTrainingRangerLevel
        {
            get => _heroTrainingRangerLevel;
            set => SetProperty(ref _heroTrainingRangerLevel, value);
        }

        private int _heroTrainingRangerDesiredLevel;
        public int HeroTrainingRangerDesiredLevel
        {
            get => _heroTrainingRangerDesiredLevel;
            set => SetProperty(ref _heroTrainingRangerDesiredLevel, value);
        }

        private int _heroTrainingTankLevel;
        public int HeroTrainingTankLevel
        {
            get => _heroTrainingTankLevel;
            set => SetProperty(ref _heroTrainingTankLevel, value);
        }

        private int _heroTrainingTankDesiredLevel;
        public int HeroTrainingTankDesiredLevel
        {
            get => _heroTrainingTankDesiredLevel;
            set => SetProperty(ref _heroTrainingTankDesiredLevel, value);
        }

        private int _weeklyBoVIncomeFromOtherSources;
        public int WeeklyBoVIncomeFromOtherSources
        {
            get => _weeklyBoVIncomeFromOtherSources;
            set => SetProperty(ref _weeklyBoVIncomeFromOtherSources, value);
        }

        private bool _isSingleStageAHFarming;
        public bool IsSingleStageAHFarming
        {
            get => _isSingleStageAHFarming;
            set => SetProperty(ref _isSingleStageAHFarming, value);
        }

        private int _dailyECFromExpeditions;
        public int DailyECFromExpeditions
        {
            get => _dailyECFromExpeditions;
            set => SetProperty(ref _dailyECFromExpeditions, value);
        }

        private int _weeklyAdditionalEC;
        public int WeeklyAdditionalEC
        {
            get => _weeklyAdditionalEC;
            set => SetProperty(ref _weeklyAdditionalEC, value);
        }

        private bool _isBGBoostActive;
        public bool IsBGBoostActive
        {
            get => _isBGBoostActive;
            set => SetProperty(ref _isBGBoostActive, value);
        }

        private bool _isECBoostActive;
        public bool IsECBoostActive
        {
            get => _isECBoostActive;
            set => SetProperty(ref _isECBoostActive, value);
        }

        private int _weeklyAdditionalGCCubes;
        public int WeeklyAdditionalGCCubes
        {
            get => _weeklyAdditionalGCCubes;
            set => SetProperty(ref _weeklyAdditionalGCCubes, value);
        }

        private int _weeklyAdditionalDivineCrystals;
        public int WeeklyAdditionalDivineCrystals
        {
            get => _weeklyAdditionalDivineCrystals;
            set => SetProperty(ref _weeklyAdditionalDivineCrystals, value);
        }

        private int _maxGCFromBG;
        public int MaxGCFromBG
        {
            get => _maxGCFromBG;
            set => SetProperty(ref _maxGCFromBG, value);
        }

        private bool _buying70BGCrystals;
        public bool Buying70BGCrystals
        {
            get => _buying70BGCrystals;
            set => SetProperty(ref _buying70BGCrystals, value);
        }

        //Adding entry here makes it that this value will be saved into profile file, and remembered between app restars
        private bool _isWeeklyBoVPackBought;
        public bool IsWeeklyBoVPackBought
        {
            get => _isWeeklyBoVPackBought;
            set => SetProperty(ref _isWeeklyBoVPackBought, value);
        }

        private int _BoVFromMercShop;
        public int BoVFromMercShop
        {
            get => _BoVFromMercShop;
            set => SetProperty(ref _BoVFromMercShop, value);
        }
    }
}
