using GCManagementApp.Models;
using GCManagementApp.Operations;
using GCManagementApp.Static;
using GCManagementApp.Windows;
using GCManagementApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Serilog;
using Google.Apis.Sheets.v4.Data;
using System.Diagnostics;
using GCManagementApp.Enums;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for ProcessAttachUserControl.xaml
    /// </summary>
    public partial class ProcessAttachUserControl : UserControl, INotifyPropertyChanged
    {
        public ICommand ClickCommand { get; }
        public ICommand SwipeCommand { get; }
        public ICommand ScreenshotCommand { get; }
        public ICommand OpenScreenshotsFolderCommand { get; }

        public bool IsConnected => EmulatorConnectionInfo.IsConnected;

        public bool UseBatchCmdToSwipe
        {
            get => EmulatorConnectionInfo.UseBatchCmdToSwipe;
            set => EmulatorConnectionInfo.UseBatchCmdToSwipe = value;
        }

        public List<EmulatorLanguageEnum> Languages { get; } = Enum.GetValues<EmulatorLanguageEnum>().ToList();
        public List<HeroMatchAlgorithmEnum> Algorithms { get; } = Enum.GetValues<HeroMatchAlgorithmEnum>().ToList();

        public EmulatorLanguageEnum SelectedLanguage
        {
            get => EmulatorConnectionInfo.GameLanguage;
            set 
            { 
                EmulatorConnectionInfo.GameLanguage = value;
                Properties.Settings.Default.GameLanguage = (int)value;
                Properties.Settings.Default.Save();
            }
        }

        public HeroMatchAlgorithmEnum SelectedAlgorithm
        {
            get => (HeroMatchAlgorithmEnum)Properties.Settings.Default.Algorithm;
            set
            {
                Properties.Settings.Default.Algorithm = (int)value;
                Properties.Settings.Default.Save();
            }
        }

        public double Confidence
        {
            get => EmulatorConnectionInfo.Confidence;
            set 
            { 
                EmulatorConnectionInfo.Confidence = value;
                Properties.Settings.Default.Threshold = value;
                Properties.Settings.Default.Save();
            }
        }

        public int SwipeGap
        {
            get => ImageRegions.VerticalGap;
            set 
            {
                ImageRegions.VerticalGap = value;
                Properties.Settings.Default.SwipeGap = value;
                Properties.Settings.Default.Save();
            }
        }

        public ProcessAttachUserControl()
        {
            SelectedLanguage = (EmulatorLanguageEnum)Properties.Settings.Default.GameLanguage;
            Confidence = Properties.Settings.Default.Threshold;
            SwipeGap = Properties.Settings.Default.SwipeGap;

            InitializeComponent();
            DataContext = this;

            ClickCommand = new RelayCommand(Click);
            SwipeCommand = new RelayCommand(Swipe);
            ScreenshotCommand = new RelayCommand(Screenshot);
            OpenScreenshotsFolderCommand = new RelayCommand(OpenScreenshotsFolder);
        }

        private async void Click(object param)
        {
            if (!IsConnected)
                return;

            await AdbOperations.Click(new System.Drawing.Point(1080 / 2, 720 / 2));
        }

        private async void Swipe(object param)
        {            
            if (!IsConnected)
                return;

            try
            {
                var midPoint = new System.Drawing.Point(1080 / 2, 720 / 2);                
                var swipeToPoint = midPoint;
                swipeToPoint.Offset(new System.Drawing.Point(0, ImageRegions.VerticalGap));
                await AdbOperations.Swipe(swipeToPoint, midPoint, 1500, EmulatorConnectionInfo.UseBatchCmdToSwipe);
                Log.Logger.Verbose($"Swiping {swipeToPoint.X} {swipeToPoint.Y} {midPoint.X} {midPoint.Y} {1500}");
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error trying to swipe");
                MessageBox.Show(ex.Message);
            }
        }

        private async void Screenshot(object param)
        {
            if (!IsConnected)            
                return;
            
            var sshot = (await ImageOperations.GetBitmapSourceFromScreen()).ToBitmap();
            var directory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Logs\\ScanImages";
            var path = $"{directory}\\{DateTime.Now.Ticks}.png";
            Directory.CreateDirectory(directory);
            sshot.Save(path, ImageFormat.Png);
        }

        private void OpenScreenshotsFolder(object param)
        {
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\GcManagementApp\\Logs\\ScanImages";
            Directory.CreateDirectory(path);
            Process.Start("explorer", path);
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
            OnPropertyChanged(propName);
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
