using GCManagementApp.Helpers;
using GCManagementApp.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GCManagementApp.Models
{
    [System.Diagnostics.DebuggerDisplay("{DisplayName}")]
    public class HeroGrowth : NotifyPropertyChanged
    {
        private Hero _hero;
        public Hero Hero
        {
            get => _hero;
            set => SetProperty(ref _hero, value);
        }

        private bool _isOwned;
        public bool IsOwned
        {
            get => _isOwned;
            set
            {
                SetProperty(ref _isOwned, value);
                if (value && Hero.HeroType == Enums.HeroType.T)
                {
                    Level = Math.Max(100, Level);
                    TranscendenceLevel = Math.Max(6, TranscendenceLevel);
                    ChaserLevel = Math.Max(20, ChaserLevel);
                }
            }
        }

        private int _level;
        public int Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        private int _petLevel;
        public int PetLevel
        {
            get => _petLevel;
            set => SetProperty(ref _petLevel, value);
        }

        private int _chaserLevel;
        public int ChaserLevel
        {
            get => _chaserLevel; 
            set => SetProperty(ref _chaserLevel, value);
        }

        private int _siLevel;
        public int SiLevel
        {
            get => _siLevel;
            set 
            { 
                SetProperty(ref _siLevel, value);
                var max = AvailableSiTraits.MaximumTraits(SiLevel, IsCoreOpen);
                var min = AvailableSiTraits.MinimumTraits(SiLevel);
                if (TraitsOpen > max)
                {
                    TraitsOpen = (int)max;
                }
                if (TraitsOpen < min)
                {
                    TraitsOpen = (int)min;
                }
            }
        }

        private bool _isCoreOpen;
        public bool IsCoreOpen
        {
            get => _isCoreOpen;
            set
            {
                SetProperty(ref _isCoreOpen, value);
                var max = AvailableSiTraits.MaximumTraits(SiLevel, IsCoreOpen);
                var min = AvailableSiTraits.MinimumTraits(SiLevel);
                if (TraitsOpen > max)
                {
                    TraitsOpen = (int)max;
                }
                if (TraitsOpen < min)
                {
                    TraitsOpen = (int)min;
                }
            }
        }

        private int _transcendenceLevel;
        public int TranscendenceLevel
        {
            get => _transcendenceLevel; 
            set => SetProperty(ref _transcendenceLevel, value);
        }

        private int _traitsOpen;
        public int TraitsOpen
        {
            get => _traitsOpen;
            set => SetProperty(ref _traitsOpen, value);
        }

        private int _bp;
        public int BP
        {
            get => _bp;
            set => SetProperty(ref _bp, value);
        }

        private Accessory _ring;
        public Accessory Ring
        {
            get => _ring; 
            set => SetProperty(ref _ring, value);
        }

        public int RingUpgradeSum => ((int)Ring.AccessoryTier + 1) * Ring.AccessoryUpgradeLevel;

        private Accessory _necklace;
        public Accessory Necklace
        {
            get => _necklace;
            set => SetProperty(ref _necklace, value);
        }

        public int NecklaceUpgradeSum => ((int)Necklace.AccessoryTier + 1) * Necklace.AccessoryUpgradeLevel;

        private Accessory _earrings;
        public Accessory Earrings
        {
            get => _earrings;
            set => SetProperty(ref _earrings, value);
        }

        private Equipment equipment;
        public Equipment Equipment
        {
            get => equipment;
            set => SetProperty(ref equipment, value);
        }

        public int EarringsUpgradeSum => ((int)Earrings.AccessoryTier + 1) * Earrings.AccessoryUpgradeLevel;

        public int TotalAccessoryUpgradeSum => RingUpgradeSum + NecklaceUpgradeSum + EarringsUpgradeSum;

        public string DisplayName => $"{Properties.Resources.ResourceManager.GetObject(Hero.HeroName.GetDescription()) ?? Hero.HeroName}{(Hero.HeroType == Enums.HeroType.T ? "(T)" : Hero.HeroType == Enums.HeroType.S ? "(S)" : "")}";

        public string ImageName => $"{Hero.HeroName}{Hero.HeroType}";

        public ICommand EditHeroCommand => new RelayCommand(p => ((MainWindow)App.Current.MainWindow).SelectCharacter((HeroGrowth)p));
    }
}
