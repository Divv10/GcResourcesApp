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
            new ExclusiveWeaponCost(-1, 1),
            new ExclusiveWeaponCost(0, 1),
            new ExclusiveWeaponCost(1, 1),
            new ExclusiveWeaponCost(2, 1),
            new ExclusiveWeaponCost(3, 1),
            new ExclusiveWeaponCost(4, 2),
            new ExclusiveWeaponCost(5, 2),
            new ExclusiveWeaponCost(6, 2),
            new ExclusiveWeaponCost(7, 3),
            new ExclusiveWeaponCost(8, 0),
        };

        public static ExclusiveWeaponCost CalculateCost(int from, int to, int currentProgress)
        {
            var cost = CostsTable.FirstOrDefault(x => x.Level == from);
            
            var levels = CostsTable.Where(x => x.Level >= from && x.Level < to);
            var y = levels.Sum(x => x.WeaponCost);
            return new ExclusiveWeaponCost(levels.Count(), levels.Sum(x => x.WeaponCost) - currentProgress);
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

        public ExclusiveWeaponCost(int level, int weaponCost)
        {
            Level = level;
            WeaponCost = weaponCost;
            EwMatsCost = WeaponCost * 300;
        }
    }
}
