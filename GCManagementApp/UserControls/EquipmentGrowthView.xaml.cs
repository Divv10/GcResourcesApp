using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Models;
using GCManagementApp.Static;
using MaterialDesignThemes.Wpf;
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

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for EquipmentGrowthView.xaml
    /// </summary>
    public partial class EquipmentGrowthView : UserControl, INotifyPropertyChanged
    {
        public bool IsPerformanceModeEnabled => GCManagementApp.Properties.Settings.Default.PerformanceMode;

        public SnackbarMessageQueue HeroGrowthSnackbarMessageQueue { get; } = new SnackbarMessageQueue();

        private ObservableCollection<HeroGrowth> _heroes = null!;

        public ObservableCollection<HeroGrowth> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        private int _selectedSetFilterIndex;
        public int SelectedSetFilterIndex
        {
            get => _selectedSetFilterIndex;
            set 
            { 
                SetProperty(ref _selectedSetFilterIndex, value); 
                HeroesView?.Refresh();
            }
        }
        public SetFilterEnum SelectedSetFilter => (SetFilterEnum)SelectedSetFilterIndex;

        private int _selectedGearTierFilterIndex;
        public int SelectedGearTierFilterIndex
        {
            get => _selectedGearTierFilterIndex;
            set
            {
                SetProperty(ref _selectedGearTierFilterIndex, value);
                HeroesView?.Refresh();
            }
        }
        public TierFilterEnum SelectedGearTierFilter => (TierFilterEnum)SelectedGearTierFilterIndex;

        private int _selectedEwFilterIndex;
        public int SelectedEwFilterIndex
        {
            get => _selectedEwFilterIndex;
            set
            {
                SetProperty(ref _selectedEwFilterIndex, value);
                HeroesView?.Refresh();
            }
        }
        public EwFilterEnum SelectedEwFilter => (EwFilterEnum)SelectedEwFilterIndex;

        private int _selectedArtifactFilterIndex;
        public int SelectedArtifactFilterIndex
        {
            get => _selectedArtifactFilterIndex;
            set
            {
                SetProperty(ref _selectedArtifactFilterIndex, value);
                HeroesView?.Refresh();
            }
        }
        public ArtifactFilterEnum SelectedArtifactFilter => (ArtifactFilterEnum)SelectedArtifactFilterIndex;

        private int _selectedOrderIndex;
        public int SelectedOrderIndex
        {
            get => _selectedOrderIndex;
            set
            {
                SetProperty(ref _selectedOrderIndex, value);
                if (HeroesView != null)
                {
                    using (HeroesView.DeferRefresh())
                    {
                        HeroesView.SortDescriptions.Clear();
                        switch (SelectedOrder)
                        {
                            case EquipmentOrderSortEnum.Name:
                                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
                                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
                                break;
                            case EquipmentOrderSortEnum.SetTierSum:
                                HeroesView.SortDescriptions.Add(new SortDescription("Equipment.EquipmentTierSum", ListSortDirection.Descending));
                                break;
                            case EquipmentOrderSortEnum.WeaponT:
                                HeroesView.SortDescriptions.Add(new SortDescription("Equipment.Weapon.Transcendence", ListSortDirection.Descending));
                                break;
                            case EquipmentOrderSortEnum.ArtifactLvl:
                                HeroesView.SortDescriptions.Add(new SortDescription("Equipment.ArtifactUpgrade", ListSortDirection.Descending));
                                HeroesView.SortDescriptions.Add(new SortDescription("Equipment.ArtifactTier", ListSortDirection.Descending));
                                break;
                            case EquipmentOrderSortEnum.EwLvl:
                                HeroesView.SortDescriptions.Add(new SortDescription("Equipment.IsExclusiveWeaponOwned", ListSortDirection.Descending));
                                HeroesView.SortDescriptions.Add(new SortDescription("Equipment.ExclusiveWeaponUpgrade", ListSortDirection.Descending));
                                break;
                        }
                    }
                }

                if (_isInit)
                {
                    Properties.Settings.Default.LastEquipmentSortOrder = value;
                    Properties.Settings.Default.Save();
                }
            }
        }
        public EquipmentOrderSortEnum SelectedOrder => (EquipmentOrderSortEnum)SelectedOrderIndex;

        private GearTier HighestSet { get; }

        public string SetsDone => $"{Properties.Resources.MaxTierSets} {Heroes.Where(h => h.Equipment.Weapon.Tier == HighestSet && h.Equipment.Armor.Tier == HighestSet && h.Equipment.Armor.Tier == HighestSet && h.Equipment.SupportArmorFirst.Tier == HighestSet && h.Equipment.SupportArmorSecond.Tier == HighestSet && h.Equipment.SupportWeapon.Tier == HighestSet).Count()} / {Heroes.Count()}";
        public string EwDone => $"{Properties.Resources.ExclusiveWeaponsOwned} {Heroes.Where(h => h.Equipment.IsExclusiveWeaponOwned && h.Equipment.ExclusiveWeaponUpgrade == 8).Count()} / {Heroes.Count()}";
        public string WeaponTDone => $"{Properties.Resources.WeaponsWithT9} {Heroes.Where(h => h.Equipment.Weapon.Transcendence == 9).Count()} / {Heroes.Count()}";


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

        private HeroGrowth _currentHeroGrowth = null!;
        public HeroGrowth CurrentHeroGrowth
        {
            get => _currentHeroGrowth;
            set => SetProperty(ref _currentHeroGrowth, value);
        }

        private bool _isInit = false;

        public EquipmentGrowthView()
        {
            InitializeComponent();
            DataContext = this;

            HighestSet = ((GearTier[])Enum.GetValues(typeof(GearTier))).Max();

            SelectedOrderIndex = (int)EquipmentOrderSortEnum.Name;
            SelectedArtifactFilterIndex = (int)ArtifactFilterEnum.All;
            SelectedEwFilterIndex = (int)EwFilterEnum.All;
            SelectedGearTierFilterIndex = (int)TierFilterEnum.All;
            SelectedSetFilterIndex = (int)SetFilterEnum.All;

            _isInit = true;

            Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);
            HeroesView.Filter = new Predicate<object>(h => Filter(h as HeroGrowth));

            if (Properties.Settings.Default.LastEquipmentSortOrder >= 0)
            {
                SelectedOrderIndex = Properties.Settings.Default.LastEquipmentSortOrder;
            }
            else
            {
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));
            }

            this.IsVisibleChanged += RefreshCollection;

            ProfileManager.ProfileChanged += (sender, ae) =>
            {
                SelectedOrderIndex = (int)EquipmentOrderSortEnum.Name;
                SelectedArtifactFilterIndex = (int)ArtifactFilterEnum.All;
                SelectedEwFilterIndex = (int)EwFilterEnum.All;
                SelectedGearTierFilterIndex = (int)TierFilterEnum.All;
                SelectedSetFilterIndex = (int)SetFilterEnum.All;

                Heroes = new ObservableCollection<HeroGrowth>(ProfileGrowth.Heroes);
                HeroesView.Filter = new Predicate<object>(h => Filter(h as HeroGrowth));
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.IsOwned), ListSortDirection.Descending));
                HeroesView.SortDescriptions.Add(new SortDescription(nameof(HeroGrowth.DisplayName), ListSortDirection.Ascending));

                OnPropertyChanged(string.Empty);
            };
        }

        #region Filters

        private bool Filter(HeroGrowth heroGrowth)
        {
            return (SelectedSetFilter == SetFilterEnum.All || 
                    (SelectedSetFilter == SetFilterEnum.Orange && IsFullSet(heroGrowth.Equipment, GearSet.Orange)) || 
                    (SelectedSetFilter == SetFilterEnum.Blue && IsFullSet(heroGrowth.Equipment, GearSet.Blue)) || 
                    (SelectedSetFilter == SetFilterEnum.Purple && IsFullSet(heroGrowth.Equipment, GearSet.Purple)) || 
                    (SelectedSetFilter == SetFilterEnum.Green && IsFullSet(heroGrowth.Equipment, GearSet.Green)) || 
                    (SelectedSetFilter == SetFilterEnum.Red && IsFullSet(heroGrowth.Equipment, GearSet.Red)) ||
                    (SelectedSetFilter == SetFilterEnum.Mixed && !IsSameSet(heroGrowth.Equipment) && !IsMissingGear(heroGrowth.Equipment)) ||
                    (SelectedSetFilter == SetFilterEnum.MixedMissing && !IsSameSet(heroGrowth.Equipment) && IsMissingGear(heroGrowth.Equipment)))
                && (SelectedGearTierFilter == TierFilterEnum.All || 
                    (SelectedGearTierFilter == TierFilterEnum.Max && IsEquipSameTier(heroGrowth.Equipment) && IsEquipMaxTier(heroGrowth.Equipment)) ||
                    (SelectedGearTierFilter == TierFilterEnum.Mixed && !IsEquipSameTier(heroGrowth.Equipment) && IsAnyEquipMaxTier(heroGrowth.Equipment)) ||
                    (SelectedGearTierFilter == TierFilterEnum.Below && !IsAnyEquipMaxTier(heroGrowth.Equipment) && !IsFullSetMissing(heroGrowth.Equipment)) ||
                    (SelectedGearTierFilter == TierFilterEnum.None && IsFullSetMissing(heroGrowth.Equipment)))
                && (SelectedEwFilter == EwFilterEnum.All ||
                    (SelectedEwFilter == EwFilterEnum.Owned && heroGrowth.Equipment.IsExclusiveWeaponOwned) ||
                    (SelectedEwFilter == EwFilterEnum.NotOwned && !heroGrowth.Equipment.IsExclusiveWeaponOwned))
                && (SelectedArtifactFilter == ArtifactFilterEnum.All ||
                    (SelectedArtifactFilter == ArtifactFilterEnum.None && heroGrowth.Equipment.ArtifactTier == ArtifactTier.None) ||
                    (SelectedArtifactFilter == ArtifactFilterEnum.Normal && heroGrowth.Equipment.ArtifactType == ArtifactType.Normal && heroGrowth.Equipment.ArtifactTier != ArtifactTier.None) ||
                    (SelectedArtifactFilter == ArtifactFilterEnum.Burning && heroGrowth.Equipment.ArtifactType == ArtifactType.Burning && heroGrowth.Equipment.ArtifactTier != ArtifactTier.None) ||
                    (SelectedArtifactFilter == ArtifactFilterEnum.Cursed && heroGrowth.Equipment.ArtifactType == ArtifactType.Cursed && heroGrowth.Equipment.ArtifactTier != ArtifactTier.None) ||
                    (SelectedArtifactFilter == ArtifactFilterEnum.Frozen && heroGrowth.Equipment.ArtifactType == ArtifactType.Frozen && heroGrowth.Equipment.ArtifactTier != ArtifactTier.None));
        }

        private bool IsSameSet(Equipment equipment)
        {
            return equipment.Weapon.Set == equipment.Armor.Set &&
                equipment.Armor.Set == equipment.SupportArmorFirst.Set &&
                equipment.SupportArmorFirst.Set == equipment.SupportArmorSecond.Set;
        }

        private bool IsFullSet(Equipment equipment, GearSet set)
        {
            return IsSameSet(equipment) && equipment.Weapon.Set == set;
        }

        private bool IsMissingGear(Equipment equipment)
        {
            return equipment.Weapon.Set == GearSet.None ||
                equipment.Armor.Set == GearSet.None ||
                equipment.SupportArmorFirst.Set == GearSet.None ||
                equipment.SupportArmorSecond.Set == GearSet.None;
        }

        private bool IsEquipSameTier(Equipment equipment)
        {
            return equipment.Weapon.Tier == equipment.SupportWeapon.Tier &&
                equipment.SupportWeapon.Tier == equipment.Armor.Tier &&
                equipment.Armor.Tier == equipment.SupportArmorFirst.Tier &&
                equipment.SupportArmorFirst.Tier == equipment.SupportArmorSecond.Tier;
        }

        private bool IsEquipMaxTier(Equipment equipment)
        {
            return IsEquipSameTier(equipment) && equipment.Weapon.Tier == HighestSet;
        }

        private bool IsAnyEquipMaxTier(Equipment equipment)
        {
            return equipment.Weapon.Tier == HighestSet ||
                equipment.SupportWeapon.Tier == HighestSet ||
                equipment.Armor.Tier == HighestSet ||
                equipment.SupportArmorFirst.Tier == HighestSet ||
                equipment.SupportArmorSecond.Tier == HighestSet;
        }

        private bool IsFullSetMissing(Equipment equipment)
        {
         return IsEquipSameTier(equipment) && equipment.Weapon.Tier == GearTier.None;
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
            OnPropertyChanged(nameof(EwDone));
            OnPropertyChanged(nameof(WeaponTDone));
        }

        private void EditHeroClick(object sender, RoutedEventArgs e)
        {
            var hg = (e.Source as Button)?.DataContext as HeroGrowth;
            if (hg != null)
            {
                CurrentHeroGrowth = hg;
                SelectedHeroGrowth = new HeroGrowth() { Hero = hg.Hero, IsOwned = hg.IsOwned, Equipment = hg.Equipment };
            }
            OnPropertyChanged();
        }

        private void SaveProgressClick(object sender, RoutedEventArgs e)
        {
            CurrentHeroGrowth?.Equipment?.UpdateSummaries();
            SelectedHeroGrowth?.Equipment?.UpdateSummaries();
            ProfileGrowth.Heroes = Heroes.ToList();
            ProfileGrowth.SaveToFile();
            HeroGrowthSnackbarMessageQueue?.Enqueue(Properties.Resources.Saved, null, null, null, false, true, TimeSpan.FromSeconds(1.5));

            RefreshCollections();
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

    public enum EquipmentOrderSortEnum
    {
        Name = 0,
        SetTierSum = 1,
        WeaponT = 2,
        ArtifactLvl = 3,
        EwLvl = 4,
    }

    public enum SetFilterEnum
    {
        All = 0,
        Orange = 1,
        Blue = 2,
        Purple = 3,
        Green = 4,
        Red = 5,
        Mixed = 6,
        MixedMissing = 7,
    }

    public enum TierFilterEnum
    {
        All = 0,
        Max = 1,
        Mixed = 2,
        Below = 3,
        None = 4,
    }

    public enum EwFilterEnum
    {
        All = 0,
        Owned = 1,
        NotOwned = 2,
    }

    public enum ArtifactFilterEnum
    {
        All = 0,
        None = 1,
        Normal = 2,
        Burning = 3,
        Frozen = 4,
        Cursed = 5,
    }
}
