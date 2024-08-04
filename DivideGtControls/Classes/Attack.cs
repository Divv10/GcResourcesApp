using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideGT
{
    public class Attack : NotifyPropertyChanged
    {
        private HeroEnum _attack1;
        public HeroEnum Attack1
        {
            get => _attack1;
            set
            {
                SetProperty(ref _attack1, value);
                ClearErrors(nameof(Attack1));
            }
        }

        private HeroEnum _attack2;
        public HeroEnum Attack2
        {
            get => _attack2;
            set
            {
                SetProperty(ref _attack2, value);
                ClearErrors(nameof(Attack2));
            }
        }

        private HeroEnum _attack3;
        public HeroEnum Attack3
        {
            get => _attack3;
            set
            {
                SetProperty(ref _attack3, value);
                ClearErrors(nameof(Attack3));
            }
        }

        private HeroEnum _attack4;
        public HeroEnum Attack4
        {
            get => _attack4;
            set
            {
                SetProperty(ref _attack4, value);
                ClearErrors(nameof(Attack4));
            }
        }

        private HeroEnum _pet;
        public HeroEnum Pet
        {
            get => _pet;
            set
            {
                SetProperty(ref _pet, value);
                ClearErrors(nameof(Pet));
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
    }
}
