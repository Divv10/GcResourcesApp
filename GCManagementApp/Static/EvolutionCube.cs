using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class EvolutionCube
    {

        public static double ECPerRun => 3.27;
        public static int ECFromLab => 400 * (1 +
            (ProfileGrowth.Profile.Settings.IsLabGemReset ? 1 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 1 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 8; // Eight cause for fridays its double EC
        public static int ECFromEnergy => (int)(((double)Energy.EnergyTotalWeekly / ( ProfileGrowth.Profile.Settings.IsECBoostActive ? 6 : 12 )) * ECPerRun);
        public static int ECFromExpeditions => ProfileGrowth.Profile.Settings.DailyECFromExpeditions * 7;
        public static int ECFromOtherSources => ProfileGrowth.Profile.Settings.WeeklyAdditionalEC;

        public static int TotalECWeekly => ECFromEnergy + ECFromLab + ECFromExpeditions + ECFromOtherSources;
    }
}
