using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Enums
{
    public enum GearSet
    {
        [Description("None")]
        None,
        [Description("Purple - Spirit of Luck")]
        Purple,
        [Description("Blue - Power of Anger")]
        Blue,
        [Description("Green - Vows of Violence")]
        Green,
        [Description("Orange - Dagger of Passion")]
        Orange,
        [Description("Red - Bloody Revenge")]
        Red,
        [Description("Two Red + Two Blue")]
        RedBlue,
    }
}
