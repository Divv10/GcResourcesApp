using GCManagementApp.Helpers;
using GCManagementApp.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace GCManagementApp.Windows
{
    /// <summary>
    /// Interaction logic for ShowRegionWindow.xaml
    /// </summary>
    public partial class ShowRegionWindow : Window, INotifyPropertyChanged
    {
        public ICommand CloseWindow { get; }

        private string _windowSize;
        public string WindowSize
        {
            get => _windowSize;
            set => SetProperty(ref _windowSize, value);
        }

        public ShowRegionWindow()
        {            
            CloseWindow = new RelayCommand(p => Close());
            InitializeComponent();
            DataContext = this;
            this.Loaded += ShowRegionWindow_Loaded;
            this.Closing += ShowRegionWindow_Closing;

            var wndsCount = EmulatorConnectionInfo.RegionWindowList.Count;
            for (int w = wndsCount - 1; w >= 0; w--)
            {
                try
                {
                    EmulatorConnectionInfo.RegionWindowList[w].Close();
                }
                catch { }
            }
        }

        private void ShowRegionWindow_Closing(object sender, CancelEventArgs e)
        {
            var thisWindow = EmulatorConnectionInfo.RegionWindowList.FirstOrDefault(w => w == this);
            if (thisWindow != null)
            {
                EmulatorConnectionInfo.RegionWindowList.Remove(thisWindow);
            }
            this.Closing -= ShowRegionWindow_Closing;
        }

        private void ShowRegionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowSize = $"{this.ActualWidth}x{this.ActualHeight}";
            this.Loaded -= ShowRegionWindow_Loaded;
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
