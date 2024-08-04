using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class BoV
    {

        public static int BoVPerRun => 4;
        public static int BoVFromLab => 670 * (1 +
            (ProfileGrowth.Profile.Settings.IsLabGemReset ? 1 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 1 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;
        public static int BoVFromEnergy => (int)(((double)Energy.EnergyTotalWeekly / 12) * BoVPerRun);
        // Added new source calculation
        public static int BoVFromItemShop => ProfileGrowth.Profile.Settings.IsWeeklyBoVPackBought ? 1000 : 0; //1000 BoV if we check the switch, otherwise its 0
                                                                                                              // Accounting it into total weekly calculations
        public static int BoVFromMercShop => ProfileGrowth.Profile.Settings.BoVFromMercShop;
        public static int TotalBoVWeekly => BoVFromEnergy + BoVFromLab + BoVFromItemShop + BoVFromMercShop;
    }
}
