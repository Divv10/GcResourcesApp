using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DivideGT
{
    /// <summary>
    /// Interaction logic for DefenseDisplay.xaml
    /// </summary>
    public partial class DefenseDisplay : UserControl, INotifyPropertyChanged
    {
        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return null;
        }

        public Defense DefenseData
        {
            get => (Defense)GetValue(DefenseDataProperty);
            set => SetValue(DefenseDataProperty, value);
        }

        public Visibility IsDefense1Hidden => DefenseData.Hidden1 == 1 || DefenseData.Hidden2 == 1 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsDefense2Hidden => DefenseData.Hidden1 == 2 || DefenseData.Hidden2 == 2 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsDefense3Hidden => DefenseData.Hidden1 == 3 || DefenseData.Hidden2 == 3 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsDefense4Hidden => DefenseData.Hidden1 == 4 || DefenseData.Hidden2 == 4 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsDefense5Hidden => DefenseData.Hidden1 == 5 || DefenseData.Hidden2 == 5 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsDefense6Hidden => DefenseData.Hidden1 == 6 || DefenseData.Hidden2 == 6 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsDefense7Hidden => DefenseData.Hidden1 == 7 || DefenseData.Hidden2 == 7 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsDefense8Hidden => DefenseData.Hidden1 == 8 || DefenseData.Hidden2 == 8 ? Visibility.Visible : Visibility.Collapsed;

        public DefenseDisplay()
        {            
            InitializeComponent();
        }

        public static readonly DependencyProperty DefenseDataProperty = DependencyProperty.Register("DefenseData", typeof(Defense), typeof(DefenseDisplay),
            new PropertyMetadata(new Defense(), new PropertyChangedCallback(OnDefenseDataChanged)));

        private static void OnDefenseDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DefenseDisplay ad = d as DefenseDisplay;
            ad.OnDefenseDataChanged(e);
        }

        private void OnDefenseDataChanged(DependencyPropertyChangedEventArgs e)
        {
            OnPropertyChanged("DefenseData");
            OnPropertyChanged("IsDefense1Hidden");
            OnPropertyChanged("IsDefense2Hidden");
            OnPropertyChanged("IsDefense3Hidden");
            OnPropertyChanged("IsDefense4Hidden");
            OnPropertyChanged("IsDefense5Hidden");
            OnPropertyChanged("IsDefense6Hidden");
            OnPropertyChanged("IsDefense7Hidden");
            OnPropertyChanged("IsDefense8Hidden");
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
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
