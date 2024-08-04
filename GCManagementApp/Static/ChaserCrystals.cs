using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class ChaserCrystals
    {
        public static int ChaserCrystalsPerRun => 5;
        public static int ChaserCrystalsFromEnergy => (int)(((double)Energy.EnergyTotalWeekly / 12) * ChaserCrystalsPerRun);
        public static int ChaserCrystalsFromDefenceMode => 135 * (3 + 
            (ProfileGrowth.Profile.Settings.IsDefenseGemReset ? 3 : 0) + 
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;
        public static int ChaserCrystalsTotalWeekly => ChaserCrystalsFromDefenceMode + ChaserCrystalsFromEnergy;
    }
}
