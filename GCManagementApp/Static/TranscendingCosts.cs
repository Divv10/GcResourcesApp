using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class TranscendingCosts
    {
        public static List<TranscendenceCost> CostsTable { get; } = new List<TranscendenceCost>()
        {
            new TranscendenceCost(0, 0, 0),
            new TranscendenceCost(1, 1, 5500000),
            new TranscendenceCost(2, 1, 6000000),
            new TranscendenceCost(3, 1, 7000000),
            new TranscendenceCost(4, 2, 13500000),
            new TranscendenceCost(5, 3, 20500000),
            new TranscendenceCost(6, 4, 28000000),

        };

        public static TranscendenceCost CalculateCost(int from, int to)
        {
            if (to == from)
            {
                return new TranscendenceCost(0, 0, 0);
            }

            var costs = from == 0 ? CostsTable.Where(c => c.Level >= from && c.Level <= to) : CostsTable.Where(c => c.Level > from && c.Level <= to);
            return new TranscendenceCost(to - from, costs.Sum(c => c.DupesCost), costs.Sum(c => c.GoldCost));
        }
    }

    public class TranscendenceCost
    {
        public int Level { get; set; }
        public int DupesCost { get; set; }
        public int GoldCost { get; set; }

        public TranscendenceCost(int level, int dupesCost, int goldCost)
        {
            Level = level;
            DupesCost = dupesCost;
            GoldCost = goldCost;
        }
    }
}
