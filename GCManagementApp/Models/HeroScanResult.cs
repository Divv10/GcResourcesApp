using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class HeroScanResult : NotifyPropertyChanged
    {
        private Hero _hero;
        public Hero Hero
        {
            get => _hero;
            set => SetProperty(ref _hero, value);
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        private int? _level;
        public int? Level
        {
            get => _level; set => SetProperty(ref _level, value);
        }

        private int? _si;
        public int? Si
        {
            get => _si; set => SetProperty(ref _si, value);
        }

        private int? _chaser;
        public int? Chaser
        {
            get => _chaser; set => SetProperty(ref _chaser, value);
        }

        private int? _trans;
        public int? Trans
        {
            get => _trans; set => SetProperty(ref _trans, value);
        }

        private int? _pet;
        public int? Pet
        {
            get => _pet; set => SetProperty(ref _pet, value);
        }

        private int? _bp;
        public int? Bp
        {
            get => _bp; set => SetProperty(ref _bp, value);
        }

        private int? _ewLevel;
        public int? EwLevel
        {
            get => _ewLevel; set => SetProperty(ref _ewLevel, value);
        }

        private int? _artiTier;
        public int? ArtiTier
        {
            get => _artiTier; set => SetProperty(ref _artiTier, value);
        }

        private int? _artiLevel;
        public int? ArtiLevel
        {
            get => _artiLevel; set => SetProperty(ref _artiLevel, value);
        }

        private int? _artiType;
        public int? ArtiType
        {
            get => _artiType; set => SetProperty(ref _artiType, value);
        }

        private int? _ringTier;
        public int? RingTier
        {
            get => _ringTier; set => SetProperty(ref _ringTier, value); 
        }

        private int? _ringLevel;
        public int? RingLevel
        {
            get => _ringLevel; set => SetProperty(ref _ringLevel, value);
        }

        private int? _ringType;
        public int? RingType
        {
            get => _ringType; set => SetProperty(ref _ringType, value);
        }

        private int? _necklaceTier;
        public int? NecklaceTier
        {
            get => _necklaceTier; set => SetProperty(ref _necklaceTier, value);
        }

        private int? _necklaceLevel;
        public int? NecklaceLevel
        {
            get => _necklaceLevel; set => SetProperty(ref _necklaceLevel, value);
        }

        private int? _necklaceType;
        public int? NecklaceType
        {
            get => _necklaceType; set => SetProperty(ref _necklaceType, value);
        }

        private int? _earringTier;
        public int? EarringTier
        {
            get => _earringTier; set => SetProperty(ref _earringTier, value);
        }

        private int? _earringLevel;
        public int? EarringLevel
        {
            get => _earringLevel; set => SetProperty(ref _earringLevel, value);
        }

        private int? _earringType;
        public int? EarringType
        {
            get => _earringType; set => SetProperty(ref _earringType, value);
        }
    }
}
