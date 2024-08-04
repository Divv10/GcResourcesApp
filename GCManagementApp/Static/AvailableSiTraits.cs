using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class AvailableSiTraits
    {
        public static double MaximumTraits(int siLevel, bool isCoreOpen)
        {
            switch (siLevel)
            {
                default: return 0d;
                case 0:
                    return isCoreOpen ? 9d : 0d;
                case 1:
                case 2:
                case 3:
                case 4:
                    return 9d;
                case 5:
                    return isCoreOpen ? 18d : 9d;
                case 6:
                case 7:
                case 8:
                case 9:
                    return 18d;
                case 10:
                    return isCoreOpen ? 27d : 18d;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    return 27d;
            }
        }

        public static double MinimumTraits(int siLevel)
        {
            switch (siLevel)
            {
                default: return 0d;
                case 1: return 1d;
                case 2: return 2d;
                case 3: return 3d;
                case 4: return 5d;
                case 5: return 7d;
                case 6: return 1d + 9d;
                case 7: return 2d + 9d;
                case 8: return 3d + 9d;
                case 9: return 5d + 9d;
                case 10: return 7d + 9d;
                case 11: return 1d + 18d;
                case 12: return 2d + 18d;
                case 13: return 3d + 18d;
                case 14: return 5d + 18d;
                case 15: return 7d + 18d;
            }
        }
    }
}
