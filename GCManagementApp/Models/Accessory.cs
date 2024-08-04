using GCManagementApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    public class Accessory : NotifyPropertyChanged
    {
        private AccessoryTierEnum _accessoryTier;
        public AccessoryTierEnum AccessoryTier 
        {
            get => _accessoryTier;
            set => SetProperty(ref _accessoryTier, value);
        }

        private int _accessoryUpgradeLevel;
        public int AccessoryUpgradeLevel
        {
            get => _accessoryUpgradeLevel;
            set => SetProperty(ref _accessoryUpgradeLevel, value);
        }

        private AccessorySetEnum _accessorySet;
        public AccessorySetEnum AccessorySet
        {
            get => _accessorySet;
            set => SetProperty(ref _accessorySet, value);
        }

        public string AccessoryUpgradeDisplayLevel => AccessorySet == AccessorySetEnum.None ? "None" : AccessoryUpgradeLevel.ToString();
        public string AccessoryDisplayTier => AccessorySet == AccessorySetEnum.None ? "-" : AccessoryTier.ToString();
    }
}
