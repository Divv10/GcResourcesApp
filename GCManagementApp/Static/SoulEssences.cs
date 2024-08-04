using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class SoulEssences
    {
        public static double ChaserCrystalsNeeded => 1;
        public static int SoulEssencesTotalWeekly => (int)(((double)ChaserCrystals.ChaserCrystalsTotalWeekly / 5) / ChaserCrystalsNeeded);
        public static int SoulEssencesNeededForFullSI => SiLevelingCosts.CalculateCost(0, 15).SeCost;
        public static double WeeksForFullSi => (double)SoulEssencesNeededForFullSI / (double)SoulEssencesTotalWeekly;
    }
}
