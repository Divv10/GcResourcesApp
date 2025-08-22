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
using System.Windows.Controls;
using System.Windows.Input;

namespace GCManagementApp.UserControls
{
    /// <summary>
    /// Interaction logic for MultipleMassEffectUserControl.xaml
    /// </summary>
    public partial class MultipleMassEffectUserControl : UserControl, INotifyPropertyChanged
    {
        public ICommand RemoveStatCommand { get; set; }
        public ICommand AddNewStatCommand { get; set; }
        public ICommand ModifyHeroesCommand { get; set; }

        public ObservableCollection<SelectableHero> Heroes { get; set; }

        public bool? IsAllHeroesSelected
        {
            get
            {
                var selected = Heroes.Select(item => item.IsSelected).Distinct().ToList();
                return selected.Count == 1 ? selected.Single() : (bool?)null;
            }
            set
            {
                if (value.HasValue)
                {
                    SelectAll(value.Value, Heroes);
                    OnPropertyChanged();
                }
            }
        }

        private HeroGrowth _placeholderHero;
        public HeroGrowth PlaceholderHero
        {
            get => _placeholderHero;
            set => SetProperty(ref _placeholderHero, value);
        }

        public ObservableCollection<MassEditTypeEnum> SelectedStats { get; set; }
        public AccessorySetEnum[] AccessorySetValues { get; } = (AccessorySetEnum[])Enum.GetValues(typeof(AccessorySetEnum));
        public ArtifactTier[] ArtifactTierValues { get; } = ((ArtifactTier[])Enum.GetValues(typeof(ArtifactTier))).OrderByDescending(x => x).ToArray().MoveToFront(x => x == ArtifactTier.None);
        public ArtifactType[] ArtifactTypeValues { get; } = (ArtifactType[])Enum.GetValues(typeof(ArtifactType));

        public MultipleMassEffectUserControl()
        {
            InitializeComponent();
            DataContext = this;

            RemoveStatCommand = new RelayCommand(RemoveStat);
            AddNewStatCommand = new RelayCommand(AddStat);
            ModifyHeroesCommand = new RelayCommand(ModifyStats);

            Heroes = new ObservableCollection<SelectableHero>(Hero.GetHeroesCollection.Select(x => new SelectableHero() { Hero = x}).OrderBy(x => x.Hero.ToString()));
            SelectedStats = new ObservableCollection<MassEditTypeEnum>();
            PlaceholderHero = new HeroGrowth() { Equipment = new Equipment(), Earrings = new Accessory(), Necklace = new Accessory(), Ring = new Accessory() };
        }

        private static void SelectAll(bool select, IEnumerable<SelectableHero> models)
        {
            foreach (var model in models)
            {
                model.IsSelected = select;
            }
        }

        private void RemoveStat(object param)
        {
            MassEditTypeEnum? stat = SelectedStats.FirstOrDefault(x => x == (MassEditTypeEnum)param);
            if (stat != null)
            {
                SelectedStats.Remove(stat.Value);
            }
        }

        private async void AddStat(object param)
        {
            var dlg = new MassEditAddStatDialog(SelectedStats);

            try
            {
                var result = await DialogHost.Show(dlg, "StatsDialog");
                if ((bool)result)
                {
                    SelectedStats.Add(dlg.SelectedStat);
                }
            } catch (InvalidOperationException) { return; }
        }

        private async void ModifyStats(object param)
        {
            if (SelectedStats.Count == 0 || Heroes.Count(x => x.IsSelected) == 0)
                return;

            var dlg = new MessageBoxDialog() { DisplayText = $"Are You sure to edit {SelectedStats.Count} for {Heroes.Count(x => x.IsSelected)} heroes?" };
            var result = await DialogHost.Show(dlg, "StatsDialog");
            if ((bool)result)
            {
                foreach (var h in Heroes.Where(x => x.IsSelected))
                {
                    var hero = ProfileGrowth.Heroes.FirstOrDefault(x => x.Hero.HeroName == h.Hero.HeroName && x.Hero.HeroType == h.Hero.HeroType);
                    if (hero != null)
                    {
                        foreach (var stat in SelectedStats)
                        {
                            switch (stat)
                            {
                                case MassEditTypeEnum.Transcendence:
                                    hero.TranscendenceLevel = PlaceholderHero.TranscendenceLevel; break;
                                case MassEditTypeEnum.Level:
                                    hero.Level = PlaceholderHero.Level; break;
                                case MassEditTypeEnum.Pet:
                                    hero.PetLevel= PlaceholderHero.PetLevel; break;
                                case MassEditTypeEnum.Chaser:
                                    hero.ChaserLevel = PlaceholderHero.ChaserLevel; break;
                                case MassEditTypeEnum.Descent:
                                    hero.DescentLevel = PlaceholderHero.DescentLevel; break;
                                case MassEditTypeEnum.SoulImprint:
                                    hero.SiLevel = PlaceholderHero.SiLevel;
                                    hero.IsCoreOpen = PlaceholderHero.IsCoreOpen;
                                    hero.TraitsOpen = PlaceholderHero.TraitsOpen;
                                    break;
                                case MassEditTypeEnum.AccessoryEarring:
                                    hero.Earrings = PlaceholderHero.Earrings; break;
                                case MassEditTypeEnum.AccessoryRing:
                                    hero.Ring = PlaceholderHero.Ring; break;
                                case MassEditTypeEnum.AccessoryNecklace:
                                    hero.Necklace = PlaceholderHero.Necklace; break;
                                case MassEditTypeEnum.ExclusiveWeapon:
                                    hero.Equipment.IsExclusiveWeaponOwned = PlaceholderHero.Equipment.IsExclusiveWeaponOwned;
                                    hero.Equipment.ExclusiveWeaponUpgrade = PlaceholderHero.Equipment.ExclusiveWeaponUpgrade;
                                    break;
                                case MassEditTypeEnum.Artifact:
                                    hero.Equipment.ArtifactType = PlaceholderHero.Equipment.ArtifactType;
                                    hero.Equipment.ArtifactTier = PlaceholderHero.Equipment.ArtifactTier;
                                    hero.Equipment.ArtifactUpgrade = PlaceholderHero.Equipment.ArtifactUpgrade;
                                    break;
                            }
                        }
                        
                        hero.IsOwned = SelectedStats.Count() > 0 ? true : hero.IsOwned;
                    }
                }
            }
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
