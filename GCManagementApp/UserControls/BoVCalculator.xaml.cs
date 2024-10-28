using GCManagementApp.Properties;
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
    /// Interaction logic for BoVCalculator.xaml
    /// </summary>
    public partial class BoVCalculator : UserControl, INotifyPropertyChanged
    {
        public ICommand SetMaxLevelCommand { get; set; }

        public int AssaultCL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingAssaultLevel;
            set 
            {
                ProfileGrowth.Profile.Settings.HeroTrainingAssaultLevel = value;
                Update();
            }
        }

        public int AssaultDL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingAssaultDesiredLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingAssaultDesiredLevel = value;
                Update();
            }
        }

        public int RangerCL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingRangerLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingRangerLevel = value;
                Update();
            }
        }

        public int RangerDL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingRangerDesiredLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingRangerDesiredLevel = value;
                Update();
            }
        }

        public int TankCL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingTankLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingTankLevel = value;
                Update();
            }
        }

        public int TankDL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingTankDesiredLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingTankDesiredLevel = value;
                Update();
            }
        }

        public int HealerCL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingHealerLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingHealerLevel = value;
                Update();
            }
        }

        public int HealerDL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingHealerDesiredLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingHealerDesiredLevel = value;
                Update();
            }
        }

        public int MageCL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingMageLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingMageLevel = value;
                Update();
            }
        }

        public int MageDL
        {
            get => ProfileGrowth.Profile.Settings.HeroTrainingMageDesiredLevel;
            set
            {
                ProfileGrowth.Profile.Settings.HeroTrainingMageDesiredLevel = value;
                Update();
            }
        }

        public int WeeklyBoVOther
        {
            get => ProfileGrowth.Profile.Settings.WeeklyBoVIncomeFromOtherSources;
            set
            {
                ProfileGrowth.Profile.Settings.WeeklyBoVIncomeFromOtherSources = value;
                OnPropertyChanged(nameof(DailyBoVOther));
                Update();
            }
        }

        public int WeeklyBoVAdditional
        {
            get => BoV.BoVFromItemShop + BoV.BoVFromMercShop; // + BoV from GB shop should be added here
        }

        public int DailyBoVAdditional
        {
            get => WeeklyBoVAdditional / 7;
        }

        public int DailyBoVOther
        {
            get => ProfileGrowth.Profile.Settings.WeeklyBoVIncomeFromOtherSources / 7;
        }

        private int _assaultBoVNeeded;
        public int AssaultBoVNeeded
        {
            get => _assaultBoVNeeded;
            set => SetProperty(ref _assaultBoVNeeded, value);
        }

        private int _rangerBoVNeeded;
        public int RangerBoVNeeded
        {
            get => _rangerBoVNeeded;
            set => SetProperty(ref _rangerBoVNeeded, value);
        }

        private int _tankBoVNeeded;
        public int TankBoVNeeded
        {
            get => _tankBoVNeeded;
            set => SetProperty(ref _tankBoVNeeded, value);
        }

        private int _healerBoVNeeded;
        public int HealerBoVNeeded
        {
            get => _healerBoVNeeded;
            set => SetProperty(ref _healerBoVNeeded, value);
        }

        private int _mageBoVNeeded;
        public int MageBoVNeeded
        {
            get => _mageBoVNeeded;
            set => SetProperty(ref _mageBoVNeeded, value);
        }

        private int _assaultBoVReadyIn;
        public int AssaultBoVReadyIn
        {
            get => _assaultBoVReadyIn;
            set => SetProperty(ref _assaultBoVReadyIn, value);
        }

        private int _rangerBoVReadyIn;
        public int RangerBoVReadyIn
        {
            get => _rangerBoVReadyIn;
            set => SetProperty(ref _rangerBoVReadyIn, value);
        }

        private int _tankBoVReadyIn;
        public int TankBoVReadyIn
        {
            get => _tankBoVReadyIn;
            set => SetProperty(ref _tankBoVReadyIn, value);
        }

        private int _healerBoVReadyIn;
        public int HealerBoVReadyIn
        {
            get => _healerBoVReadyIn;
            set => SetProperty(ref _healerBoVReadyIn, value);
        }

        private int _mageBoVReadyIn;
        public int MageBoVReadyIn
        {
            get => _mageBoVReadyIn;
            set => SetProperty(ref _mageBoVReadyIn, value);
        }

        private int _totalBoVNeeded;
        public int TotalBoVNeeded
        {
            get => _totalBoVNeeded;
            set => SetProperty(ref _totalBoVNeeded, value);
        }

        private int _daysNeeded;
        public int DaysNeeded
        {
            get => _daysNeeded;
            set => SetProperty(ref _daysNeeded, value);
        }

        public int DailyBoVEnergy => Energy.EnergyTotalWeekly / (7 * 12) * BoV.BoVPerRun;
        public int DailyBoVWL => BoV.BoVFromLab / 7;
        public int DailyBoVIncome => DailyBoVWL + DailyBoVEnergy;

        public BoVCalculator()
        {
            InitializeComponent();
            DataContext = this;

            SetMaxLevelCommand = new RelayCommand(o => SetMaxLevel(o));

            this.IsVisibleChanged += (_, e) =>
            {
                if (e.NewValue as bool? == true)
                    Update();
            };
        }

        private void SetMaxLevel(object param)
        {
            int maxLevel = 720;

            switch (param.ToString())
            {
                case "assault":
                    AssaultDL = maxLevel;
                    OnPropertyChanged(nameof(AssaultDL));
                    break;
                case "ranger":
                    RangerDL = maxLevel;
                    OnPropertyChanged(nameof(RangerDL));
                    break;
                case "tank":
                    TankDL = maxLevel;
                    OnPropertyChanged(nameof(TankDL));
                    break;
                case "healer":
                    HealerDL = maxLevel;
                    OnPropertyChanged(nameof(HealerDL));
                    break;
                case "mage":
                    MageDL = maxLevel;
                    OnPropertyChanged(nameof(MageDL));
                    break;
            }
        }

        private void Update()
        {
            OnPropertyChanged(nameof(DailyBoVIncome));
            OnPropertyChanged(nameof(DailyBoVEnergy));
            OnPropertyChanged(nameof(DailyBoVWL));
            // This makes the app reloading those values when switching back to this tab
            OnPropertyChanged(nameof(WeeklyBoVAdditional));
            OnPropertyChanged(nameof(DailyBoVAdditional));

            AssaultBoVNeeded = AssaultDL > AssaultCL ? HeroTrainingCost.CalculateBoVCost(AssaultCL, AssaultDL) : 0;
            RangerBoVNeeded = RangerDL > RangerCL ? HeroTrainingCost.CalculateBoVCost(RangerCL, RangerDL) : 0;
            TankBoVNeeded = TankDL > TankCL ? HeroTrainingCost.CalculateBoVCost(TankCL, TankDL) : 0;
            HealerBoVNeeded = HealerDL > HealerCL ? HeroTrainingCost.CalculateBoVCost(HealerCL, HealerDL) : 0;
            MageBoVNeeded = MageDL > MageCL ? HeroTrainingCost.CalculateBoVCost(MageCL, MageDL) : 0;

            int dailyBoV = (BoV.TotalBoVWeekly + WeeklyBoVOther) / 7;
            AssaultBoVReadyIn = (int)Math.Ceiling((double)AssaultBoVNeeded / (double)dailyBoV);
            RangerBoVReadyIn = (int)Math.Ceiling((double)RangerBoVNeeded / (double)dailyBoV);
            TankBoVReadyIn = (int)Math.Ceiling((double)TankBoVNeeded / (double)dailyBoV);
            HealerBoVReadyIn = (int)Math.Ceiling((double)HealerBoVNeeded / (double)dailyBoV);
            MageBoVReadyIn = (int)Math.Ceiling((double)MageBoVNeeded / (double)dailyBoV);

            TotalBoVNeeded = AssaultBoVNeeded + RangerBoVNeeded + TankBoVNeeded + HealerBoVNeeded + MageBoVNeeded;
            DaysNeeded = (int)Math.Ceiling((double)TotalBoVNeeded / (double)dailyBoV);
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
