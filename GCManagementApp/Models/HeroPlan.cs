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
    public class HeroPlan : NotifyPropertyChanged
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

        private HeroClass _heroClass;
        public HeroClass HeroClass
        {
            get => _heroClass;
            set => SetProperty(ref _heroClass, value);
        }

        private HeroAttribute _heroAttribute;
        public HeroAttribute HeroAttribute
        {
            get => _heroAttribute;
            set => SetProperty(ref _heroAttribute, value);
        }

        private string _unknownHeroName;
        public string UnknownHeroName
        {
            get => _unknownHeroName;
            set => SetProperty(ref _unknownHeroName, value);
        }

        public override string ToString()
        {
            return HeroName == HeroEnum.Custom ? $"{UnknownHeroName}({HeroType})" : $"{HeroName}({HeroType})";
        }

        private Hero _hero;
        [XmlIgnore]
        [JsonIgnore]
        public Hero Hero
        {
            get => _hero;
            set => SetProperty(ref _hero, value);
        }

        private GrowthPlan _currentGrowth;
        [XmlIgnore]
        [JsonIgnore]
        public GrowthPlan CurrentGrowth
        {
            get => _currentGrowth;
            set => SetProperty(ref _currentGrowth, value);
        }

        private GrowthPlan _desiredGrowth;
        public GrowthPlan DesiredGrowth
        {
            get => _desiredGrowth;
            set => SetProperty(ref _desiredGrowth, value);
        }

        private SiLevelCost _siLevelCost;
        [XmlIgnore]
        [JsonIgnore]
        public SiLevelCost SiLevelCost
        {
            get => _siLevelCost;
            set => SetProperty(ref _siLevelCost, value);
        }

        private ChaserLevelCost _clLevelCost;
        [XmlIgnore]
        [JsonIgnore]
        public ChaserLevelCost ClLevelCost
        {
            get => _clLevelCost;
            set => SetProperty(ref _clLevelCost, value);
        }

        private TranscendenceCost _tCost;
        [XmlIgnore]
        [JsonIgnore]
        public TranscendenceCost TCost
        {
            get => _tCost;
            set => SetProperty(ref _tCost, value);
        }

        private DescendCosts _dCost;
        [XmlIgnore]
        [JsonIgnore]
        public DescendCosts DCost
        {
            get => _dCost;
            set => SetProperty(ref _dCost, value);
        }

        private double _daysForGE;
        [XmlIgnore]
        [JsonIgnore]
        public double DaysForGE
        {
            get => _daysForGE;
            set => SetProperty(ref _daysForGE, value);
        }

        private double _daysForSi;
        [XmlIgnore]
        [JsonIgnore]
        public double DaysForSi
        {
            get => _daysForSi;
            set => SetProperty(ref _daysForSi, value);
        }

        private double _daysForDe;
        [XmlIgnore]
        [JsonIgnore]
        public double DaysForDe
        {
            get => _daysForDe;
            set => SetProperty(ref _daysForDe, value);
        }

        private int _geNeeded;
        [XmlIgnore]
        [JsonIgnore]
        public int GeNeeded
        {
            get => _geNeeded;
            set => SetProperty(ref _geNeeded, value);
        }

        private int _siNeeded;
        [XmlIgnore]
        [JsonIgnore]
        public int SiNeeded
        {
            get => _siNeeded;
            set => SetProperty(ref _siNeeded, value);
        }

        private int _divineCrystalsNeeded;
        [XmlIgnore]
        [JsonIgnore]
        public int DivineCrystalsNeeded
        {
            get => _divineCrystalsNeeded;
            set => SetProperty(ref _divineCrystalsNeeded, value);
        }

        private int _goldCost;
        [XmlIgnore]
        [JsonIgnore]
        public int GoldCost
        {
            get => _goldCost;
            set => SetProperty(ref _goldCost, value);
        }

        private long _goldNeeded;
        [XmlIgnore]
        [JsonIgnore]
        public long GoldNeeded
        {
            get => _goldNeeded;
            set => SetProperty(ref _goldNeeded, value);
        }

        private double _daysForGold;
        [XmlIgnore]
        [JsonIgnore]
        public double DaysForGold
        {
            get => _daysForGold;
            set => SetProperty(ref _daysForGold, value);
        }

        private bool _baseHeroNeedToBeBuilt;
        [XmlIgnore]
        [JsonIgnore]
        public bool BaseHeroNeedToBeBuilt
        {
            get => _baseHeroNeedToBeBuilt;
            set => SetProperty(ref _baseHeroNeedToBeBuilt, value);
        }

        [XmlIgnore]
        [JsonIgnore]
        public bool CoreOpenVisible => DesiredGrowth?.SiLevel >= CurrentGrowth?.SiLevel && (DesiredGrowth?.SiLevel == 5 || DesiredGrowth?.SiLevel == 10) && DesiredGrowth?.IsCoreOpen == true;

        [JsonIgnore]
        public string DisplayName => Hero == null ? string.Empty : $"{Properties.Resources.ResourceManager.GetObject(Hero.HeroName.GetDescription()) ?? (HeroName == HeroEnum.Custom ? UnknownHeroName : Hero.HeroName)}{(Hero.HeroType == Enums.HeroType.T ? "(T)" : Hero.HeroType == HeroType.S ? "(S)" : "")}";

        [JsonIgnore]
        public string ImageName => $"{Hero.HeroName}{Hero.HeroType}";
    }
}
