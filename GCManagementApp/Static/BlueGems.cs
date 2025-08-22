using GCManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class BlueGems
    {
        public static int BGFromEnergy => 32;
        public static double EnergyPackCubesMultiplier => ProfileGrowth.Profile.Settings.IsExtraEnergyBought ? 1.3 : 1;
        public static int BlueGemsFromPvP => 4000;
        public static List<SellRarityType> SellRarityTypes { get; } = new List<SellRarityType>()
        {
            new SellRarityType() { Type = Enums.SellRarityTypeEnum.S, Sell = 220 },
            new SellRarityType() { Type = Enums.SellRarityTypeEnum.A, Sell = 44 },
        };
        public static int BlueGemsFromDailyMission => 14000;
        public static int BlueGemsFromDefenceMode => 736 * (3 +
            (ProfileGrowth.Profile.Settings.IsDefenseGemReset ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageBought ? 3 : 0) +
            (ProfileGrowth.Profile.Settings.IsDailyEntryPackageEssentialBought ? 1 : 0)) * 7;
        public static int BlueGemsFromEnergy => (int)(((double)Energy.EnergyTotalWeekly / 12) * BGFromEnergy * EnergyPackCubesMultiplier);
        public static int BlueGemsFromArena = 4000;
        
        public static int BlueGemsFromWeeklyMission => 3000;

        public static int BlueGemSellRarity
        {
            get
            {
                var sell = SellRarityTypes.FirstOrDefault(t => t.Type == ProfileGrowth.Profile.Settings.RaritySellUpgradeType);
                return (int)(sell.Sell * 5);
            }
        }

        public static int BlueGemsWeeklyTotal => BlueGemsFromPvP + BlueGemsFromWeeklyMission + BlueGemsFromDailyMission + BlueGemsFromDefenceMode + BlueGemsFromArena + BlueGemSellRarity + BlueGemsFromEnergy;

        public static int TotalBlueGems => ProfileGrowth.Profile.Settings.OverrideBlueGems ? ProfileGrowth.Profile.Settings.CustomBlueGems : BlueGemsWeeklyTotal;
    }
}
