using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class DivineCrystals
    {
        public static int CrystalsFromAnni => 78 * (3 +
            (ProfileGrowth.Profile.Settings.IsAnnihilationGemReset ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;
        public static int CheapCrystalCost => 35;
        public static int ExpensiveCrystalCost => 70;
        public static int CheapCrystalLimit => 1200;
        public static int ExpensiveCrystalLimit => 1500;
        public static int CrystalsFromBlueGems => CheapCrystalsWeekly + ExpensiveCrystalsWeekly;
        public static int CrystalsFromConvertion => Math.Min(1500, GrowthCubes.CubesTotalWeekly);
        public static int AdditionalCrystalsWeekly => ProfileGrowth.Profile.Settings.WeeklyAdditionalDivineCrystals;
        public static int CrystalsTotalWeekly => CrystalsFromAnni + CrystalsFromBlueGems + AdditionalCrystalsWeekly;
        public static int CheapCrystalsWeekly
        {
            get
            {
                var blueGemsAvailable = BlueGems.TotalBlueGems;
                var gcWorth = GrowthCubes.CubeCostInBG * GrowthCubes.CubesFromBlueGems;

                return Math.Min(CheapCrystalLimit, (blueGemsAvailable - gcWorth) / CheapCrystalCost);
            }
        }
        public static int CheapCrystalsCost => CheapCrystalsWeekly * CheapCrystalCost;
        public static int ExpensiveCrystalsWeekly
        {
            get
            {
                if (!ProfileGrowth.Profile.Settings.Buying70BGCrystals)
                    return 0;

                var blueGemsAvailable = BlueGems.TotalBlueGems;
                var gcWorth = GrowthCubes.CubeCostInBG * GrowthCubes.CubesFromBlueGems;
                blueGemsAvailable -= (gcWorth + CheapCrystalsCost);
                if (blueGemsAvailable <= 0)
                    return 0;

                return Math.Min(ExpensiveCrystalLimit, blueGemsAvailable / CheapCrystalCost);
            }
        }

        public static int ExpensiveCrystalsCost => ExpensiveCrystalsWeekly * ExpensiveCrystalCost;
        public static int CrystalsForFullDescent => 35000;
        public static double WeeksForFullDescent => (double)CrystalsForFullDescent / (double)CrystalsTotalWeekly;
    }
}
