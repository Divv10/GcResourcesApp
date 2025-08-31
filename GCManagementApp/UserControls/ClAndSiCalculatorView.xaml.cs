using GCManagementApp.Models;
using GCManagementApp.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for ClAndSiCalculatorView.xaml
    /// </summary>
    public partial class ClAndSiCalculatorView : UserControl, INotifyPropertyChanged
    {
        private int _currentCl = 0;
        public int CurrentCl
        {
            get => _currentCl;
            set
            {
                SetProperty(ref _currentCl, value);
                CalculateCost();
            }
        }

        private int _desiredCl = 25;
        public int DesiredCl
        {
            get => _desiredCl;
            set
            {
                SetProperty(ref _desiredCl, value);
                CalculateCost();
            }
        }

        private int _currentSi = 0;
        public int CurrentSi
        {
            get => _currentSi;
            set
            {
                SetProperty(ref _currentSi, value);
                CalculateSiCost();
            }
        }

        private int _desiredSi = 15;
        public int DesiredSi
        {
            get => _desiredSi;
            set
            {
                SetProperty(ref _desiredSi, value);
                CalculateSiCost();
            }
        }

        private int _currentDl = 0;
        public int CurrentDl
        {
            get => _currentDl;
            set
            {
                SetProperty(ref _currentDl, value);
                CalculateDCost();
            }
        }

        private int _desiredDl = 10;
        public int DesiredDl
        {
            get => _desiredDl;
            set
            {
                SetProperty(ref _desiredDl, value);
                CalculateDCost();
            }
        }

        public ChaserLevelCost Cost { get; set; }

   
        public SiLevelCost SiCost { get; set; }
  
        public DescendCosts DCost { get; set; }

        public Inventory Inventory { get; set; } = ProfileGrowth.Profile.MaterialsInventory;
        
        public List<ChaserLevelCost> ChaserCostsTable { get; } = ChaserLevelingCosts.CostsTable;

        public List<SiLevelCost> SiCostsTable { get; } = SiLevelingCosts.CostsTable;
 
        public List<DescendCosts> DCostsTable { get; } = DescendingCosts.CostsTable;

        public ClAndSiCalculatorView()
        {
            InitializeComponent();
            DataContext = this;
            CalculateCost();
            CalculateSiCost();
            CalculateDCost();
            //Inventory.PropertyChanged += (s, o) => { OnPropertyChanged(nameof(CraftableSoulEssences)); };
        }

        private void CalculateCost()
        {
            Cost = ChaserLevelingCosts.CalculateCost(CurrentCl, DesiredCl);
            OnPropertyChanged(nameof(Cost));
        }

        private void CalculateSiCost()
        {
            SiCost = SiLevelingCosts.CalculateCost(CurrentSi, DesiredSi);
            OnPropertyChanged(nameof(SiCost));
        }

        private void CalculateDCost()
        {
            DCost = DescendingCosts.CalculateCost(CurrentDl, DesiredDl);
            OnPropertyChanged(nameof(DCost));
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
