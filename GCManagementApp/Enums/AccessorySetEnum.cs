using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Enums
{
    public enum AccessorySetEnum
    {
        [Description("None")]
        None = 0,
        [Description("PurpleAccSet")]
        Purple = 1,
        [Description("BlueAccSet")]
        Blue = 2,
        [Description("OrangeAccSet")]
        Orange = 3,
    }
}
