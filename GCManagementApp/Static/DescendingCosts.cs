using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class DescendingCosts
    {
        public static List<DescendCosts> CostsTable { get; } = new List<DescendCosts>()
        {
            new DescendCosts(0, 1500, 500, 3890000),
            new DescendCosts(1, 3500, 500, 2500000),//
            new DescendCosts(2, 3500, 500, 2930000),//
            new DescendCosts(3, 3500, 500, 3410000),
            new DescendCosts(4, 3500, 500, 3890000),//
            new DescendCosts(5, 3500, 500, 4350000),//
            new DescendCosts(6, 3500, 500, 4850000),//
            new DescendCosts(7, 3500, 500, 5350000),//
            new DescendCosts(8, 3500, 500, 5800000),
            new DescendCosts(9, 3500, 500, 6300000),
            new DescendCosts(10, 3500, 500, 6800000),

        };

        public static DescendCosts CalculateCost(int from, int to)
        {
            if (to == from)
            {
                return new DescendCosts(0, 0, 0, 0);
            }

            var costs = from == 0 ? CostsTable.Where(c => c.Level >= from && c.Level <= to) : CostsTable.Where(c => c.Level > from && c.Level <= to);
            return new DescendCosts(to - from, costs.Sum(c => c.DivineCrystalsCost), costs.Sum(c => c.GrowthEssenceCost), costs.Sum(c => c.GoldCost));
        }
    }

    public class DescendCosts
    {
        public int Level { get; set; }
        public int DivineCrystalsCost { get; set; }
        public int GrowthEssenceCost { get; set; }
        public int GoldCost { get; set; }
        public int DupesForDescent { get; set; }

        public DescendCosts(int level, int divineCrystalsCost, int growthEssenceCost, int goldCost)
        {
            Level = level;
            DivineCrystalsCost = divineCrystalsCost;
            GrowthEssenceCost = growthEssenceCost;
            GoldCost = goldCost;
        }
    }
}
