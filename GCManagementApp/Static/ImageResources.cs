using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    internal static class ImageResources
    {
        public static string InventoryTitle => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\inventory.png";
        public static string InventoryButton => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\inventory_btn.png";
        public static string HomeButton => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\home_btn.png";
        public static string UnitSR => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\unitsr.png";
        public static string UnitT => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\unitt.png";
        public static string QuestionMark => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\question_mark.png";
        public static string AccOrange => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_orange.png";
        public static string AccBlue => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_blue.png";
        public static string AccPurple => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_purple.png";
        public static string AccPurple2 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_purple2.png";
        public static string AccType1 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_type1.png";
        public static string AccT4 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_t4.png";
        public static string AccT3 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_t3.png";
        public static string AccT2 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_t2.png";
        public static string AccT1 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\acc_t1.png";
        public static string ArtiT5 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\arti_t5.png";
        public static string ArtiT4 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\arti_t4.png";
        public static string ArtiT3 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\arti_t3.png";
        public static string ArtiT2 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\arti_t2.png";
        public static string ArtiT1 => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\arti_t1.png";
        public static string ArtiFrozen => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\arti_frozen.png";
        public static string ArtiCurse => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\arti_cursed.png";
        public static string ArtiBurning => $"Images{(EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.English ? "" : EmulatorConnectionInfo.GameLanguage == Enums.EmulatorLanguageEnum.Portuguese ? "_br" : "_kr")}\\arti_burning.png";
    }
}
