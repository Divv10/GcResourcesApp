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
            new TranscendenceCost(0, 2000, 0, 6300000),
            new TranscendenceCost(1, 1000, 0, 2000000),
            new TranscendenceCost(2, 1000, 0, 2500000),
            new TranscendenceCost(3, 1000, 0, 3000000),
            new TranscendenceCost(4, 1000, 0, 3500000),
            new TranscendenceCost(5, 1000, 0, 4000000),
            new TranscendenceCost(6, 1000, 0, 4500000),
            new TranscendenceCost(7, 0, 1, 5000000),
            new TranscendenceCost(8, 0, 1, 6000000),
            new TranscendenceCost(9, 0, 1, 8000000),
            new TranscendenceCost(10, 0, 1, 11000000),
            new TranscendenceCost(11, 0, 1, 15000000),
            new TranscendenceCost(12, 0, 1, 20000000),
            new TranscendenceCost(13, 0, 2, 25000000),
            new TranscendenceCost(14, 0, 2, 30000000),
            new TranscendenceCost(15, 0, 2, 35000000),
        };

        public static TranscendenceCost CalculateCost(int from, int to)
        {
            if (to == from)
            {
                return new TranscendenceCost(0, 0, 0, 0);
            }

            var costs = from == 0 ? CostsTable.Where(c => c.Level >= from && c.Level <= to) : CostsTable.Where(c => c.Level > from && c.Level <= to);
            return new TranscendenceCost(to - from, costs.Sum(c => c.AwakeningCubesCost), costs.Sum(c => c.DupesCost), costs.Sum(c => c.GoldCost));
        }
    }

    public class TranscendenceCost
    {
        public int Level { get; set; }
        public int AwakeningCubesCost { get; set; }
        public int DupesCost { get; set; }
        public int GoldCost { get; set; }

        public TranscendenceCost(int level, int awakeningCubesCost, int dupesCost, int goldCost)
        {
            Level = level;
            AwakeningCubesCost = awakeningCubesCost;
            DupesCost = dupesCost;
            GoldCost = goldCost;
        }
    }
}
