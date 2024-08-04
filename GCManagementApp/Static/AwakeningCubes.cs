using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class AwakeningCubes
    {

        public static int CubesPerRun => 8;
        public static double EnergyPackCubesMultiplier => ProfileGrowth.Profile.Settings.IsExtraEnergyBought ? 1.3 : 1;
        public static int CubesFromDefenseMode => 78 * (3 +
            (ProfileGrowth.Profile.Settings.IsDefenseGemReset ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;
        public static int CubesFromEnergy => (int)(((double)Energy.EnergyTotalWeekly / 12) * CubesPerRun * EnergyPackCubesMultiplier);
        public static int TotalCubesWeekly => CubesFromEnergy + CubesFromDefenseMode;
    }
}
