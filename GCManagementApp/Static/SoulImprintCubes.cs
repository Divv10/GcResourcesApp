using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class SoulImprintCubes
    {
        public static int CubesFromAnni => 260 * (3 +
            (ProfileGrowth.Profile.Settings.IsAnnihilationGemReset ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7 / 20;
        public static int CubesFromBlueGems => Math.Min(600, ProfileGrowth.Profile.Settings.OverrideBlueGems ? (ProfileGrowth.Profile.Settings.CustomBlueGems / 120) : (BlueGems.BlueGemsWeeklyTotal / 120));
        public static int AdditionalCubesWeekly => ProfileGrowth.Profile.Settings.WeeklyAdditionalSICubes;
        public static int CubesTotalWeekly => CubesFromAnni + CubesFromBlueGems + AdditionalCubesWeekly;
        public static int CubesForFullSi => 3150;
        public static int CubesForFullSiSr => 3000;
        public static int CubesForFullSiNoEvent => 3250;
        public static double WeeksForFullSi => (double)CubesForFullSi / (double)CubesTotalWeekly;
        public static double WeeksForFullSiSr => (double)CubesForFullSiSr / (double)CubesTotalWeekly;
        public static double WeeksForFullSiNoEvent => (double)CubesForFullSiNoEvent / (double)CubesTotalWeekly;
    }
}
