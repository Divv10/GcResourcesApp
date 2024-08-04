using System.ComponentModel;

namespace GCManagementApp.Enums
{
    public enum ArtifactType
    {
        [Description("Normal")]
        Normal,
        [Description("LegendaryBurning")]
        Burning,
        [Description("LegendaryCursed")]
        Cursed,
        [Description("LegendaryFrozen")]
        Frozen,
    }
}
