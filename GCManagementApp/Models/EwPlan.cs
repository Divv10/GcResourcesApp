using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using GCManagementApp.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GCManagementApp.Models
{
    [Serializable]
    public class EwPlan : NotifyPropertyChanged
    {
        private HeroEnum _heroName;
        public HeroEnum HeroName
        {
            get => _heroName;
            set => SetProperty(ref _heroName, value);
        }

        private HeroType _heroType;
        public HeroType HeroType
        {
            get => _heroType;
            set => SetProperty(ref _heroType, value);
        }

        public override string ToString()
        {
            return $"{Properties.Resources.ResourceManager.GetObject(HeroName.GetDescription()) ?? HeroName}({HeroType})";
        }

        private Hero _hero;
        [XmlIgnore]
        [JsonIgnore]
        public Hero Hero
        {
            get => _hero;
            set => SetProperty(ref _hero, value);
        }

        private bool _isEwOwned;
        public bool IsEwOwned
        {
            get => _isEwOwned;
            set => SetProperty(ref _isEwOwned, value);
        }

        private int _currentLevel;
        public int CurrentLevel
        {
            get => _currentLevel;
            set => SetProperty(ref _currentLevel, value);
        }

        private int _desiredLevel;
        public int DesiredLevel
        {
            get => _desiredLevel;
            set => SetProperty(ref _desiredLevel, value);
        }

        private int _currentPartialLevel;
        public int CurrentPartialLevel
        {
            get => _currentPartialLevel;
            set => SetProperty(ref _currentPartialLevel, value);
        }

        private ExclusiveWeaponCost _weaponCost;
        [XmlIgnore]
        [JsonIgnore]
        public ExclusiveWeaponCost WeaponCost
        {
            get => _weaponCost;
            set => SetProperty(ref _weaponCost, value);
        }

        private double _daysForEW;
        [XmlIgnore]
        [JsonIgnore]
        public double DaysForEW
        {
            get => _daysForEW;
            set => SetProperty(ref _daysForEW, value);
        }

        private int _ewMatsNeeded;
        [XmlIgnore]
        [JsonIgnore]
        public int EwMatsNeeded
        {
            get => _ewMatsNeeded;
            set => SetProperty(ref _ewMatsNeeded, value);
        }

        [JsonIgnore]
        public string DisplayName => $"{Properties.Resources.ResourceManager.GetObject(Hero.HeroName.GetDescription()) ?? Hero.HeroName}{(Hero.HeroType == Enums.HeroType.T ? "(T)" : Hero.HeroType == HeroType.S ? "(S)" : "")}";

        [JsonIgnore]
        public string ImageName => $"{Hero.HeroName}{Hero.HeroType}";
    }
}
