using System.Drawing;

namespace GCManagementApp.Static
{
    internal static class ImageRegions
    {
        public static Point HorizontalGap => new Point(132, 0);
        public static int VerticalGap { get; set; } = 200;
        public static Rectangle HeroNameArea => new Rectangle(-31, 96, 178, 80);
        public static Rectangle FirstHeroArea => new Rectangle(100, 190, 70, 160);
        public static Rectangle QuestionMarkArea => new Rectangle(190, 10, 70, 60);
        public static Rectangle InventoryArea => new Rectangle(1134, 10, 80, 60);
        public static Rectangle HomeArea => new Rectangle(1186, 0, 92, 76);
        public static Rectangle InventoryTitleArea => new Rectangle(75, 10, 180, 65);
        public static Rectangle ArtiTierArea => new Rectangle(880, 436, 28, 20);
        public static Rectangle ArtiTypeArea => new Rectangle(879, 438, 79, 75);
        public static Rectangle RingTierArea => new Rectangle(960, 432, 46, 24);
        public static Rectangle RingTypeArea => new Rectangle(1030, 438, 20, 20);
        public static Rectangle NecklaceTierArea => new Rectangle(1047, 432, 46, 24);
        public static Rectangle NecklaceTypeArea => new Rectangle(1095, 440, 40, 20);
        public static Rectangle NecklaceTypeCheckArea => new Rectangle(1079, 468, 40, 20);
        public static Rectangle EarringTierArea => new Rectangle(1134, 432, 46, 24);
        public static Rectangle EarringTypeArea => new Rectangle(1183, 440, 40, 20);
    }
}
