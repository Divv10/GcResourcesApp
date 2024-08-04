using GCManagementApp.Enums;

namespace GCManagementApp.Models
{
    public class AwakeningCubeUpgrade
    {
        public AwakeningCubesUpgradeTypeEnum Type { get; set; }
        public int Cost { get; set; }
        public int Sell { get; set; }
        public double BlueGemPerCube => (double)Sell / (double)Cost;
    }
}
