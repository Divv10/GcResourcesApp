using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public class ChaserCubes
    {
        public static int CubesFromAnni => 260 * (3 +
            (ProfileGrowth.Profile.Settings.IsAnnihilationGemReset ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7 / 10;

        public static int TotalCubesWeekly => CubesFromAnni; 
    }
}
