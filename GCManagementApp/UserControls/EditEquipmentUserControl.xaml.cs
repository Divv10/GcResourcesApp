using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using PixelLab.Common;
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
    /// Interaction logic for EditEquipmentUserControl.xaml
    /// </summary>
    public partial class EditEquipmentUserControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty HeroGrowthProperty = DependencyProperty.Register(nameof(HeroGrowth), typeof(HeroGrowth), typeof(EditEquipmentUserControl), new FrameworkPropertyMetadata(null));

        public HeroGrowth HeroGrowth
        {
            get => (HeroGrowth)GetValue(HeroGrowthProperty);
            set => SetValue(HeroGrowthProperty, value);
        }

        public GearSet[] GearSetValues { get; } = ((GearSet[])Enum.GetValues(typeof(GearSet))).OrderByDescending(x => x).ToArray().MoveToFront(x => x == GearSet.None);
        public GearTier[] GearTierValues { get; } = ((GearTier[])Enum.GetValues(typeof(GearTier))).OrderByDescending(x => x).ToArray().MoveToFront(x => x == GearTier.None);
        public ArtifactTier[] ArtifactTierValues { get; } = ((ArtifactTier[])Enum.GetValues(typeof(ArtifactTier))).OrderByDescending(x => x).ToArray().MoveToFront(x => x == ArtifactTier.None);
        public ArtifactType[] ArtifactTypeValues { get; } = (ArtifactType[])Enum.GetValues(typeof(ArtifactType));

        public EditEquipmentUserControl()
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
