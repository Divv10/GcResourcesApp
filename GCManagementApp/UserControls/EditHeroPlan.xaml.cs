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
    /// Interaction logic for EditHeroPlan.xaml
    /// </summary>
    public partial class EditHeroPlan : UserControl, INotifyPropertyChanged
    {
        public ICommand GoBackCommand { get; }
        public ICommand GoNextCommand { get; }

        private WizardStep _step;
        public WizardStep Step
        {
            get => _step;
            set
            {
                SetProperty(ref _step, value);
                OnPropertyChanged(nameof(IsFirstStep));
                OnPropertyChanged(nameof(IsSecondStep));
                                
                if (value == WizardStep.Second && SelectedHero.HeroType == Enums.HeroType.T)
                {
                    SelectedHeroDetails.TranscendenceLevel = Math.Max(0, SelectedHeroDetails.TranscendenceLevel);
                    SelectedHeroDetails.ChaserLevel = Math.Max(20, SelectedHeroDetails.ChaserLevel);
                    SelectedHeroDetails.Level = Math.Max(100, SelectedHeroDetails.Level);
                    OnPropertyChanged(SelectedHero.DisplayName);
                }
                else if (value == WizardStep.Second && SelectedHero.HeroType == Enums.HeroType.S)
                {
                    SelectedHeroDetails.TranscendenceLevel = Math.Max((int)Static.StaticValues.MaxTranscendenceLevel, SelectedHeroDetails.TranscendenceLevel);
                    SelectedHeroDetails.ChaserLevel = Math.Max((int)Static.StaticValues.MaxClLevel, SelectedHeroDetails.ChaserLevel);
                    SelectedHeroDetails.SiLevel = Math.Max((int)Static.StaticValues.MaxSiLevel, SelectedHeroDetails.SiLevel);
                    SelectedHeroDetails.Level = Math.Max(1, SelectedHeroDetails.Level);
                    SelectedHeroDetails.TraitsOpen = 27;
                    OnPropertyChanged(SelectedHero.DisplayName);
                }
                CustomHeroName = SelectedHero.HeroName == HeroEnum.Custom ? HeroPlan.UnknownHeroName : SelectedHero.DisplayName;
            }
        }

        private string _customHeroName;
        public string CustomHeroName
        {
            get => _customHeroName;
            set => SetProperty(ref _customHeroName, value);
        }

        public bool IsFirstStep => Step == WizardStep.First;
        public bool IsSecondStep => Step == WizardStep.Second;

        public List<HeroType> HeroTypes { get; } = Enum.GetValues<HeroType>().ToList();
        public List<HeroClass> HeroClasses { get; } = Enum.GetValues<HeroClass>().ToList();

        private HeroPlan _heroPlane;
        public HeroPlan HeroPlan
        {
            get => _heroPlane;
            set => SetProperty(ref _heroPlane, value);
        }

        private Hero _unknownHero = new Hero(HeroEnum.Custom, Enums.HeroType.SR, Enums.HeroClass.Assault, Enums.HeroAttribute.Blue);

        //public List<Hero> HeroesCollection { get; } = Hero.GetHeroesCollection.OrderBy(h => h.DisplayName).ToList();
        private List<Hero> _heroesCollection;
        public List<Hero> HeroesCollection
        {
            get
            {
                if (_heroesCollection == null)
                {
                    _heroesCollection = ProfileGrowth.Heroes.Where(h => !(h.ChaserLevel == 25 && h.SiLevel == 15 && h.DescentLevel == 10)).Select(h => h.Hero).OrderBy(h => h.DisplayName).ToList();
                    _heroesCollection.Add(_unknownHero);
                }
                return _heroesCollection;
            }
        }

        private Hero _selectedHero;
        public Hero SelectedHero
        {
            get => _selectedHero;
            set
            {
                SetProperty(ref _selectedHero, value);
                SelectedHeroDetails = ProfileGrowth.Heroes.FirstOrDefault(h => h.Hero.HeroType == value?.HeroType && h.Hero.HeroName == value?.HeroName);

                if (SelectedHero.HeroName == HeroEnum.Custom)
                {
                    HeroPlan.DesiredGrowth = new GrowthPlan();
                    SelectedHeroDetails = new HeroGrowth() { Hero = _unknownHero };
                    OnPropertyChanged(nameof(IsHeroSelected));
                    return;
                }

                if (SelectedHero.HeroType == Enums.HeroType.T)
                {
                    SelectedHeroDetails.TranscendenceLevel = Math.Max(0, SelectedHeroDetails.TranscendenceLevel);
                    SelectedHeroDetails.ChaserLevel = Math.Max(20, SelectedHeroDetails.ChaserLevel);
                    SelectedHeroDetails.Level = Math.Max(100, SelectedHeroDetails.Level);
                }
                else if (SelectedHero.HeroType == Enums.HeroType.S)
                {
                    SelectedHeroDetails.TranscendenceLevel = Math.Max((int)Static.StaticValues.MaxTranscendenceLevel, SelectedHeroDetails.TranscendenceLevel);
                    SelectedHeroDetails.ChaserLevel = Math.Max((int)Static.StaticValues.MaxClLevel, SelectedHeroDetails.ChaserLevel);
                    SelectedHeroDetails.SiLevel = Math.Max((int)Static.StaticValues.MaxSiLevel, SelectedHeroDetails.SiLevel);
                    SelectedHeroDetails.Level = Math.Max(1, SelectedHeroDetails.Level);
                    SelectedHeroDetails.TraitsOpen = 27;
                }
                HeroPlan.DesiredGrowth.IsCoreOpen = SelectedHeroDetails.IsCoreOpen;
                HeroPlan.DesiredGrowth.TranscendenceLevel = SelectedHeroDetails.TranscendenceLevel;
                HeroPlan.DesiredGrowth.ChaserLevel = SelectedHeroDetails.ChaserLevel;
                HeroPlan.DesiredGrowth.SiLevel = SelectedHeroDetails.SiLevel;
                HeroPlan.DesiredGrowth.TraitsOpen = SelectedHeroDetails.TraitsOpen;
                HeroPlan.DesiredGrowth.DescentLevel = SelectedHeroDetails.DescentLevel;

                HeroPlan.CurrentGrowth = new GrowthPlan()
                {
                    SiLevel = SelectedHeroDetails.SiLevel,
                    TranscendenceLevel = SelectedHeroDetails.TranscendenceLevel,
                    TraitsOpen = SelectedHeroDetails.TraitsOpen,
                    IsCoreOpen = SelectedHeroDetails.IsCoreOpen,
                    ChaserLevel = SelectedHeroDetails.ChaserLevel,
                    DescentLevel = SelectedHeroDetails.DescentLevel,
                };

                OnPropertyChanged(nameof(IsHeroSelected));
                OnPropertyChanged(nameof(CoreName));
                OnPropertyChanged(nameof(CoreVisible));
                OnPropertyChanged(nameof(CoreEnabled));
            }
        }

        public string CoreName => HeroPlan.DesiredGrowth.SiLevel.GetCoreLabel();

        public bool CoreVisible => (new[] { 0, 5, 10 }).Any(l => l == HeroPlan.DesiredGrowth.SiLevel);

        public bool CoreEnabled => CoreVisible && SelectedHeroDetails != null && !(HeroPlan.DesiredGrowth.SiLevel == SelectedHeroDetails.SiLevel && SelectedHeroDetails.IsCoreOpen);

        public int MaxDupesForTrans
        {
            get
            {
                if (SelectedHeroDetails?.TranscendenceLevel == null)
                return 0;
                return TranscendingCosts.CalculateCost(SelectedHeroDetails.TranscendenceLevel, HeroPlan.DesiredGrowth.TranscendenceLevel).DupesCost;
            }
        }

        public int MaxDupesForSi
        {
            get
            {
                if (SelectedHeroDetails?.SiLevel == null)
                    return 0;
                return SiLevelingCosts.CalculateCost(SelectedHeroDetails.SiLevel, HeroPlan.DesiredGrowth.SiLevel).GrowthCubesCost / 250;
            }
        }

        public int MaxDupesForDescent
        {
            get
            {
                if (SelectedHeroDetails?.DescentLevel == null)
                    return 0;
                return DescendingCosts.CalculateCost(SelectedHeroDetails.DescentLevel, HeroPlan.DesiredGrowth.DescentLevel).DivineCrystalsCost / 1500;
            }
        }

        public bool IsHeroSelected => SelectedHero != null && SelectedHero.HeroName != HeroEnum.Custom;

        private HeroGrowth _selectedHeroDetails;
        public HeroGrowth SelectedHeroDetails
        {
            get => _selectedHeroDetails;
            set => SetProperty(ref _selectedHeroDetails, value);
        }

        public EditHeroPlan()
        {
            InitializeComponent();
            DataContext = this;
            GoNextCommand = new RelayCommand(o => Step = WizardStep.Second);
            GoBackCommand = new RelayCommand(o => Step = WizardStep.First);
            HeroPlan = new HeroPlan() { DesiredGrowth = new GrowthPlan() { ChaserLevel = 0, SiLevel = 0, TranscendenceLevel = 0, DescentLevel = 0 } };
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

        private void SiDesired_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnPropertyChanged(nameof(MaxDupesForSi));
            HeroPlan.DesiredGrowth.DupesForSi = Math.Min(HeroPlan.DesiredGrowth.DupesForSi, MaxDupesForSi);

            if (CoreVisible && HeroPlan.DesiredGrowth.SiLevel == SelectedHeroDetails.SiLevel)
            {
                HeroPlan.DesiredGrowth.IsCoreOpen = SelectedHeroDetails.IsCoreOpen;
            }

            UpdateCoreBindings();
        }

        private void DescentDesired_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnPropertyChanged(nameof(MaxDupesForDescent));
            HeroPlan.DesiredGrowth.DupesForDescent = Math.Min(HeroPlan.DesiredGrowth.DupesForDescent, MaxDupesForDescent);
        }

        private void TranscendenceDesired_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            OnPropertyChanged(nameof(MaxDupesForTrans));
            HeroPlan.DesiredGrowth.DupesForTrans = Math.Min(HeroPlan.DesiredGrowth.DupesForTrans, MaxDupesForTrans);
        }

        private void UpdateCoreBindings()
        {
            OnPropertyChanged(nameof(CoreName));
            OnPropertyChanged(nameof(CoreVisible));
            OnPropertyChanged(nameof(CoreEnabled));
        }
    }

    public enum WizardStep
    {
        First,
        Second,
    }
}
