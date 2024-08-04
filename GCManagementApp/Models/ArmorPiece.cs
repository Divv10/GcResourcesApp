using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class ArmorPiece : GearPiece
    {
        private GearSet _set;
        public GearSet Set
        {
            get => _set;
            set => SetProperty(ref _set, value);
        }

        public ArmorPiece() : base()
        {
            Set = GearSet.None;
        }
    }
}
