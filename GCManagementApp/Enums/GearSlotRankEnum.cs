using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Enums
{
    public enum GearSlotRankEnum
    {
        [Description("Normal")]
        Normal,
        [Description("Premium")]
        Premium,
        [Description("Rare")]
        Rare,
        [Description("Unique")]
        Unique,
        [Description("Legendary")]
        Legendary,
        [Description("Ancient")]
        Ancient,
    }
}
