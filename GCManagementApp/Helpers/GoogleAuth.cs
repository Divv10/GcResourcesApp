using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Helpers
{
    public static class GoogleAuth
    {
        public static string JsonContent =>
            @"put your own auth json here";

        public static string BuildsSheetName => "Builds";
        public static string ContentKeysSheetName => "Content Keys";
        public static string EquipmentSheetName => "Equipment";

        public static string DatabaseSheetId => "1FU4RI2MMvSQkO0k4c4IxgwY-hx2YFKNBIfhsXT4uC-I";
    }
}
