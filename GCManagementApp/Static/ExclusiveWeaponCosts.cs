using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class ExclusiveWeaponCosts
    {
        public static int WeaponEwMatsCost { get; } = 300;
        public static int WeaponGoldCost { get; } = 500000;

        public static List<ExclusiveWeaponCost> CostsTable { get; } = new List<ExclusiveWeaponCost>()
        {
            new ExclusiveWeaponCost(-1, 1, 0),
            new ExclusiveWeaponCost(0, 1, 0),
            new ExclusiveWeaponCost(1, 1, 0),
            new ExclusiveWeaponCost(2, 1, 0),
            new ExclusiveWeaponCost(3, 1, 0),
            new ExclusiveWeaponCost(4, 2, 0),
            new ExclusiveWeaponCost(5, 2, 0),
            new ExclusiveWeaponCost(6, 2, 0),
            new ExclusiveWeaponCost(7, 3, 0),
            new ExclusiveWeaponCost(8, 2, 148),
            new ExclusiveWeaponCost(9, 3, 187),
            new ExclusiveWeaponCost(10, 0, 0)
        };

        public static ExclusiveWeaponCost CalculateCost(int from, int to, int currentEProgress)
        {
            var cost = CostsTable.FirstOrDefault(x => x.Level == from);
            
            var levels = CostsTable.Where(x => x.Level >= from && x.Level < to);
            var y = levels.Sum(x => x.WeaponCost);
            var orbCost = 0;

            if (to >= 9 && from != 9)
            {
                var levelECost = CostsTable.Where(x => x.Level == 8);
                orbCost += levelECost.Sum(x => x.WeaponCost) * 148;
            }

            if (to == 10)
            {
                var levelNCost = CostsTable.Where(x => x.Level == 9);
                orbCost += levelNCost.Sum(x => x.WeaponCost) * 187;
            }

            return new ExclusiveWeaponCost(levels.Count(), levels.Sum(x => x.WeaponCost) - currentEProgress, orbCost);
        }
    }
    public class ExclusiveWeaponCost
    {
        public int Level { get; set; }

        private int _weaponCost;
        public int WeaponCost 
        { 
            get => _weaponCost;
            set
            {
                _weaponCost = value;
                EwMatsCost = value * 300;
            }
        }
        public int EwMatsCost { get; set; }
        public int EwOrbCost { get; set; }

        public ExclusiveWeaponCost(int level, int weaponCost, int weaponOrbCost)
        {
            Level = level;
            WeaponCost = weaponCost;
            EwMatsCost = WeaponCost * 300;
            EwOrbCost = weaponOrbCost;
        }
    }
}
