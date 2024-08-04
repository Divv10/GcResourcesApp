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
            new ChaserLevelCost(0, 200, 150, 5000000),
            new ChaserLevelCost(1, 0, 40, 300000),
            new ChaserLevelCost(2, 0, 40, 300000),
            new ChaserLevelCost(3, 0, 40, 300000),
            new ChaserLevelCost(4, 0, 40, 300000),
            new ChaserLevelCost(5, 200, 40, 1500000),
            new ChaserLevelCost(6, 0, 76, 600000),
            new ChaserLevelCost(7, 0, 76, 600000),
            new ChaserLevelCost(8, 0, 76, 600000),
            new ChaserLevelCost(9, 0, 76, 600000),
            new ChaserLevelCost(10, 200, 76, 3000000),
            new ChaserLevelCost(11, 0, 142, 1200000),
            new ChaserLevelCost(12, 0, 142, 1200000),
            new ChaserLevelCost(13, 0, 142, 1200000),
            new ChaserLevelCost(14, 0, 142, 1200000),
            new ChaserLevelCost(15, 200, 142, 6000000),
            new ChaserLevelCost(16, 0, 238, 1500000),
            new ChaserLevelCost(17, 0, 238, 1500000),
            new ChaserLevelCost(18, 0, 238, 1500000),
            new ChaserLevelCost(19, 0, 238, 1500000),
            new ChaserLevelCost(20, 200, 238, 7500000),
            new ChaserLevelCost(21, 0, 364, 3000000),
            new ChaserLevelCost(22, 0, 364, 3000000),
            new ChaserLevelCost(23, 0, 364, 3000000),
            new ChaserLevelCost(24, 0, 364, 3000000),
            new ChaserLevelCost(25, 200, 364, 15000000),
        };

        public static ChaserLevelCost CalculateCost(int from, int to)
        {
            if (to == from)
            {
                return new ChaserLevelCost(0, 0, 0, 0);
            }

            var costs = from == 0 ? CostsTable.Where(c => c.Level >= from && c.Level <= to) : CostsTable.Where(c => c.Level > from && c.Level <= to);
            return new ChaserLevelCost(to - from, costs.Sum(c => c.ChaserCubesCost), costs.Sum(c => c.ChaserCrystalsCost), costs.Sum(c => c.GoldCost));
        }
    }

    public class ChaserLevelCost
    {
        public int Level { get; set; }
        public int ChaserCubesCost { get; set; }
        public int ChaserCrystalsCost { get; set; }
        public int GoldCost { get; set; }

        public ChaserLevelCost(int level, int chaserCubesCost, int chaserCrystalsCost, int goldCost)
        {
            Level = level;
            ChaserCubesCost = chaserCubesCost;
            ChaserCrystalsCost = chaserCrystalsCost;
            GoldCost = goldCost;
        }
    }
}
