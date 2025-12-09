using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class Energy
    {
        public static int EnergyNaturalRegen => 4032;
        public static int EnergyMailAds => 2100;
        public static int EnergyShopAds => ProfileGrowth.Profile.Settings.EnergyAdsShop;
        public static int EnergyDaily => 770; 
        public static int EnergyWeekly => 450;
        public static int EnergySupportPackEnergy => ProfileGrowth.Profile.Settings.EnergySupportPackageBought ? 600 * 7 : 0;
        public static int EnergySupportPack2Energy => ProfileGrowth.Profile.Settings.EnergySupportPackage2Bought ? 1400 * 7 : 0;
        public static int DailySpecUpPackEnergy => ProfileGrowth.Profile.Settings.DailySpecUpPackageBought ? 500 * 7 : 0;
        public static int SpecialEnergyPackPremiumEnergy => ProfileGrowth.Profile.Settings.SpecialEnergyPremiumPackageBought ? 660 * 7 : 0;
        public static int SpecialEnergyPackBasicEnergy => ProfileGrowth.Profile.Settings.SpecialEnergyBasicPackageBought ? 220 * 7 : 0;
        public static int EnergySupplyPackageEnergy => ProfileGrowth.Profile.Settings.EnergySupplyPackageBought ? 5200 * 4 : 0;
        public static int GemmedEnergy => ProfileGrowth.Profile.Settings.EnergyFromGemsBought ? 150 * 10 * 7 : 0;
        public static int ExtraEnergyFromPack => ProfileGrowth.Profile.Settings.TotalEnergyBought;
        public static int TotalEnergyFromPack => EnergySupportPackEnergy + EnergySupportPack2Energy + DailySpecUpPackEnergy + SpecialEnergyPackPremiumEnergy + SpecialEnergyPackBasicEnergy + ExtraEnergyFromPack + EnergySupplyPackageEnergy + GemmedEnergy;
        public static int EnergyVulcanus => ProfileGrowth.Profile.Settings.VulcRankTier.Energy;
        public static int EnergyTotalWeekly => EnergyNaturalRegen + EnergyMailAds + EnergyShopAds + EnergyDaily + EnergyWeekly + EnergyVulcanus + TotalEnergyFromPack;
        public static int EnergyPerBossStage => 12;
    }
}
