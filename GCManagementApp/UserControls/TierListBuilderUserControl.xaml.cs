using GCManagementApp.ViewModels;
using System.Windows.Controls;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for TierListBuilderUserControl.xaml
    /// </summary>
    public partial class TierListBuilderUserControl : UserControl
    {     
        public TierListBuilderUserControl()
        {
            InitializeComponent();
            DataContext = new TierListBuilderViewModel();            
        }        
    }
}
