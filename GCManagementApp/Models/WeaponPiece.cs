using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class WeaponPiece : ArmorPiece
    {
        private int _transcendence;
        public int Transcendence
        {
            get => _transcendence;
            set => SetProperty(ref _transcendence, value);
        }

        public WeaponPiece() : base()
        {
            Transcendence = 0;
        }
    }
}
