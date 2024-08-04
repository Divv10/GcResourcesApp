using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for SiCoresOpenTierListOverlay.xaml
    /// </summary>
    public partial class SiCoresOpenTierListOverlay : UserControl
    {
        public static readonly DependencyProperty SiLevelProperty = DependencyProperty.Register("SiLevel", typeof(int), typeof(SiCoresOpenTierListOverlay), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty IsCoreOpenProperty = DependencyProperty.Register("IsCoreOpen", typeof(bool), typeof(SiCoresOpenTierListOverlay), new FrameworkPropertyMetadata(null));

        public int SiLevel
        {
            get { return (int)GetValue(SiLevelProperty); }
            set { SetValue(SiLevelProperty, value); }
        }

        public bool IsCoreOpen
        {
            get { return (bool)GetValue(IsCoreOpenProperty); }
            set { SetValue(IsCoreOpenProperty, value); }
        }

        public SiCoresOpenTierListOverlay()
        {
            InitializeComponent();
        }
    }
}
