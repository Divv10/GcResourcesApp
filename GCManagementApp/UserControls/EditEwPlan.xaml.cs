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
    /// Interaction logic for EditEwPlan.xaml
    /// </summary>
    public partial class EditEwPlan : UserControl, INotifyPropertyChanged
    {
        public List<Hero> HeroesCollection { get; } = ProfileGrowth.Heroes.Where(h => !(h.Equipment.IsExclusiveWeaponOwned && h.Equipment.ExclusiveWeaponUpgrade == 8)).Select(h => h.Hero).OrderBy(h => h.DisplayName).ToList();

        private Hero _selectedHero;
        public Hero SelectedHero
        {
            get => _selectedHero;
            set
            {
                SetProperty(ref _selectedHero, value);
                SelectedHeroDetails = ProfileGrowth.Heroes.FirstOrDefault(h => h.Hero.HeroType == value.HeroType && h.Hero.HeroName == value.HeroName);

                CurrentEwLevel = SelectedHeroDetails.Equipment.IsExclusiveWeaponOwned ? SelectedHeroDetails.Equipment.ExclusiveWeaponUpgrade : 0;
                DesiredEwLevel = CurrentEwLevel;
                CurrentProgress = 0;
                MaxCurrentProgress = ExclusiveWeaponCosts.CostsTable.FirstOrDefault(c => c.Level == CurrentEwLevel).WeaponCost;

                OnPropertyChanged(nameof(IsHeroSelected));
                OnPropertyChanged(nameof(IsCurrentProgressVisible));
            }
        }

        private int _currentEwLevel;
        public int CurrentEwLevel
        {
            get => _currentEwLevel;
            set => SetProperty(ref _currentEwLevel, value);
        }

        private int _desiredEwLevel;
        public int DesiredEwLevel
        {
            get => _desiredEwLevel;
            set 
            { 
                SetProperty(ref _desiredEwLevel, value);
                MaxCurrentProgress = ExclusiveWeaponCosts.CostsTable.FirstOrDefault(c => c.Level == CurrentEwLevel).WeaponCost;
                CurrentProgress = 0;
                OnPropertyChanged(nameof(IsCurrentProgressVisible));
            }
        }

        private int _currentProgress;
        public int CurrentProgress
        {
            get => _currentProgress;
            set => SetProperty(ref _currentProgress, value);
        }

        private int _maxCurrentProgress;
        public int MaxCurrentProgress
        {
            get => _maxCurrentProgress; 
            set => SetProperty(ref _maxCurrentProgress, value);
        }

        public bool IsCurrentProgressVisible => MaxCurrentProgress > 1;

        public bool IsHeroSelected => SelectedHero != null;

        private HeroGrowth _selectedHeroDetails;
        public HeroGrowth SelectedHeroDetails
        {
            get => _selectedHeroDetails;
            set => SetProperty(ref _selectedHeroDetails, value);
        }

        public EditEwPlan()
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
