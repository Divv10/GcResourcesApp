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

        public static string BuildsSheetName => "All-In-One";
        public static string ContentKeysSheetName => "Content Keys";
        public static string EquipmentSheetName => "Equipment";

        public static string DatabaseSheetId => "16jpYyBX6Ni9noRNCH9mIJJCY7tozxrzXbA7bfrGWZSk";
    }
}
