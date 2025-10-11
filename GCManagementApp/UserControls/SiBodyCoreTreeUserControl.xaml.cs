using GCManagementApp.Enums;
using GCManagementApp.Models;
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
    /// Interaction logic for SiBodyCoreTreeUserControl.xaml
    /// </summary>
    public partial class SiBodyCoreTreeUserControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty SiBodyTraitsProperty = DependencyProperty.Register(nameof(SiBodyTrait), typeof(List<List<int>>), typeof(SiBodyCoreTreeUserControl));

        public List<List<int>> SiBodyTrait
        {
            get => (List<List<int>>)GetValue(SiBodyTraitsProperty);
            set => SetValue(SiBodyTraitsProperty, value);
        }

        public SiBodyCoreTreeUserControl()
        {
            InitializeComponent();
            DataContext = this;
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
    }
}
