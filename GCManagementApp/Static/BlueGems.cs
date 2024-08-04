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
        public static int BlueGemsFromPvP => 3000;
        public static List<AwakeningCubeUpgrade> AwakeningCubeUpgrades { get; } = new List<AwakeningCubeUpgrade>()
        {
            new AwakeningCubeUpgrade() { Type = Enums.AwakeningCubesUpgradeTypeEnum.S, Cost = 285, Sell = 1320 },
            new AwakeningCubeUpgrade() { Type = Enums.AwakeningCubesUpgradeTypeEnum.A, Cost = 61, Sell = 264 },
        };
        public static int BlueGemsFromAC
        {
            get
            {
                var selectedUpgradeType = AwakeningCubeUpgrades.FirstOrDefault(t => t.Type == ProfileGrowth.Profile.Settings.AwakeningCubesUpgradeType);
                return (int)(AwakeningCubes.TotalCubesWeekly * selectedUpgradeType.BlueGemPerCube);
            }
        }
        public static int BlueGemsWeeklyTotal => BlueGemsFromPvP + BlueGemsFromAC;
    }
}
