using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for AccessoryGrowthView.xaml
    /// </summary>
    public partial class AccessoryGrowthView : UserControl, INotifyPropertyChanged
    {
        public bool IsPerformanceModeEnabled => GCManagementApp.Properties.Settings.Default.PerformanceMode;

        private ObservableCollection<HeroGrowth> _heroes = null!;

        public ObservableCollection<HeroGrowth> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        private int _selectedFilterIndex;
        public int SelectedFilterIndex
        {
            get => _selectedFilterIndex;
            set 
            { 
                SetProperty(ref _selectedFilterIndex, value); 
                HeroesView?.Refresh();
            }
        }

        public WorkingFilterEnum SelectedFilter => (WorkingFilterEnum)SelectedFilterIndex;

        private int _selectedOrderIndex;
        public int SelectedOrderIndex
        {
            get => _selectedOrderIndex;
            set
            {
                SetProperty(ref _selectedOrderIndex, value);
                if (HeroesView != null)
                {
                    HeroesView.SortDescriptions.Clear();
                    switch (SelectedOrder)
                    {
                        case UpgradeOrderEnum.Sum:
                            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.TotalAccessoryUpgradeSum), ListSortDirection.Descending));
                            break;
                        case UpgradeOrderEnum.Ring:
                            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.RingUpgradeSum), ListSortDirection.Descending));
                            break;
                        case UpgradeOrderEnum.Necklace:
                            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.NecklaceUpgradeSum), ListSortDirection.Descending));
                            break;
                        case UpgradeOrderEnum.Earrings:
                            HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.EarringsUpgradeSum), ListSortDirection.Descending));
                            break;
                    }
                    HeroesView.Refresh();
                }

                if (_isInit)
                {
                    Properties.Settings.Default.LastAccessoriesSortOrder = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public UpgradeOrderEnum SelectedOrder => (UpgradeOrderEnum)SelectedOrderIndex;

        private int _selectedVisibilityIndex;
        public int SelectedVisibilityIndex
        {
            get => _selectedVisibilityIndex;
            set 
            { 
                SetProperty(ref _selectedVisibilityIndex, value);
                HeroesView?.Refresh();
            }
        }

        public VisibilitySetEnum SelectedSet => (VisibilitySetEnum)SelectedSetIndex;

        private int _selectedSetIndex;
        public int SelectedSetIndex
        {
            get => _selectedSetIndex;
            set
            {
                SetProperty(ref _selectedSetIndex, value);
                HeroesView?.Refresh();
            }
        }

        private bool _isEditEnabled;
        public bool IsEditEnabled
        {
            get => _isEditEnabled;
            set 
            { 
                SetProperty(ref _isEditEnabled, value); 
                OnPropertyChanged(nameof(DesiredItemHeight));
            }
        }

        public int EditsStarted { get; set; }

        public VisibilityFilterEnum SelectedVisibility => (VisibilityFilterEnum)SelectedVisibilityIndex;

        public int SetsDone => Heroes.Where(h => h.TotalAccessoryUpgradeSum == 3 * 18).Count();

        public int SetsWorking => Heroes.Where(h =>  h.TotalAccessoryUpgradeSum < 3 * 18 && h.TotalAccessoryUpgradeSum > 0).Count();

        public int SetsNotDone => Heroes.Where(h => h.TotalAccessoryUpgradeSum == 0).Count();

        public int DesiredItemHeight => IsEditEnabled ? 400 : 185;

        public AccessorySetEnum[] AccessorySetValues { get; } = (AccessorySetEnum[])Enum.GetValues(typeof(AccessorySetEnum));

        public ICollectionView HeroesView
        {
            get { return CollectionViewSource.GetDefaultView(Heroes); }
        }

        private bool _isInit = false;

        public AccessoryGrowthView()
        {
            InitializeComponent();
            DataContext = this;

            SelectedFilterIndex = (int)WorkingFilterEnum.All;
            SelectedOrderIndex = (int)UpgradeOrderEnum.Sum;
            SelectedVisibilityIndex = (int)VisibilityFilterEnum.All;
            SelectedSetIndex = (int)VisibilitySetEnum.All;

            _isInit = true;

            this.iconWarningUpdateAcc.Visibility = Visibility.Collapsed;
            this.txtWarningUpdateAcc.Visibility = Visibility.Collapsed;

            Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);
            foreach (var h in Heroes)
            {
                if (h.Ring == null)
                    h.Ring = new Accessory();
                h.Ring.PropertyChanged += HeroPropertyChanged;

                if (h.Necklace == null)
                    h.Necklace = new Accessory();
                h.Necklace.PropertyChanged += HeroPropertyChanged;

                if (h.Earrings == null)
                    h.Earrings = new Accessory();
                h.Earrings.PropertyChanged += HeroPropertyChanged;
            }
            HeroesView.Filter = new Predicate<object>(h => Filter(h as HeroGrowth));

            if (Properties.Settings.Default.LastAccessoriesSortOrder >= 0)
            {
                SelectedOrderIndex = Properties.Settings.Default.LastAccessoriesSortOrder;
            }
            else
            {
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.TotalAccessoryUpgradeSum), ListSortDirection.Descending));
            }

            this.IsVisibleChanged += RefreshCollection;

            ProfileManager.ProfileChanged += (sender, ae) =>
            {
                Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);
                foreach (var h in Heroes)
                {
                    if (h.Ring == null)
                        h.Ring = new Accessory();
                    h.Ring.PropertyChanged += HeroPropertyChanged;

                    if (h.Necklace == null)
                        h.Necklace = new Accessory();
                    h.Necklace.PropertyChanged += HeroPropertyChanged;

                    if (h.Earrings == null)
                        h.Earrings = new Accessory();
                    h.Earrings.PropertyChanged += HeroPropertyChanged;
                }
                HeroesView.Filter = new Predicate<object>(h => Filter(h as HeroGrowth));
                OnPropertyChanged(string.Empty);
            };
        }

        private void HeroPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ProfileGrowth.SaveToFile();
            RefreshCollections();
        }

        #region Filters

        private bool Filter(HeroGrowth heroGrowth)
        {
            return ((SelectedFilter == WorkingFilterEnum.All || 
                    (SelectedFilter == WorkingFilterEnum.Done && IsDone(heroGrowth)) || 
                    (SelectedFilter == WorkingFilterEnum.NotDone && IsNotDone(heroGrowth)) || 
                    (SelectedFilter == WorkingFilterEnum.Working && IsWorking(heroGrowth)))
                && (SelectedVisibility == VisibilityFilterEnum.All || 
                    (SelectedVisibility == VisibilityFilterEnum.AnyT1 && AnyAccessoryTier(heroGrowth, AccessoryTierEnum.T1)) ||
                    (SelectedVisibility == VisibilityFilterEnum.AnyT2 && AnyAccessoryTier(heroGrowth, AccessoryTierEnum.T2)) ||
                    (SelectedVisibility == VisibilityFilterEnum.AnyT3 && AnyAccessoryTier(heroGrowth, AccessoryTierEnum.T3)) ||
                    (SelectedVisibility == VisibilityFilterEnum.AnyT4 && AnyAccessoryTier(heroGrowth, AccessoryTierEnum.T4)) ||
                    (SelectedVisibility == VisibilityFilterEnum.OnlyT1 && AllAccessoryTier(heroGrowth, AccessoryTierEnum.T1)) ||
                    (SelectedVisibility == VisibilityFilterEnum.OnlyT2 && AllAccessoryTier(heroGrowth, AccessoryTierEnum.T2)) ||
                    (SelectedVisibility == VisibilityFilterEnum.OnlyT3 && AllAccessoryTier(heroGrowth, AccessoryTierEnum.T3)) ||
                    (SelectedVisibility == VisibilityFilterEnum.OnlyT4 && AllAccessoryTier(heroGrowth, AccessoryTierEnum.T4)))
                && (SelectedSet == VisibilitySetEnum.All ||
                    (SelectedSet == VisibilitySetEnum.Blue && AllAccessorySet(heroGrowth, AccessorySetEnum.Blue)) ||
                    (SelectedSet == VisibilitySetEnum.Orange && AllAccessorySet(heroGrowth, AccessorySetEnum.Orange)) ||
                    (SelectedSet == VisibilitySetEnum.Purple && AllAccessorySet(heroGrowth, AccessorySetEnum.Purple)) ||
                    (SelectedSet == VisibilitySetEnum.Mixed && !AllAccessorySameSet(heroGrowth)))
                   );
        }

        #endregion

        private void RefreshCollection(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                RefreshCollections();
            }
        }

        private void RefreshCollections()
        {
            OnPropertyChanged(nameof(SetsDone));
            OnPropertyChanged(nameof(SetsNotDone));
            OnPropertyChanged(nameof(SetsWorking));
        }

        private bool IsDone(HeroGrowth heroGrowth)
        {
            int maxTier = (int)Enum.GetValues(typeof(AccessoryTierEnum)).Cast<AccessoryTierEnum>().Last() + 1;
            return heroGrowth.TotalAccessoryUpgradeSum == 9 * maxTier * 3;
        }

        private bool IsWorking(HeroGrowth heroGrowth)
        {
            int maxTier = (int)Enum.GetValues(typeof(AccessoryTierEnum)).Cast<AccessoryTierEnum>().Last() + 1;
            return heroGrowth.TotalAccessoryUpgradeSum != 9 * maxTier * 3 && heroGrowth.TotalAccessoryUpgradeSum != 0;
        }

        private bool IsNotDone(HeroGrowth heroGrowth)
        {
            return heroGrowth.TotalAccessoryUpgradeSum == 0;
        }

        private bool AnyAccessoryTier(HeroGrowth heroGrowth, AccessoryTierEnum tier)
        {
            return heroGrowth.Ring.AccessoryTier == tier || heroGrowth.Necklace.AccessoryTier == tier || heroGrowth.Earrings.AccessoryTier == tier;
        }

        private bool AllAccessoryTier(HeroGrowth heroGrowth, AccessoryTierEnum tier)
        {
            return heroGrowth.Ring.AccessoryTier == tier && heroGrowth.Necklace.AccessoryTier == tier && heroGrowth.Earrings.AccessoryTier == tier;
        }

        private bool AllAccessorySameSet(HeroGrowth heroGrowth)
        {
            return heroGrowth.Ring.AccessorySet == heroGrowth.Necklace.AccessorySet && heroGrowth.Necklace.AccessorySet == heroGrowth.Earrings.AccessorySet;
        }

        private bool AllAccessorySet(HeroGrowth heroGrowth, AccessorySetEnum accessorySet)
        {
            return AllAccessorySameSet(heroGrowth) && heroGrowth.Ring.AccessorySet == accessorySet;
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

        private void Flipper_IsFlippedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (e.NewValue)
                EditsStarted++;
            else
                EditsStarted--;

            IsEditEnabled = EditsStarted != 0;


            // When all editing cards are done (get unflipped) editing, refresh entire view.
            if (!IsEditEnabled)
            {
                iconWarningUpdateAcc.Visibility = Visibility.Collapsed;
                txtWarningUpdateAcc.Visibility = Visibility.Collapsed;
                HeroesView?.Refresh();   
            }
            else
            {
                // Show reminder when at least 1 editing card are being edited.
                iconWarningUpdateAcc.Visibility = Visibility.Visible;
                txtWarningUpdateAcc.Text = "All changes will be visible when editing is done.";
                txtWarningUpdateAcc.Visibility = Visibility.Visible;
            }
            
        }
    }

    public enum WorkingFilterEnum
    {
        Done = 0,
        Working = 1,
        NotDone = 2,
        All = 3,
    }

    public enum UpgradeOrderEnum
    {
        Ring = 0,
        Necklace = 1,
        Earrings = 2,
        Sum = 3,
    }

    public enum VisibilityFilterEnum
    {
        AnyT1 = 0, 
        AnyT2 = 1,
        AnyT3 = 2,
        AnyT4 = 3,
        OnlyT1 = 4,
        OnlyT2 = 5,
        OnlyT3 = 6,
        OnlyT4 = 7,
        All = 8,
    }

    public enum VisibilitySetEnum
    {
        Orange = 0,
        Blue = 1,
        Purple = 2,
        Mixed = 3,
        All = 4,
    }
}
