using GCManagementApp.Enums;
using GCManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GCManagementApp.Static
{
    internal static class EmulatorConnectionInfo
    {
        public static bool IsConnected { get; set; }
        public static bool UseBatchCmdToSwipe { get; set; }
        public static EmulatorLanguageEnum GameLanguage { get; set; }
        public static double Confidence { get; set; } = 0.9;

        public static List<Window> RegionWindowList { get; set; } = new List<Window>();
    }
}
