using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class SelectableHero : NotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private Hero _hero;
        public Hero Hero
        {
            get => _hero;
            set => SetProperty(ref _hero, value);
        }
    }
}
