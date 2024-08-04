using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using Microsoft.VisualBasic;
using NetSparkleUpdater.AppCastHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MassEditUserControl.xaml
    /// </summary>
    public partial class MassEditUserControl : UserControl, INotifyPropertyChanged
    {
        public ICommand SortByHeroName { get; }
        public ICommand SortByT { get; }
        public ICommand SortBySi { get; }
        public ICommand SortByCl { get; }
        public ICommand SortByLevel { get; }
        public ICommand SortByPet { get; }
        public ICommand SortByBp { get; }

        private string _filterName;
        public string FilterName
        {
            get => _filterName;
            set
            {
                SetProperty(ref _filterName, value);
                HeroesView.Refresh();
            }
        }

        private bool _showNotOwned = true;
        public bool ShowNotOwned
        {
            get => _showNotOwned;
            set
            {
                SetProperty(ref _showNotOwned, value);
                HeroesView.Refresh();
            }
        }

        public ICollectionView HeroesView
        {
            get { return CollectionViewSource.GetDefaultView(Heroes); }
        }

        private MassEditTemplateTypeEnum _selectedEditType;
        public MassEditTemplateTypeEnum SelectedEditType
        {
            get => _selectedEditType;
            set => SetProperty(ref _selectedEditType, value);
        }

        private ObservableCollection<HeroGrowth> _heroes = null!;
        public ObservableCollection<HeroGrowth> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        public bool IsPerformanceModeEnabled => GCManagementApp.Properties.Settings.Default.PerformanceMode;
        public MassEditTemplateTypeEnum[] EditTypes { get; } = Enum.GetValues<MassEditTemplateTypeEnum>();
        public AccessorySetEnum[] AccessorySetValues { get; } = (AccessorySetEnum[])Enum.GetValues(typeof(AccessorySetEnum));
        public ArtifactTier[] ArtifactTierValues { get; } = ((ArtifactTier[])Enum.GetValues(typeof(ArtifactTier))).OrderByDescending(x => x).ToArray().MoveToFront(x => x == ArtifactTier.None);
        public ArtifactType[] ArtifactTypeValues { get; } = (ArtifactType[])Enum.GetValues(typeof(ArtifactType));

        public MassEditUserControl()
        {
            InitializeComponent();
            DataContext = this;

            SortByHeroName = new RelayCommand(OnSortByHeroName);
            SortByT = new RelayCommand(OnSortByT);
            SortBySi = new RelayCommand(OnSortBySi);
            SortByCl = new RelayCommand(OnSortByCl);
            SortByLevel = new RelayCommand(OnSortByLevel);
            SortByPet = new RelayCommand(OnSortByPet);
            SortByBp = new RelayCommand(OnSortByBp);

            Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes.OrderBy(x => x.DisplayName));
            foreach (var hero in Heroes)
            {
                hero.PropertyChanged += HeroesPropertyChanged;
                hero.Equipment.PropertyChanged += HeroesPropertyChanged;
            };
            using (HeroesView.DeferRefresh())
            {
                HeroesView.Filter = new Predicate<object>(h => Filter(h as HeroGrowth));
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
            }
            OnPropertyChanged(string.Empty);
        }

        private bool Filter(HeroGrowth heroGrowth)
        {
            return
                (FilterName == null || heroGrowth.DisplayName.IndexOf(FilterName, StringComparison.OrdinalIgnoreCase) != -1) && (ShowNotOwned || heroGrowth.IsOwned);
        }

        #region Sort

        private void OnSortByHeroName(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
            }
        }

        private void OnSortByT(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.TranscendenceLevel), ListSortDirection.Descending));
            }
        }

        private void OnSortBySi(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.SiLevel), ListSortDirection.Descending));
            }
        }

        private void OnSortByCl(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.ChaserLevel), ListSortDirection.Descending));
            }
        }

        private void OnSortByLevel(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.Level), ListSortDirection.Descending));
            }
        }

        private void OnSortByPet(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.PetLevel), ListSortDirection.Descending));
            }
        }

        private void OnSortByBp(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.BP), ListSortDirection.Descending));
            }
        }

        #endregion

        private void HeroesPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ProfileGrowth.SaveToFile();
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
            OnPropertyChanged(propName!);
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
