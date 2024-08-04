using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Enums
{
    public enum HeroClass
    {
        [Description("Tank")]
        Tank,
        [Description("Assault")]
        Assault,
        [Description("Ranger")]
        Ranger,
        [Description("Mage")]
        Mage,
        [Description("Healer")]
        Healer,
    }
}
