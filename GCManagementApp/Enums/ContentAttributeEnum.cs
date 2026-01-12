using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Enums
{
    public enum ContentAttributeEnum
    {
        [Description("None")]
        None = 0,
        [Description("Punição")]
        Red = 1,
        [Description("Vida")]
        Green = 2,
        [Description("Equilíbrio")]
        Blue = 3,
    }
}
