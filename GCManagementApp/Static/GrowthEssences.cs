using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class GrowthEssences
    {
        public static int GrowthEssencePerRun => 2;
        public static int GrowthEssenceFromEnergy => (int)(((double)Energy.EnergyTotalWeekly / 12) * GrowthEssencePerRun);
        public static int GrowthEssenceFromDefenceMode => 28 * (3 + 
            (ProfileGrowth.Profile.Settings.IsDefenseGemReset ? 3 : 0) + 
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;
        public static int GrowthEssenceTotalWeekly => GrowthEssenceFromDefenceMode + GrowthEssenceFromEnergy;
        public static int GrowthEssencesNeededForFullSI => SiLevelingCosts.CalculateCost(0, 15).GeCost;
        public static int GrowthEssencesNeededForFullChaser => ChaserLevelingCosts.CalculateCost(0, 25).GrowthEssencesCost;
        public static double WeeksForFullSi => (double)GrowthEssencesNeededForFullSI / (double)GrowthEssenceTotalWeekly;
        public static double WeeksForFullChaser => (double)GrowthEssencesNeededForFullChaser / (double)GrowthEssenceTotalWeekly;
    }
}
