using ControlzEx.Theming;
using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for HeroGrowthView.xaml
    /// </summary>
    public partial class HeroGrowthView : UserControl, INotifyPropertyChanged
    {
        public ICommand FilterHeroType { get; }
        public ICommand FilterDescent { get; }
        public ICommand FilterSi { get; }
        public ICommand FilterCl { get; }
        public ICommand FilterClass { get; }
        public ICommand FilterAttribute { get; }
        public ICommand FilterReset { get; }

        public ICommand SortByHeroName { get; }
        public ICommand SortByT { get; }
        public ICommand SortBySi { get; }
        public ICommand SortByCl { get; }
        public ICommand SortByLevel { get; }
        public ICommand SortByPet { get; }
        public ICommand SortByBp { get; }
        public ICommand SortByDescent { get; }

        public bool IsPerformanceModeEnabled => GCManagementApp.Properties.Settings.Default.PerformanceMode;

        public SnackbarMessageQueue HeroGrowthSnackbarMessageQueue { get; } = new SnackbarMessageQueue();

        private ObservableCollection<HeroGrowth> _heroes = null!;
        public ObservableCollection<HeroGrowth> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        public ICollectionView HeroesView
        {
            get { return CollectionViewSource.GetDefaultView(Heroes); }
        }

        private HeroGrowth _selectedHeroGrowth = null!;
        public HeroGrowth SelectedHeroGrowth
        {
            get => _selectedHeroGrowth;
            set => SetProperty(ref _selectedHeroGrowth, value);
        }

        private RecommendedBuild _recommendedBuild;
        public RecommendedBuild RecommendedBuild
        {
            get => _recommendedBuild;
            set => SetProperty(ref _recommendedBuild, value);
        }

        private HeroGrowth _currentHeroGrowth = null!;
        public HeroGrowth CurrentHeroGrowth
        {
            get => _currentHeroGrowth;
            set => SetProperty(ref _currentHeroGrowth, value);
        }

        private HeroType? _filterHeroTypeEnum;
        public HeroType? FilterHeroTypeEnum 
        {
            get => _filterHeroTypeEnum;
            set => SetProperty(ref _filterHeroTypeEnum, value);
        }
        private DescentFilterEnum? _filterDescentEnum;
        public DescentFilterEnum? FilterDescentEnum
        {
            get => _filterDescentEnum;
            set => SetProperty(ref _filterDescentEnum, value);
        }

        private SiLevelFilterEnum? _filterSiEnum;
        public SiLevelFilterEnum? FilterSiEnum
        {
            get => _filterSiEnum;
            set => SetProperty(ref _filterSiEnum, value);
        }
        private ChaserLevelFilterEnum? _filterClEnum;
        public ChaserLevelFilterEnum? FilterClEnum
        {
            get => _filterClEnum;
            set => SetProperty(ref _filterClEnum, value);
        }
        private HeroClass? _filterClassEnum;
        public HeroClass? FilterClassEnum
        {
            get => _filterClassEnum; 
            set => SetProperty(ref _filterClassEnum, value);
        }
        private HeroAttribute? _filterAttributeEnum;
        public HeroAttribute? FilterAttributeEnum
        {
            get => _filterAttributeEnum; 
            set => SetProperty(ref _filterAttributeEnum, value);
        }

        public int HeroesOwned => Heroes.Count(h => h.IsOwned);
        public int HeroesD10 => Heroes.Count(h => h.DescentLevel == 10);
        public int HeroesSi15 => Heroes.Count(h => h.SiLevel == 15);
        public int HeroesCl25 => Heroes.Count(h => h.ChaserLevel == 25);
        public int HeroesLevel215 => Heroes.Count(h => h.Level >= StaticValues.MaxLevel - 5);
        public int TotalHeroes => Heroes.Count();
        public int MaxEquipTierLevel => (int)StaticValues.MaxLevel - 5;

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

        public HeroGrowthView()
        {
            InitializeComponent();
            DataContext = this;


            Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);

            using (HeroesView.DeferRefresh())
            {
                HeroesView.Filter = new Predicate<object>(h => Filter(h as HeroGrowth));
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
            }

            if (Properties.Settings.Default.LastHeroesSortOrder >= 0)
            {
                switch ((HeroSortOrder)Properties.Settings.Default.LastHeroesSortOrder)
                {
                    case HeroSortOrder.ChaserLevel:
                        OnSortByCl(null);
                        break;
                    case HeroSortOrder.PetLevel:
                        OnSortByPet(null);
                        break;
                    case HeroSortOrder.SiLevel:
                        OnSortBySi(null);
                        break;
                    case HeroSortOrder.HeroLevel:
                        OnSortByLevel(null);
                        break;
                    case HeroSortOrder.HeroName:
                        OnSortByHeroName(null);
                        break;
                    case HeroSortOrder.TranscendenceLevel:
                        OnSortByT(null);
                        break;
                    case HeroSortOrder.Bp:
                        OnSortByBp(null);
                        break;
                    case HeroSortOrder.DescentLevel:
                        OnSortByDescent(null);
                        break;
                }
            }

            FilterHeroType = new RelayCommand(OnFilterHeroType);
            FilterDescent = new RelayCommand(OnFilterDescent);
            FilterSi = new RelayCommand(OnFilterSi);
            FilterCl = new RelayCommand(OnFilterCl);
            FilterClass = new RelayCommand(OnFilterClass);
            FilterAttribute = new RelayCommand(OnFilterAttribute);
            FilterReset = new RelayCommand(OnFiltersReset);

            SortByHeroName = new RelayCommand(OnSortByHeroName);
            SortByT = new RelayCommand(OnSortByT);
            SortBySi = new RelayCommand(OnSortBySi);
            SortByCl = new RelayCommand(OnSortByCl);
            SortByLevel = new RelayCommand(OnSortByLevel);
            SortByPet = new RelayCommand(OnSortByPet);
            SortByBp = new RelayCommand(OnSortByBp);
            SortByDescent = new RelayCommand(OnSortByDescent);

            this.IsVisibleChanged += RefreshCollection;

            ProfileManager.ProfileChanged += (sender, ae) =>
            {
                Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);
                using (HeroesView.DeferRefresh())
                {
                    HeroesView.Filter = new Predicate<object>(h => Filter(h as HeroGrowth));
                    HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
                    HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
                }
                OnPropertyChanged(string.Empty);
            };
        }

        #region Filters

        private bool Filter(HeroGrowth heroGrowth)
        {
            return
                (FilterName == null || heroGrowth.DisplayName.IndexOf(FilterName, StringComparison.OrdinalIgnoreCase) != -1) &&
                (ShowNotOwned || heroGrowth.IsOwned) &&
                (FilterHeroTypeEnum == null || heroGrowth.Hero.HeroType == FilterHeroTypeEnum.Value) &&
                (FilterClassEnum == null || heroGrowth.Hero.HeroClass == FilterClassEnum.Value) &&
                (FilterAttributeEnum == null || heroGrowth.Hero.HeroAttribute == FilterAttributeEnum.Value) &&
                (FilterDescentEnum == null || FilterDescentEnum == DescentFilterEnum.D10 && heroGrowth.DescentLevel == 10) || 
                    (FilterDescentEnum == DescentFilterEnum.D8 && heroGrowth.DescentLevel < 10 && heroGrowth.DescentLevel >= 8) ||
                    (FilterDescentEnum == DescentFilterEnum.D5 && heroGrowth.DescentLevel < 8 && heroGrowth.DescentLevel >=5) || 
                    (FilterDescentEnum == DescentFilterEnum.D3 && heroGrowth.DescentLevel < 5 && heroGrowth.DescentLevel >= 3) ||
                    (FilterDescentEnum == DescentFilterEnum.D0 && heroGrowth.DescentLevel < 3 && heroGrowth.DescentLevel >= 0) &&
                (FilterSiEnum == null ||
                    (FilterSiEnum == SiLevelFilterEnum.SI15 && heroGrowth.SiLevel == 15) ||
                    (FilterSiEnum == SiLevelFilterEnum.SI10 && heroGrowth.SiLevel < 15 && heroGrowth.SiLevel >= 10) ||
                    (FilterSiEnum == SiLevelFilterEnum.SI5 && heroGrowth.SiLevel < 10 && heroGrowth.SiLevel >= 5) ||
                    (FilterSiEnum == SiLevelFilterEnum.SI0 && heroGrowth.SiLevel < 5 && heroGrowth.SiLevel >= 0)) &&
                (FilterClEnum == null ||
                    (FilterClEnum == ChaserLevelFilterEnum.CL25 && heroGrowth.ChaserLevel == 25) ||
                    (FilterClEnum == ChaserLevelFilterEnum.CL20 && heroGrowth.ChaserLevel < 25 && heroGrowth.ChaserLevel >= 20) ||
                    (FilterClEnum == ChaserLevelFilterEnum.CL0 && heroGrowth.ChaserLevel < 20 && heroGrowth.ChaserLevel >= 0));
        }

        private void OnFilterHeroType(object param) 
        {
            FilterHeroTypeEnum = param as HeroType?;
            HeroesView.Refresh();
        }

        private void OnFilterDescent(object param)
        {
            FilterDescentEnum = param as DescentFilterEnum?;
            HeroesView.Refresh();
        }

        private void OnFilterSi(object param)
        {
            FilterSiEnum = param as SiLevelFilterEnum?;
            HeroesView.Refresh();
        }

        private void OnFilterCl(object param)
        {
            FilterClEnum = param as ChaserLevelFilterEnum?;
            HeroesView.Refresh();
        }

        private void OnFilterClass(object param)
        {
            FilterClassEnum = param as HeroClass?;
            HeroesView.Refresh();
        }

        private void OnFilterAttribute(object param) 
        {
            FilterAttributeEnum = param as HeroAttribute?;
            HeroesView.Refresh();
        }

        private void OnFiltersReset(object param)
        {
            FilterHeroTypeEnum = null;
            FilterSiEnum = null;
            FilterClEnum = null;
            FilterClassEnum = null;
            FilterAttributeEnum = null;
            HeroesView.Refresh();
        }

        #endregion

        #region Sort

        private void OnSortByHeroName(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
            }
            Properties.Settings.Default.LastHeroesSortOrder = (int)HeroSortOrder.HeroName;
            Properties.Settings.Default.Save();
        }

        private void OnSortByT(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.TranscendenceLevel), ListSortDirection.Descending));
            }
            Properties.Settings.Default.LastHeroesSortOrder = (int)HeroSortOrder.TranscendenceLevel;
            Properties.Settings.Default.Save();
        }

        private void OnSortBySi(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.SiLevel), ListSortDirection.Descending));
            }
            Properties.Settings.Default.LastHeroesSortOrder = (int)HeroSortOrder.SiLevel;
            Properties.Settings.Default.Save();
        }

        private void OnSortByCl(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.ChaserLevel), ListSortDirection.Descending));
            }
            Properties.Settings.Default.LastHeroesSortOrder = (int)HeroSortOrder.ChaserLevel;
            Properties.Settings.Default.Save();
        }

        private void OnSortByLevel(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.Level), ListSortDirection.Descending));
            }
            Properties.Settings.Default.LastHeroesSortOrder = (int)HeroSortOrder.HeroLevel;
            Properties.Settings.Default.Save();
        }

        private void OnSortByPet(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.PetLevel), ListSortDirection.Descending));
            }
            Properties.Settings.Default.LastHeroesSortOrder = (int)HeroSortOrder.PetLevel;
            Properties.Settings.Default.Save();
        }

        private void OnSortByBp(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.BP), ListSortDirection.Descending));
            }
            Properties.Settings.Default.LastHeroesSortOrder = (int)HeroSortOrder.Bp;
            Properties.Settings.Default.Save();
        }

        private void OnSortByDescent(object param)
        {
            using (HeroesView.DeferRefresh())
            {
                HeroesView.SortDescriptions.Clear();
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DescentLevel), ListSortDirection.Descending));
            }
            Properties.Settings.Default.LastHeroesSortOrder = (int)HeroSortOrder.DescentLevel;
            Properties.Settings.Default.Save();
        }

        #endregion

        private void EditHeroClick(object sender, RoutedEventArgs e)
        {
            var hg = (e.Source as Button)?.DataContext as HeroGrowth;
            EditHero(hg, false);
        }

        public void EditHero(HeroGrowth hg, bool openDrawerManually)
        {
            try
            {
                if (hg != null)
                {
                    CurrentHeroGrowth = hg;
                    SelectedHeroGrowth = new HeroGrowth() { Hero = hg.Hero, IsOwned = hg.IsOwned, Level = hg.Level, ChaserLevel = hg.ChaserLevel, PetLevel = hg.PetLevel, SiLevel = hg.SiLevel, TranscendenceLevel = hg.TranscendenceLevel, Ring = hg.Ring, Earrings = hg.Earrings, Necklace = hg.Necklace, Equipment = hg.Equipment, IsCoreOpen = hg.IsCoreOpen, TraitsOpen = hg.TraitsOpen, BP = hg.BP, TransPercentage = hg.TransPercentage, DescentLevel = hg.DescentLevel };
                    RecommendedBuild = BuildsHelper.GetBuildForHero(new Tuple<HeroEnum, HeroType>(hg.Hero.HeroName, hg.Hero.HeroType));
                    if (!hg.IsOwned)
                    {
                        SelectedHeroGrowth.IsOwned = true;
                    }
                }
                OnPropertyChanged();

                if (openDrawerManually)
                    HeroDrawer.IsLeftDrawerOpen = true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error opening hero panel");
            }
        }

        private void SaveProgressClick(object sender, RoutedEventArgs e)
        {
            CurrentHeroGrowth.PetLevel = SelectedHeroGrowth.PetLevel;
            CurrentHeroGrowth.ChaserLevel = SelectedHeroGrowth.ChaserLevel;
            CurrentHeroGrowth.SiLevel = SelectedHeroGrowth.SiLevel;
            CurrentHeroGrowth.DescentLevel = SelectedHeroGrowth.DescentLevel;
            CurrentHeroGrowth.Level = SelectedHeroGrowth.Level;
            CurrentHeroGrowth.TranscendenceLevel = SelectedHeroGrowth.TranscendenceLevel;
            CurrentHeroGrowth.IsOwned = SelectedHeroGrowth.IsOwned;
            CurrentHeroGrowth.IsCoreOpen = SelectedHeroGrowth.IsCoreOpen;
            CurrentHeroGrowth.TraitsOpen = SelectedHeroGrowth.TraitsOpen;
            CurrentHeroGrowth.Ring.AccessoryTier = SelectedHeroGrowth.Ring.AccessoryTier;
            CurrentHeroGrowth.Ring.AccessoryUpgradeLevel = SelectedHeroGrowth.Ring.AccessoryUpgradeLevel;
            CurrentHeroGrowth.Ring.AccessorySet = SelectedHeroGrowth.Ring.AccessorySet;
            CurrentHeroGrowth.Necklace.AccessoryTier = SelectedHeroGrowth.Necklace.AccessoryTier;
            CurrentHeroGrowth.Necklace.AccessoryUpgradeLevel = SelectedHeroGrowth.Necklace.AccessoryUpgradeLevel;
            CurrentHeroGrowth.Necklace.AccessorySet = SelectedHeroGrowth.Necklace.AccessorySet;
            CurrentHeroGrowth.Earrings.AccessoryTier = SelectedHeroGrowth.Earrings.AccessoryTier;
            CurrentHeroGrowth.Earrings.AccessoryUpgradeLevel = SelectedHeroGrowth.Earrings.AccessoryUpgradeLevel;
            CurrentHeroGrowth.Earrings.AccessorySet = SelectedHeroGrowth.Earrings.AccessorySet;
            CurrentHeroGrowth.BP = SelectedHeroGrowth.BP;
            CurrentHeroGrowth.TransPercentage = SelectedHeroGrowth.TransPercentage;
            ProfileGrowth.Heroes = Heroes.ToList();
            ProfileGrowth.SaveToFile();
            HeroGrowthSnackbarMessageQueue?.Enqueue(Properties.Resources.Saved, null, null, null, false, true, TimeSpan.FromSeconds(1.5));

            OnPropertyChanged(nameof(HeroesCl25));
            OnPropertyChanged(nameof(HeroesSi15));
            OnPropertyChanged(nameof(HeroesOwned));
            OnPropertyChanged(nameof(HeroesLevel215));
        }

        private void RefreshCollection(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RefreshCollections();
            }
        }
        private void HeroDrawer_DrawerClosing(object sender, DrawerClosingEventArgs e)
        {
            BuildUserControl.SiSelector.SelectedIndex = -1;
            BuildUserControl.SiSelector.Text = "SI Traits";
        }
        private void RefreshCollections()
        {
            ////Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);
            //using (HeroesView.DeferRefresh())
            //{
            //    HeroesView.Filter = new Predicate<object>(h => Filter(h as HeroGrowth));
            //    HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
            //    HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
            //}
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

    public enum HeroSortOrder
    {
        HeroName = 0,
        TranscendenceLevel = 1,
        SiLevel = 2,
        ChaserLevel = 3,
        HeroLevel = 4,
        PetLevel = 5,
        Bp = 6,
        DescentLevel = 7,
    }
}
