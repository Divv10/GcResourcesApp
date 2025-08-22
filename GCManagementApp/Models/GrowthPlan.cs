using GCManagementApp.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    [Serializable]
    public class GrowthPlan : NotifyPropertyChanged
    {
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
            set => SetProperty(ref _siLevel, value);
        }

        private int _descentLevel;
        public int DescentLevel
        {
            get => _descentLevel;
            set => SetProperty(ref _descentLevel, value);
        }

        private int _transcendenceLevel;
        public int TranscendenceLevel
        {
            get => _transcendenceLevel;
            set => SetProperty(ref _transcendenceLevel, value);
        }

        private int _dupesForSi;
        public int DupesForSi
        {
            get => _dupesForSi;
            set
            {
                SetProperty(ref _dupesForSi, value);
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

        private int _traitsOpen;
        public int TraitsOpen
        {
            get => _traitsOpen;
            set => SetProperty(ref _traitsOpen, value);
        }

        private int _heroSpecificSiCubesOwned;
        public int HeroSpecificSiCubesOwned
        {
            get => _heroSpecificSiCubesOwned;
            set => SetProperty(ref _heroSpecificSiCubesOwned, value);
        }

        private int _heroSpecificDivineCrystalsOwned;
        public int HeroSpecificDivineCrystalsOwned
        {
            get => _heroSpecificDivineCrystalsOwned;
            set => SetProperty(ref _heroSpecificDivineCrystalsOwned, value);
        }
    }
}
