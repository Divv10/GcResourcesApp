using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class GrowthCubes
    {
        public static int CubesFromAnni => 17 * (3 +
            (ProfileGrowth.Profile.Settings.IsAnnihilationGemReset ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;
                
        public static int CubeCostInBG = 120;
        public static int CubesFromBlueGems => Math.Min(ProfileGrowth.Profile.Settings.MaxGCFromBG, BlueGems.TotalBlueGems / CubeCostInBG);
        public static int CubesFromBlueGemsCost => CubesFromBlueGems * CubeCostInBG;
        public static int AdditionalCubesWeekly => ProfileGrowth.Profile.Settings.WeeklyAdditionalGCCubes;
        public static int CubesTotalWeekly => CubesFromAnni + CubesFromBlueGems + AdditionalCubesWeekly;
        public static int CubesForFullSi => 3150;
        public static int CubesForFullSiSr => 3000;
        public static int CubesForFullSiNoEvent => 3250;
        public static int CubesForFullChaser => 600;
        public static int CubesForFullChaserSrNoEvent => 100;
        public static double WeeksForFullSi => (double)CubesForFullSi / (double)CubesTotalWeekly;
        public static double WeeksForFullSiSr => (double)CubesForFullSiSr / (double)CubesTotalWeekly;
        public static double WeeksForFullSiNoEvent => (double)CubesForFullSiNoEvent / (double)CubesTotalWeekly;
        public static double WeeksForFullChaser => (double)CubesForFullChaser / (double)CubesTotalWeekly;
        public static double WeeksForFullChaserSr => (double)CubesForFullChaserSrNoEvent / (double)CubesTotalWeekly;

    }
}
