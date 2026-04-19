using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
	public class ExclusiveWeaponMaterials
	{
		public static int EwOrbFromDefenseMode => 91 * (3 +
            (ProfileGrowth.Profile.Settings.IsDefenseGemReset ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;
		public static int EwOrbClassFromDefenseMode => 14 * (3 +
            (ProfileGrowth.Profile.Settings.IsDefenseGemReset ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;

        public static int AdditionalOrbWeekly => ProfileGrowth.Profile.Settings.WeeklyEwMatsIncome;
        public static int AdditionalOrbClassWeekly => ProfileGrowth.Profile.Settings.WeeklyEWOrbIncome;

        public static int EwOrbWeeklyTotal => EwOrbFromDefenseMode;
        public static int EwOrbClassWeeklyTotal => EwOrbClassFromDefenseMode;

	}
}
