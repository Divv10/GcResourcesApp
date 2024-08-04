using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class ClassGearSlots : NotifyPropertyChanged
    {
        private GearSlot _weapon;
        public GearSlot Weapon
        {
            get => _weapon;
            set => SetProperty(ref _weapon, value);
        }

        private GearSlot _secondaryWeapon;
        public GearSlot SecondaryWeapon
        {
            get => _secondaryWeapon;
            set => SetProperty(ref _secondaryWeapon, value);
        }

        private GearSlot _armor;
        public GearSlot Armor
        {
            get => _armor;
            set => SetProperty(ref _armor, value);
        }

        private GearSlot _secondaryArmorOne;
        public GearSlot SecondaryArmorOne
        {
            get => _secondaryArmorOne;
            set => SetProperty(ref _secondaryArmorOne, value);
        }

        private GearSlot _secondaryArmorTwo;
        public GearSlot SecondaryArmorTwo
        {
            get => _secondaryArmorTwo;
            set => SetProperty(ref _secondaryArmorTwo, value);
        }

        public ClassGearSlots()
        {
            Weapon = new GearSlot();
            SecondaryWeapon = new GearSlot();
            Armor = new GearSlot();
            SecondaryArmorOne = new GearSlot();
            SecondaryArmorTwo = new GearSlot();

            Weapon.PropertyChanged += (s, e) => { OnPropertyChanged("GearSlot"); };
            SecondaryWeapon.PropertyChanged += (s, e) => { OnPropertyChanged("GearSlot"); };
            Armor.PropertyChanged += (s, e) => { OnPropertyChanged("GearSlot"); };
            SecondaryArmorOne.PropertyChanged += (s, e) => { OnPropertyChanged("GearSlot"); };
            SecondaryArmorTwo.PropertyChanged += (s, e) => { OnPropertyChanged("GearSlot"); };
        }
    }
}
