using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Enums
{
    public enum MassEditTemplateTypeEnum
    {
        [Description("SoulImprint")]
        SoulImprint,
        [Description("ChaserTranscendence")]
        ChaserTranscendence,
        [Description("LevelPetLevel")]
        LevelPetLevel,
        [Description("Accessories")]
        Accessory,
        [Description("ExclusiveWeapon")]
        ExclusiveWeapon,
        [Description("Artifact")]
        Artifact,
        [Description("Descent")]
        Descent,
    }
}
