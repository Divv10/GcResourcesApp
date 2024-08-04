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
    /// Interaction logic for StarsDisplay.xaml
    /// </summary>
    public partial class StarsDisplay : UserControl, INotifyPropertyChanged
    {
        protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return null;
        }

        public StarsEnum Stars
        {
            get => (StarsEnum)GetValue(StarsProperty);
            set => SetValue(StarsProperty, value);
        }

        public bool OneStar => (int)Stars > 0 && (int)Stars != 4;
        public bool TwoStar => (int)Stars > 1 && (int)Stars != 4;
        public bool ThreeStar => (int)Stars > 2 && (int)Stars != 4;
        public Visibility ZeroStars => (int)Stars < 4 ? Visibility.Visible : Visibility.Hidden;
        public Visibility NoAttacks => Stars == StarsEnum.NotAttacked ? Visibility.Visible : Visibility.Hidden;

        public StarsDisplay()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty StarsProperty = DependencyProperty.Register("Stars", typeof(StarsEnum), typeof(StarsDisplay), 
            new PropertyMetadata(StarsEnum.NotAttacked, new PropertyChangedCallback(OnStarsChanged)));

        private static void OnStarsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StarsDisplay sd = d as StarsDisplay;
            sd.OnStarsChanged(e);
        }

        private void OnStarsChanged(DependencyPropertyChangedEventArgs e)
        {
            OnPropertyChanged("OneStar");
            OnPropertyChanged("TwoStar");
            OnPropertyChanged("ThreeStar");
            OnPropertyChanged("ZeroStars");
            OnPropertyChanged("NoAttacks");
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
