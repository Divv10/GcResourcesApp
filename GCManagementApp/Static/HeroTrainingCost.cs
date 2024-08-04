using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class HeroTrainingCost
    {
        public static int CalculateBoVCost(int currentLevel, int desiredLevel)
        {
            if ((currentLevel < 300 && desiredLevel < 300) || (currentLevel > 300 && desiredLevel > 300))
            {
                return (BoVCostAtLevel(currentLevel + 1) + BoVCostAtLevel(desiredLevel)) / 2 * (desiredLevel - currentLevel);
            }
            int fragmetsCost = (BoVCostAtLevel(currentLevel + 1) + BoVCostAtLevel(300)) / 2 * (300 - currentLevel );
            int blessingsCost = (BoVCostAtLevel(300 + 1) + BoVCostAtLevel(desiredLevel)) / 2 * (desiredLevel - 300 );

            return fragmetsCost + blessingsCost;
        }

        public static int BoVCostAtLevel(int level)
        {
            return level <= 300 ? (int)Math.Ceiling((double)level * 2d / 5d) : level * 2;
        }
    }
}
