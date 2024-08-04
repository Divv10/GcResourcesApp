using GCManagementApp.Helpers;
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
    /// Interaction logic for MaterialsInventoryView.xaml
    /// </summary>
    public partial class MaterialsInventoryView : UserControl, INotifyPropertyChanged
    {
        private Inventory _inventory;
        public Inventory Inventory
        {
            get => _inventory;
            set => SetProperty(ref _inventory, value);
        }

        public MaterialsInventoryView()
        {
            InitializeComponent();
            DataContext = this;
            Inventory = ProfileGrowth.Profile.MaterialsInventory;

            ProfileManager.ProfileChanged += (sender, ae) =>
            {
                Inventory = ProfileGrowth.Profile.MaterialsInventory;
                OnPropertyChanged(string.Empty);
            };
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

