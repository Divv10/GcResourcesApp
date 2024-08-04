using GCManagementApp.Enums;
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
    /// Interaction logic for GearSlotsClassUserControl.xaml
    /// </summary>
    public partial class GearSlotsClassUserControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ClassTypeProperty = DependencyProperty.Register("ClassType", typeof(HeroClass), typeof(GearSlotsClassUserControl), new FrameworkPropertyMetadata(HeroClass.Tank, new PropertyChangedCallback(OnClassTypePropertyChanged)));

        public ICommand SelectGearSlotCommand { get; }

        public HeroClass ClassType
        {
            get { return (HeroClass)GetValue(ClassTypeProperty); }
            set { SetValue(ClassTypeProperty, value); }
        }

        private ClassGearSlots _gearSlots;
        public ClassGearSlots GearSlots
        {
            get => _gearSlots;
            set => SetProperty(ref _gearSlots, value);
        }

        private GearSlot _selectedGearSlot;
        public GearSlot SelectedGearSlot
        {
            get => _selectedGearSlot;
            set => SetProperty(ref _selectedGearSlot, value);
        }

        private WeaponTypeEnum _selectedSlotType;
        public WeaponTypeEnum SelectedSlotType
        {
            get => _selectedSlotType;
            set => SetProperty(ref _selectedSlotType, value);
        }

        private GearSlotCost _currentSlotCost = new GearSlotCost();
        public GearSlotCost CurrentSlotCost
        {
            get => _currentSlotCost;
            set => SetProperty(ref _currentSlotCost, value);
        }

        private GearSlotCost _currentClassSlotCost = new GearSlotCost();
        public GearSlotCost CurrentClassSlotCost
        {
            get => _currentClassSlotCost;
            set => SetProperty(ref _currentClassSlotCost, value);
        }

        private GearSlotCost _totalCost = new GearSlotCost();
        public GearSlotCost TotalCost
        {
            get => _totalCost;
            set => SetProperty(ref _totalCost, value);
        }

        public GearSlotRankEnum[] GearSlotRankValues { get; set; } = Enum.GetValues<GearSlotRankEnum>().ToArray();

        public GearSlotsClassUserControl()
        {
            InitializeComponent();
            DataContext = this;

            switch (ClassType)
            {
                default:
                    GearSlots = ProfileGrowth.Profile.TankGearSlots; break;
                case HeroClass.Healer:
                    GearSlots = ProfileGrowth.Profile.HealerGearSlots; break;
                case HeroClass.Assault:
                    GearSlots = ProfileGrowth.Profile.AssaultGearSlots; break;
                case HeroClass.Ranger:
                    GearSlots = ProfileGrowth.Profile.RangerGearSlots; break;
                case HeroClass.Mage:
                    GearSlots = ProfileGrowth.Profile.MageGearSlots; break;
            }

            SelectedGearSlot = GearSlots.Weapon;
            UpdateCalculations();

            SelectGearSlotCommand = new RelayCommand(SelectGearSlot);
            ProfileGrowth.GearSlotChangedEvent += (s, e) => { UpdateCalculations(); };
        }

        private static void OnClassTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GearSlotsClassUserControl gscuc)
            {
                switch ((HeroClass)e.NewValue)
                {
                    default:
                        gscuc.GearSlots = ProfileGrowth.Profile.TankGearSlots; break;
                    case HeroClass.Healer:
                        gscuc.GearSlots = ProfileGrowth.Profile.HealerGearSlots; break;
                    case HeroClass.Assault:
                        gscuc.GearSlots = ProfileGrowth.Profile.AssaultGearSlots; break;
                    case HeroClass.Ranger:
                        gscuc.GearSlots = ProfileGrowth.Profile.RangerGearSlots; break;
                    case HeroClass.Mage:
                        gscuc.GearSlots = ProfileGrowth.Profile.MageGearSlots; break;
                }

                gscuc.SelectedGearSlot = gscuc.GearSlots.Weapon;

                var currentSlotCost = GearSlotsUpgradingCost.CalculateCostForSlot(gscuc.SelectedGearSlot);
                gscuc.CurrentSlotCost.RaidMatsCost = currentSlotCost.RaidMatsCost;
                gscuc.CurrentSlotCost.BlueStonesCost = currentSlotCost.BlueStonesCost;
            }
        }

        private void UpdateCalculations()
        {
            var currentSlotCost = GearSlotsUpgradingCost.CalculateCostForSlot(SelectedGearSlot);
            CurrentSlotCost.RaidMatsCost = currentSlotCost.RaidMatsCost;
            CurrentSlotCost.BlueStonesCost = currentSlotCost.BlueStonesCost;

            var currentClassSlotsCost = GearSlotsUpgradingCost.CalculateCostForClass(ClassType);
            CurrentClassSlotCost.RaidMatsCost = currentClassSlotsCost.RaidMatsCost;
            CurrentClassSlotCost.BlueStonesCost = currentClassSlotsCost.BlueStonesCost;

            var totalCost = GearSlotsUpgradingCost.CalculateCostForAllClasses();
            TotalCost.RaidMatsCost = totalCost.RaidMatsCost;
            TotalCost.BlueStonesCost = totalCost.BlueStonesCost;
        }

        private void SelectGearSlot(object param)
        {
            switch (param.ToString())
            {
                default:
                    SelectedGearSlot = GearSlots.Weapon;
                    SelectedSlotType = WeaponTypeEnum.Weapon;
                    break;
                case "SubWeapon":
                    SelectedGearSlot = GearSlots.SecondaryWeapon;
                    SelectedSlotType = WeaponTypeEnum.SubWeapon;
                    break;
                case "Armor":
                    SelectedGearSlot = GearSlots.Armor;
                    SelectedSlotType = WeaponTypeEnum.Armor;
                    break;
                case "SubArmor1":
                    SelectedGearSlot = GearSlots.SecondaryArmorOne;
                    SelectedSlotType = WeaponTypeEnum.SubArmor1;
                    break;
                case "SubArmor2":
                    SelectedGearSlot = GearSlots.SecondaryArmorTwo;
                    SelectedSlotType = WeaponTypeEnum.SubArmor2;
                    break;
            }

            var currentSlotCost = GearSlotsUpgradingCost.CalculateCostForSlot(SelectedGearSlot);
            CurrentSlotCost.RaidMatsCost = currentSlotCost.RaidMatsCost;
            CurrentSlotCost.BlueStonesCost = currentSlotCost.BlueStonesCost;
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
