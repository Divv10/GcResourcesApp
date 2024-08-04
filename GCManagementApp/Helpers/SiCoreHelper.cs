using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Helpers
{
    public static class SiCoreHelper
    {
        public static string GetCoreLabel(this int siLevel)
        {
            return siLevel switch
            {
                0 => Properties.Resources.MemoryCoreOpen,
                5 => Properties.Resources.BodyCoreOpen,
                10 => Properties.Resources.SoulCoreOpen,
                _ => "",
            };
        }
    }
}
