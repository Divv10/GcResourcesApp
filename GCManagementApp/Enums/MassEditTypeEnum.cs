using System.ComponentModel;

namespace GCManagementApp.Enums
{
    public enum MassEditTypeEnum
    {
        Transcendence,
        Level,
        [Description("Soul Imprint")]
        SoulImprint,
        Chaser,
        Descent,
        Pet,
        [Description("Accessory - Earrings")]
        AccessoryEarring,
        [Description("Accessory - Necklace")]
        AccessoryNecklace,
        [Description("Accessory - Ring")]
        AccessoryRing,
        [Description("Exclusive weapon")]
        ExclusiveWeapon,
        Artifact,
    }
}
