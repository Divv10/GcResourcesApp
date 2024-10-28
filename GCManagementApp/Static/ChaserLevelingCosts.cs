using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class ChaserLevelingCosts
    {
        public static List<ChaserLevelCost> CostsTable { get; } = new List<ChaserLevelCost>()
        {
            new ChaserLevelCost(0, 100, 30, 1000000),
            new ChaserLevelCost(1, 0, 8, 100000),
            new ChaserLevelCost(2, 0, 8, 100000),
            new ChaserLevelCost(3, 0, 8, 100000),
            new ChaserLevelCost(4, 0, 8, 100000),
            new ChaserLevelCost(5, 100, 8, 1200000),
            new ChaserLevelCost(6, 0, 15, 200000),
            new ChaserLevelCost(7, 0, 15, 200000),
            new ChaserLevelCost(8, 0, 15, 200000),
            new ChaserLevelCost(9, 0, 15, 200000),
            new ChaserLevelCost(10, 100, 15, 1400000),
            new ChaserLevelCost(11, 0, 28, 300000),
            new ChaserLevelCost(12, 0, 28, 300000),
            new ChaserLevelCost(13, 0, 28, 300000),
            new ChaserLevelCost(14, 0, 28, 300000),
            new ChaserLevelCost(15, 100, 28, 1600000),
            new ChaserLevelCost(16, 0, 47, 400000),
            new ChaserLevelCost(17, 0, 47, 400000),
            new ChaserLevelCost(18, 0, 47, 400000),
            new ChaserLevelCost(19, 0, 47, 400000),
            new ChaserLevelCost(20, 100, 47, 1800000),
            new ChaserLevelCost(21, 0, 72, 500000),
            new ChaserLevelCost(22, 0, 72, 500000),
            new ChaserLevelCost(23, 0, 72, 500000),
            new ChaserLevelCost(24, 0, 72, 500000),
            new ChaserLevelCost(25, 100, 72, 2000000),
        };

        public static ChaserLevelCost CalculateCost(int from, int to)
        {
            if (to == from)
            {
                return new ChaserLevelCost(0, 0, 0, 0);
            }

            var costs = from == 0 ? CostsTable.Where(c => c.Level >= from && c.Level <= to) : CostsTable.Where(c => c.Level > from && c.Level <= to);
            return new ChaserLevelCost(to - from, costs.Sum(c => c.GrowthCubesCost), costs.Sum(c => c.GrowthEssencesCost), costs.Sum(c => c.GoldCost));
        }
    }

    public class ChaserLevelCost
    {
        public int Level { get; set; }
        public int GrowthCubesCost { get; set; }
        public int GrowthEssencesCost { get; set; }
        public int GoldCost { get; set; }

        public ChaserLevelCost(int level, int chaserCubesCost, int growthEssencesCost, int goldCost)
        {
            Level = level;
            GrowthCubesCost = chaserCubesCost;
            GrowthEssencesCost = growthEssencesCost;
            GoldCost = goldCost;
        }
    }
}
