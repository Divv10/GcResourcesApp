using GCManagementApp.UserControls;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GCManagementApp.Windows
{
    public static class WindowsHelper
    {
        private const double MainWindowOpacity = 0.3;

        public static void ShowSettingsWindow()
        {
            var settings = new SettingsWindow()
            {
                Owner = App.Current.MainWindow,
            };

            App.Current.MainWindow.Opacity = MainWindowOpacity;
            settings.ShowDialog();
            App.Current.MainWindow.Opacity = 1;
        }

        public static void ShowGoogleDriveWindow()
        {
            var window = new GoogleDriveWindow()
            {
                Owner = App.Current.MainWindow,
            };

            App.Current.MainWindow.Opacity = MainWindowOpacity;
            window.ShowDialog();
            if (App.Current?.MainWindow?.Opacity != null)
                App.Current.MainWindow.Opacity = 1;
        }

        public static void ShowMessageDialog(string message, Window owner)
        {
            var window = new MessageDialog()
            {
                MessageText = message,
                Owner = owner,
            };

            owner.Opacity = MainWindowOpacity;
            window.ShowDialog();
            owner.Opacity = 1;
        }

        public static void ShowProfilesWindowDialog()
        {
            var window = new ManageProfileWindow()
            {
                Owner = App.Current.MainWindow,
            };

            App.Current.MainWindow.Opacity = MainWindowOpacity;
            window.ShowDialog();
            if (App.Current?.MainWindow?.Opacity != null)
                App.Current.MainWindow.Opacity = 1;
        }

        public static void ShowDonateWindowDialog()
        {
            var window = new DonateWindow()
            {
                Owner = App.Current.MainWindow,
            };

            App.Current.MainWindow.Opacity = MainWindowOpacity;
            window.ShowDialog();
            if (App.Current?.MainWindow?.Opacity != null)
                App.Current.MainWindow.Opacity = 1;
        }

        public static void ShowDataSyncWindowDialog()
        {
            var window = new DataSyncWindow(App.Current.MainWindow)
            {
                //Owner = App.Current.MainWindow,
            };

            //App.Current.MainWindow.Opacity = MainWindowOpacity;
            window.ShowDialog();
            //if (App.Current?.MainWindow?.Opacity != null)
            //    App.Current.MainWindow.Opacity = 1;
        }
    }
}
