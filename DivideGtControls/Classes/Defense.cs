using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideGT
{
    public class Defense : NotifyPropertyChanged
    {
        private HeroEnum _defense1;
        public HeroEnum Defense1
        {
            get => _defense1;
            set
            {
                SetProperty(ref _defense1, value);
                ClearErrors(nameof(Defense1));
            }
        }

        private HeroEnum _defense2;
        public HeroEnum Defense2
        {
            get => _defense2;
            set
            {
                SetProperty(ref _defense2, value);
                ClearErrors(nameof(Defense2));
            }
        }

        private HeroEnum _defense3;
        public HeroEnum Defense3
        {
            get => _defense3;
            set
            {
                SetProperty(ref _defense3, value);
                ClearErrors(nameof(Defense3));
            }
        }

        private HeroEnum _defense4;
        public HeroEnum Defense4
        {
            get => _defense4;
            set
            {
                SetProperty(ref _defense4, value);
                ClearErrors(nameof(Defense4));
            }
        }

        private HeroEnum _defense5;
        public HeroEnum Defense5
        {
            get => _defense5;
            set
            {
                SetProperty(ref _defense5, value);
                ClearErrors(nameof(Defense5));
            }
        }

        private HeroEnum _defense6;
        public HeroEnum Defense6
        {
            get => _defense6;
            set
            {
                SetProperty(ref _defense6, value);
                ClearErrors(nameof(Defense6));
            }
        }

        private HeroEnum _defense7;
        public HeroEnum Defense7
        {
            get => _defense7;
            set
            {
                SetProperty(ref _defense7, value);
                ClearErrors(nameof(Defense7));
            }
        }

        private HeroEnum _defense8;
        public HeroEnum Defense8
        {
            get => _defense8;
            set
            {
                SetProperty(ref _defense8, value);
                ClearErrors(nameof(Defense8));
            }
        }

        private int _hidden1;
        public int Hidden1
        {
            get => _hidden1;
            set
            {
                SetProperty(ref _hidden1, value);
                ClearErrors(nameof(Hidden1));
            }
        }

        private int _hidden2;
        public int Hidden2
        {
            get => _hidden2;
            set
            {
                SetProperty(ref _hidden2, value);
                ClearErrors(nameof(Hidden2));
            }
        }

        private int _bp;
        public int Bp
        {
            get => _bp;
            set
            {
                SetProperty(ref _bp, value);
                ClearErrors(nameof(Bp));
            }
        }

        private StarsEnum _stars;
        public StarsEnum Stars
        {
            get => _stars;
            set
            {
                SetProperty(ref _stars, value);
                ClearErrors(nameof(Stars));
            }
        }

        private HeroEnum _pet1;
        public HeroEnum Pet1
        {
            get => _pet1;
            set
            {
                SetProperty(ref _pet1, value);
                ClearErrors(nameof(Pet1));
            }
        }

        private HeroEnum _pet2;
        public HeroEnum Pet2
        {
            get => _pet2;
            set
            {
                SetProperty(ref _pet2, value);
                ClearErrors(nameof(Pet2));
            }
        }
    }
}
