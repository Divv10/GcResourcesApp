using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using ControlzEx.Theming;
using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using GCManagementApp.Windows;
using MaterialDesignThemes.Wpf;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;
using Serilog;
using Theme = MaterialDesignThemes.Wpf.Theme;

namespace GCManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public ICommand ShowSettingsWindowCommand { get; }
        public ICommand ShowGoogleDriveWindowCommand { get; }
        public ICommand CheckUpdateCommand { get; }
        public ICommand ShowHelpDocCommand { get; }
        public ICommand OpenProfileWindowCommand { get; }
        public ICommand OpenDonateWindowCommand { get; }
        public ICommand ShowDataSyncWindowCommand { get; }

        public string AppVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string CurrentProfileName => ProfileManager.SelectedProfile?.Name;

        public bool IsVerticalVulcaEnabled => Properties.Settings.Default.VulcanusVerticalPlanner;

        public bool DifferentDpiBehaviour => Properties.Settings.Default.DifferentDpiBehavior;

        public SnackbarMessageQueue MainWindowSnackbarMessageQueue { get; } = new SnackbarMessageQueue();

        private SparkleUpdater _sparkle;
        public MainWindow()
        {
            DataContext = this;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Conditional(evt => evt.Level == Serilog.Events.LogEventLevel.Verbose, wt => wt.File($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Logs\\SyncToolLog.txt", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", flushToDiskInterval: TimeSpan.FromSeconds(.5)))
                .WriteTo.Conditional(evt => evt.Level != Serilog.Events.LogEventLevel.Verbose, wt => wt.File($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Logs\\log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", flushToDiskInterval: TimeSpan.FromSeconds(.5)))
                
                .CreateLogger();

            Log.Information($"Starting application. Version: {AppVersion}");

            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            var traceListener = new XamlTraceListener();
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            PresentationTraceSources.DataBindingSource.Listeners.Add(traceListener);

            ShowSettingsWindowCommand = new RelayCommand(p => WindowsHelper.ShowSettingsWindow());
            ShowGoogleDriveWindowCommand = new RelayCommand(p => WindowsHelper.ShowGoogleDriveWindow());
            CheckUpdateCommand = new RelayCommand(async p => await _sparkle.CheckForUpdatesAtUserRequest());
            ShowHelpDocCommand = new RelayCommand(p =>
            {
                Process.Start(new ProcessStartInfo { FileName = "https://docs.google.com/document/d/1FmHbjoEmuxzWGgGDhnf68sHzoTcfJ-moU_9Hgh_nsxI", UseShellExecute = true });
            });
            OpenProfileWindowCommand = new RelayCommand(p => WindowsHelper.ShowProfilesWindowDialog());
            OpenDonateWindowCommand = new RelayCommand(p => WindowsHelper.ShowDonateWindowDialog());
            ShowDataSyncWindowCommand = new RelayCommand(p => WindowsHelper.ShowDataSyncWindowDialog());

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.Language))
            {
                Properties.Settings.Default.Language = LanguageEnum.English.ToString();
                Properties.Settings.Default.Save();
            }
            var lang = Enum.Parse<LanguageEnum>(Properties.Settings.Default.Language, true);
            var langCode = lang.GetDescription();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);

            InitializeApp();
            // Check on start, and the hookup on event
            CheckProfileForTransUpgrade(null, null);
            ProfileManager.ProfileChanged += CheckProfileForTransUpgrade;

            InitializeComponent();

            if (Properties.Settings.Default.DarkTheme)
            {
                var paletteHelper = new PaletteHelper();
                Theme theme = paletteHelper.GetTheme();
                theme.SetBaseTheme(MaterialDesignThemes.Wpf.BaseTheme.Dark);
                paletteHelper.SetTheme(theme);

                ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
            }
            else
            {
                var paletteHelper = new PaletteHelper();
                Theme theme = paletteHelper.GetTheme();
                theme.SetBaseTheme(MaterialDesignThemes.Wpf.BaseTheme.Light);
                paletteHelper.SetTheme(theme);

                ThemeManager.Current.ChangeTheme(this, "Light.Blue");
            }

            _sparkle = new SparkleUpdater(Properties.Settings.Default.UpdateCheckEndpoint, new Ed25519Checker(SecurityMode.Unsafe))
            {
                UIFactory = new NetSparkleUpdater.UI.WPF.UIFactory(),
                RelaunchAfterUpdate = true,
                CustomInstallerArguments = "",
                LogWriter= new LogWriter(LogWriterOutputMode.None),
            };
            _sparkle.StartLoop(true, true);

            if (!DifferentDpiBehaviour)
                Window_StateChanged(this, EventArgs.Empty);
            else
                HandleWinMaximized();
            this.StateChanged += Window_StateChanged;

            ProfileManager.ProfileNameChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(CurrentProfileName));
            };

            if (Properties.Settings.Default.AppOpenInMins < 30)
            {
                DispatcherTimer dt = new DispatcherTimer();
                dt.Interval = TimeSpan.FromSeconds(60);
                dt.Tick += (s, e) =>
                {
                    dt.Stop();
                    Properties.Settings.Default.AppOpenInMins += 1;
                    Properties.Settings.Default.Save();

                    if (Properties.Settings.Default.AppOpenInMins >= 30)
                    {
                        MainWindowSnackbarMessageQueue.Enqueue(Properties.Resources.DoYouLikeTheApp, Properties.Resources.Donate.ToUpper(), param => WindowsHelper.ShowDonateWindowDialog(), Properties.Resources.Donate, true, true, durationOverride: TimeSpan.FromSeconds(8));
                        return;
                    };

                    dt.Start();
                };

                dt.Start();
            }

            try
            {
                BuildsHelper.GetRecommendedBuilds();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error during builds download");
            }
        }

        private void CheckProfileForTransUpgrade(object sender, EventArgs e)
        {
            if (ProfileGrowth.Heroes.Any(x => x.TranscendenceLevel > 6))
            {
                TransUpdateCheck();
                ProfileGrowth.SaveToFile();
            }
        }

        private void TransUpdateCheck()
        {
            var result = MessageBox.Show("Due to the recent update of Transcendence Levels being changed. Would you like for the app to auto-adjust for you?", "Update Adjustment", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ProfileGrowth.Heroes.ForEach(h =>
                {
                    switch (h.TranscendenceLevel)
                    {
                        case 15:
                            h.TranscendenceLevel = 6;
                            break;

                        case 14:
                            h.TranscendenceLevel = 5;
                            break;

                        case 13:
                            h.TranscendenceLevel = 5;
                            break;

                        case 12:
                            h.TranscendenceLevel = 4;
                            break;

                        case 11:
                            h.TranscendenceLevel = 4;
                            break;

                        case 10:
                            h.TranscendenceLevel = 3;
                            break;

                        case 9:
                            h.TranscendenceLevel = 3;
                            break;

                        case 8:
                            h.TranscendenceLevel = 2;
                            break;

                        case 7:
                            h.TranscendenceLevel = 1;
                            break;


                        case 6:
                            h.TranscendenceLevel = 0;
                            break;
                    }
                });
            }
        }

        public void SelectCharacter(HeroGrowth character)
        {
            this.Tabs.SelectedIndex = 0;
            HeroTab.EditHero(character, true);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Log.Error(e.Exception, "TaskScheduler Unhandled exception");
        }

        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception, "Application Unhandled exception");
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Error(e.Exception, "Dispatcher Unhandled exception");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error((Exception)e.ExceptionObject, "AppDomain Unhandled exception");
        }

        private void InitializeApp()
        {
            ProfileGrowth.Initialize();
        }

        #region PC

        public event PropertyChangedEventHandler PropertyChanged = null!;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> raiser)
        {
            var propName = ((MemberExpression)raiser?.Body!)?.Member.Name;
            OnPropertyChanged(propName!);
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null!)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        #endregion

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshMaximizeRestoreButton()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.maximizeButton.Visibility = Visibility.Collapsed;
                this.restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.maximizeButton.Visibility = Visibility.Visible;
                this.restoreButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (!DifferentDpiBehaviour)
                this.RefreshMaximizeRestoreButton();
            else
                HandleWinMaximized();
        }

        private void HandleWinMaximized()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowStyle = WindowStyle.None;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (!DifferentDpiBehaviour)
                ((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
        }

        public static IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            
            if (msg == WM_GETMINMAXINFO)
            {
                // We need to tell the system what our size should be when maximized. Otherwise it will cover the whole screen,
                // including the task bar.
                MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

                // Adjust the maximized size and position to fit the work area of the correct monitor
                IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    MONITORINFO monitorInfo = new MONITORINFO();
                    monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
                    GetMonitorInfo(monitor, ref monitorInfo);
                    RECT rcWorkArea = monitorInfo.rcWork;
                    RECT rcMonitorArea = monitorInfo.rcMonitor;
                    mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
                }

                Marshal.StructureToPtr(mmi, lParam, true);
            }

            return IntPtr.Zero;
        }

        private const int WM_GETMINMAXINFO = 0x0024;

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }
    }
}
