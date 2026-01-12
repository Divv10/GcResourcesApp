using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class SiLevelingCosts
    {
        public static List<SiLevelCost> CostsTable { get; } = new List<SiLevelCost>()
        {
            new SiLevelCost(0, 0, 20, 1500000),
            new SiLevelCost(1, 0, 20, 800000),
            new SiLevelCost(2, 0, 20, 800000),
            new SiLevelCost(3, 0, 20, 800000),
            new SiLevelCost(4, 0, 20, 800000),
            new SiLevelCost(5, 0, 9*15+20, 1700000),
            new SiLevelCost(6, 500, 100, 3000000),
            new SiLevelCost(7, 250, 0, 1000000),
            new SiLevelCost(8, 250, 0, 1000000),
            new SiLevelCost(9, 250, 0, 1000000),
            new SiLevelCost(10, 250, 9*60, 4600000),
            new SiLevelCost(11, 500, 225, 4500000),
            new SiLevelCost(12, 250, 0, 1500000),
            new SiLevelCost(13, 250, 0, 1500000),
            new SiLevelCost(14, 250, 0, 1500000),
            new SiLevelCost(15, 250, 9*150, 4200000),
        };

        public static List<SiLevelCost> SiStepsTable { get; } = new List<SiLevelCost>()
        {
            new SiLevelCost(0.5, 0, 20, 1500000),
            new SiLevelCost(1, 0, 20, 800000),
            new SiLevelCost(2, 0, 20, 800000),
            new SiLevelCost(3, 0, 20, 800000),
            new SiLevelCost(4, 0, 20, 800000),
            new SiLevelCost(5, 0, 20, 800000),
            new SiLevelCost(5.5, 250, 100, 2000000),
            new SiLevelCost(6, 250, 0, 1000000),
            new SiLevelCost(7, 250, 0, 1000000),
            new SiLevelCost(8, 250, 0, 1000000),
            new SiLevelCost(9, 250, 0, 1000000),
            new SiLevelCost(10, 250, 0, 1000000),
            new SiLevelCost(10.5, 250, 225, 3000000),
            new SiLevelCost(11, 250, 0, 1500000),
            new SiLevelCost(12, 250, 0, 1500000),
            new SiLevelCost(13, 250, 0, 1500000),
            new SiLevelCost(14, 250, 0, 1500000),
            new SiLevelCost(15, 250, 0, 1500000),
        };

        public static List<SiLevelCost> SiTraitsTable { get; } = new List<SiLevelCost>()
        {
            new SiLevelCost(0, 0, 0, 0),
            new SiLevelCost(1, 0, 15, 100000),
            new SiLevelCost(2, 0, 15, 100000),
            new SiLevelCost(3, 0, 15, 100000),
            new SiLevelCost(4, 0, 15, 100000),
            new SiLevelCost(5, 0, 15, 100000),
            new SiLevelCost(6, 0, 15, 100000),
            new SiLevelCost(7, 0, 15, 100000),
            new SiLevelCost(8, 0, 15, 100000),
            new SiLevelCost(9, 0, 15, 100000),
            new SiLevelCost(10, 0, 60, 400000),
            new SiLevelCost(11, 0, 60, 400000),
            new SiLevelCost(12, 0, 60, 400000),
            new SiLevelCost(13, 0, 60, 400000),
            new SiLevelCost(14, 0, 60, 400000),
            new SiLevelCost(15, 0, 60, 400000),
            new SiLevelCost(16, 0, 60, 400000),
            new SiLevelCost(17, 0, 60, 400000),
            new SiLevelCost(18, 0, 60, 400000),
            new SiLevelCost(19, 0, 150, 800000),
            new SiLevelCost(20, 0, 150, 800000),
            new SiLevelCost(21, 0, 150, 800000),
            new SiLevelCost(22, 0, 150, 800000),
            new SiLevelCost(23, 0, 150, 800000),
            new SiLevelCost(24, 0, 150, 800000),
            new SiLevelCost(25, 0, 150, 800000),
            new SiLevelCost(26, 0, 150, 800000),
            new SiLevelCost(27, 0, 150, 800000),
        };

        public static SiLevelCost CalculateCost(int from, int to)
        {
            if (to == from)
            {
                return new SiLevelCost(0, 0, 0, 0);
            }

            var costs = from == 0 ? CostsTable.Where(c => c.Level >= from && c.Level <= to) : CostsTable.Where(c => c.Level > from && c.Level <= to);
            return new SiLevelCost(to - from, costs.Sum(c => c.GrowthCubesCost), costs.Sum(c => c.GeCost), costs.Sum(c => c.GoldCost));
        }

        public static SiLevelCost CalculateCostWithTraits(int from, int to, int currentTraits, int desiredTraits, bool isCoreOpen, bool isDesiredCoreOpen)
        {
            var currentCost = new SiLevelCost(0, 0, 0, 0);
            if (to == from && currentTraits == desiredTraits && isCoreOpen == isDesiredCoreOpen)
            {
                return currentCost;
            }

            var currentStep = from + (isCoreOpen ? 0.5 : 0);
            var desiredStep = to + (isDesiredCoreOpen ? 0.5 : 0);

            var coresCosts = SiStepsTable.Where(c => c.Level > currentStep && c.Level <= desiredStep);
            var traitsCosts = SiTraitsTable.Where(c => c.Level > currentTraits && c.Level <= desiredTraits);

            return new SiLevelCost(0, coresCosts.Sum(c => c.GrowthCubesCost), coresCosts.Sum(c => c.GeCost) + traitsCosts.Sum(c => c.GeCost), coresCosts.Sum(c => c.GoldCost) + traitsCosts.Sum(c => c.GoldCost)) ;
        }
    }

    public class SiLevelCost
    {
        public double Level { get; set; }
        public int GrowthCubesCost { get; set; }
        public int GeCost { get; set; }
        public int GoldCost { get; set; }
        public int DupesForSi { get; set; }

        public SiLevelCost(double level, int growthCubesCost, int geCost, int goldCost)
        {
            Level = level;
            GrowthCubesCost = growthCubesCost;
            GeCost = geCost;
            GoldCost = goldCost;
        }
    }
}
