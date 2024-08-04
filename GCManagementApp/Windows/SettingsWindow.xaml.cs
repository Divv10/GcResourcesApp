using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System;
using GCManagementApp.Static;
using ControlzEx.Theming;
using MaterialDesignThemes.Wpf;
using GCManagementApp.Enums;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;

namespace GCManagementApp.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public ICommand OpenLogsCommand { get; set; }

        public bool IsPerformanceModeEnabled
        {
            get => Properties.Settings.Default.PerformanceMode;
            set
            {
                Properties.Settings.Default.PerformanceMode = value;
                Properties.Settings.Default.Save();                
            }
        }

        public bool IsVerticalVulcaEnabled
        {
            get => Properties.Settings.Default.VulcanusVerticalPlanner;
            set
            {
                Properties.Settings.Default.VulcanusVerticalPlanner = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool DifferentDpiBehaviour
        {
            get => Properties.Settings.Default.DifferentDpiBehavior;
            set
            {
                Properties.Settings.Default.DifferentDpiBehavior = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool IsDarkThemeEnabled
        {
            get => Properties.Settings.Default.DarkTheme;
            set
            {
                Properties.Settings.Default.DarkTheme = value;
                Properties.Settings.Default.Save();

                if (value)
                {
                    var paletteHelper = new PaletteHelper();
                    ITheme theme = paletteHelper.GetTheme();
                    theme.SetBaseTheme(MaterialDesignThemes.Wpf.Theme.Dark);
                    paletteHelper.SetTheme(theme);

                    ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
                }
                else
                {
                    var paletteHelper = new PaletteHelper();
                    ITheme theme = paletteHelper.GetTheme();
                    theme.SetBaseTheme(MaterialDesignThemes.Wpf.Theme.Light);
                    paletteHelper.SetTheme(theme);

                    ThemeManager.Current.ChangeTheme(this, "Light.Blue");
                }
            }
        }

        public LanguageEnum SelectedLanguage
        {
            get => Enum.Parse<LanguageEnum>(Properties.Settings.Default.Language, true);
            set
            {
                Properties.Settings.Default.Language = value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        public IEnumerable<LanguageEnum> AvailableLanguages { get; } = Enum.GetValues(typeof(LanguageEnum)).Cast<LanguageEnum>();

        public SettingsWindow()
        {
            InitializeComponent();
            DataContext = this;

            OpenLogsCommand = new RelayCommand(OpenLogs);
        }

        private void OpenLogs(object param)
        {
            Process.Start("explorer", $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Logs");
        }

        #region PC

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> raiser)
        {
            var propName = ((MemberExpression)raiser?.Body)?.Member.Name;
            OnPropertyChanged(propName!);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        #endregion
    }
}
