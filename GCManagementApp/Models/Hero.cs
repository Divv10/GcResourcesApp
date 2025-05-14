using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GCManagementApp.Models
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class Hero : NotifyPropertyChanged
    {
        public override string ToString()
        {
            return $"{Properties.Resources.ResourceManager.GetObject(HeroName.GetDescription()) ?? HeroName}({HeroType})";
        }
        private HeroEnum _heroName;
        public HeroEnum HeroName
        {
            get => _heroName;
            set => SetProperty(ref _heroName, value);
        }

        private string _customHeroName;
        [XmlIgnore]
        [JsonIgnore]
        public string CustomHeroName
        {
            get => _customHeroName;
            set => SetProperty(ref _customHeroName, value);
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

        public Hero(HeroEnum heroName, HeroType heroType, HeroClass heroClass, HeroAttribute heroAttribute)
        {
            _heroName = heroName;
            _heroType = heroType;
            _heroClass = heroClass;
            _heroAttribute = heroAttribute;
        }

        public static List<Hero> GetHeroesCollection { get; } = new List<Hero>()
        {
            new Hero(HeroEnum.Elesis, HeroType.SR, HeroClass.Assault, HeroAttribute.Green),
            new Hero(HeroEnum.Mari, HeroType.SR, HeroClass.Mage, HeroAttribute.Green),
            new Hero(HeroEnum.Lapis, HeroType.SR, HeroClass.Ranger, HeroAttribute.Green),
            new Hero(HeroEnum.Ronan, HeroType.SR, HeroClass.Tank, HeroAttribute.Red),
            new Hero(HeroEnum.Serdin, HeroType.SR, HeroClass.Healer, HeroAttribute.Red),
            new Hero(HeroEnum.Lire, HeroType.SR, HeroClass.Ranger, HeroAttribute.Green),
            new Hero(HeroEnum.Harpe, HeroType.SR, HeroClass.Mage, HeroAttribute.Yellow),
            new Hero(HeroEnum.Lass, HeroType.SR, HeroClass.Assault, HeroAttribute.Blue),
            new Hero(HeroEnum.Jin, HeroType.SR, HeroClass.Tank, HeroAttribute.Blue),
            new Hero(HeroEnum.Rin, HeroType.SR, HeroClass.Healer, HeroAttribute.Yellow),
            new Hero(HeroEnum.Rufus, HeroType.SR, HeroClass.Ranger, HeroAttribute.Blue),
            new Hero(HeroEnum.Callisto, HeroType.SR, HeroClass.Assault, HeroAttribute.Green),
            new Hero(HeroEnum.Arme, HeroType.SR, HeroClass.Mage, HeroAttribute.Red),
            new Hero(HeroEnum.Zero, HeroType.SR, HeroClass.Tank, HeroAttribute.Purple),
            new Hero(HeroEnum.Io, HeroType.SR, HeroClass.Ranger, HeroAttribute.Red),
            new Hero(HeroEnum.Cindy, HeroType.SR, HeroClass.Healer, HeroAttribute.Red),
            new Hero(HeroEnum.Sieghart, HeroType.SR, HeroClass.Assault, HeroAttribute.Yellow),
            new Hero(HeroEnum.Asin, HeroType.SR, HeroClass.Tank, HeroAttribute.Green),
            new Hero(HeroEnum.Ley, HeroType.SR, HeroClass.Mage, HeroAttribute.Green),
            new Hero(HeroEnum.Kanavan, HeroType.SR, HeroClass.Assault, HeroAttribute.Red),
            new Hero(HeroEnum.Lime, HeroType.SR, HeroClass.Healer, HeroAttribute.Yellow),
            new Hero(HeroEnum.Ragnar, HeroType.SR, HeroClass.Ranger, HeroAttribute.Yellow),
            new Hero(HeroEnum.Tia, HeroType.SR, HeroClass.Tank, HeroAttribute.Green),
            new Hero(HeroEnum.Veigas, HeroType.SR, HeroClass.Mage, HeroAttribute.Blue),
            new Hero(HeroEnum.Hwarin, HeroType.SR, HeroClass.Healer, HeroAttribute.Purple),
            new Hero(HeroEnum.Europa, HeroType.SR, HeroClass.Assault, HeroAttribute.Yellow),
            new Hero(HeroEnum.Werner, HeroType.SR, HeroClass.Ranger, HeroAttribute.Green),
            new Hero(HeroEnum.Myst, HeroType.SR, HeroClass.Tank, HeroAttribute.Yellow),
            new Hero(HeroEnum.Amy, HeroType.SR, HeroClass.Healer, HeroAttribute.Blue),
            new Hero(HeroEnum.Edel, HeroType.SR, HeroClass.Assault, HeroAttribute.Red),
            new Hero(HeroEnum.Nelia, HeroType.SR, HeroClass.Mage, HeroAttribute.Purple),
            new Hero(HeroEnum.Grandiel, HeroType.SR, HeroClass.Healer, HeroAttribute.Green),
            new Hero(HeroEnum.Ryan, HeroType.SR, HeroClass.Tank, HeroAttribute.Blue),
            new Hero(HeroEnum.Dio, HeroType.SR, HeroClass.Assault, HeroAttribute.Purple),
            new Hero(HeroEnum.Ganymede, HeroType.SR, HeroClass.Mage, HeroAttribute.Red),
            new Hero(HeroEnum.Decanee, HeroType.SR, HeroClass.Ranger, HeroAttribute.Purple),
            new Hero(HeroEnum.Elesis, HeroType.T, HeroClass.Tank, HeroAttribute.Red),
            new Hero(HeroEnum.Arme, HeroType.T, HeroClass.Healer, HeroAttribute.Purple),
            new Hero(HeroEnum.Lire, HeroType.T, HeroClass.Assault, HeroAttribute.Blue),
            new Hero(HeroEnum.Ronan, HeroType.T, HeroClass.Mage, HeroAttribute.Blue),
            new Hero(HeroEnum.Mari, HeroType.T, HeroClass.Ranger, HeroAttribute.Red),
            new Hero(HeroEnum.Amy, HeroType.T, HeroClass.Assault, HeroAttribute.Green),
            new Hero(HeroEnum.Lime, HeroType.T, HeroClass.Tank, HeroAttribute.Blue),
            new Hero(HeroEnum.Edel, HeroType.T, HeroClass.Ranger, HeroAttribute.Blue),
            new Hero(HeroEnum.Jin, HeroType.T, HeroClass.Assault, HeroAttribute.Red),
            new Hero(HeroEnum.Ley, HeroType.T, HeroClass.Healer, HeroAttribute.Green),
            new Hero(HeroEnum.Harpe, HeroType.T, HeroClass.Tank, HeroAttribute.Purple),
            new Hero(HeroEnum.Serdin, HeroType.T, HeroClass.Mage, HeroAttribute.Green),
            new Hero(HeroEnum.Sieghart, HeroType.T, HeroClass.Tank, HeroAttribute.Green),
            new Hero(HeroEnum.Io, HeroType.T, HeroClass.Healer, HeroAttribute.Blue),
            new Hero(HeroEnum.Sol, HeroType.SR, HeroClass.Assault, HeroAttribute.Red),
            new Hero(HeroEnum.Lass, HeroType.T, HeroClass.Ranger, HeroAttribute.Blue),
            new Hero(HeroEnum.Lapis, HeroType.T, HeroClass.Assault, HeroAttribute.Blue),
            new Hero(HeroEnum.Kanavan, HeroType.T, HeroClass.Healer, HeroAttribute.Red),
            new Hero(HeroEnum.Ai, HeroType.SR, HeroClass.Mage, HeroAttribute.Yellow),
            new Hero(HeroEnum.Europa, HeroType.T, HeroClass.Ranger, HeroAttribute.Red),
            new Hero(HeroEnum.Werner, HeroType.T, HeroClass.Assault, HeroAttribute.Green),
            new Hero(HeroEnum.Hwarin, HeroType.T, HeroClass.Mage, HeroAttribute.Blue),
            new Hero(HeroEnum.Mayden, HeroType.SR, HeroClass.Ranger, HeroAttribute.Green),
            new Hero(HeroEnum.Ragnar, HeroType.T, HeroClass.Healer, HeroAttribute.Yellow),
            new Hero(HeroEnum.Rufus, HeroType.T, HeroClass.Mage, HeroAttribute.Red),
            new Hero(HeroEnum.Elesis, HeroType.S, HeroClass.Assault, HeroAttribute.Red),
            new Hero(HeroEnum.Bastet, HeroType.SR, HeroClass.Ranger, HeroAttribute.Purple),
            new Hero(HeroEnum.Callisto, HeroType.T, HeroClass.Tank, HeroAttribute.Red),
            new Hero(HeroEnum.Zero, HeroType.T, HeroClass.Healer, HeroAttribute.Blue),
            new Hero(HeroEnum.Brammashell, HeroType.SR, HeroClass.Assault, HeroAttribute.Green),
            new Hero(HeroEnum.Rin, HeroType.T, HeroClass.Mage, HeroAttribute.Green),
            new Hero(HeroEnum.Veigas, HeroType.T, HeroClass.Ranger, HeroAttribute.Red),
            new Hero(HeroEnum.Nelia, HeroType.T, HeroClass.Healer, HeroAttribute.Red),
            new Hero(HeroEnum.Asin, HeroType.T, HeroClass.Assault, HeroAttribute.Purple),
            new Hero(HeroEnum.Arme, HeroType.S, HeroClass.Mage, HeroAttribute.Blue),
            new Hero(HeroEnum.Deia, HeroType.SR, HeroClass.Ranger, HeroAttribute.Yellow),
            new Hero(HeroEnum.Tia, HeroType.T, HeroClass.Assault, HeroAttribute.Red),
            new Hero(HeroEnum.Ryan, HeroType.T, HeroClass.Mage, HeroAttribute.Red),
            new Hero(HeroEnum.Urara, HeroType.SR, HeroClass.Healer, HeroAttribute.Green),
            new Hero(HeroEnum.Ganymede, HeroType.T, HeroClass.Ranger, HeroAttribute.Blue),
            new Hero(HeroEnum.Dio, HeroType.T, HeroClass.Tank, HeroAttribute.Yellow),
            new Hero(HeroEnum.Decanee, HeroType.T, HeroClass.Assault, HeroAttribute.Blue),
            new Hero(HeroEnum.Myst, HeroType.T, HeroClass.Mage, HeroAttribute.Green),
            new Hero(HeroEnum.Uno, HeroType.S, HeroClass.Ranger, HeroAttribute.Red),
            new Hero(HeroEnum.Vice, HeroType.SR, HeroClass.Mage, HeroAttribute.Blue),
            new Hero(HeroEnum.Ai, HeroType.T, HeroClass.Tank, HeroAttribute.Green),
            new Hero(HeroEnum.Mayden, HeroType.T, HeroClass.Healer, HeroAttribute.Blue),
            new Hero(HeroEnum.Lire, HeroType.S, HeroClass.Ranger, HeroAttribute.Green),
            new Hero(HeroEnum.Nepteon, HeroType.SR, HeroClass.Assault, HeroAttribute.Red),
            new Hero(HeroEnum.Cindy, HeroType.T, HeroClass.Mage, HeroAttribute.Red),
        };

        public string DisplayName =>  $"{Properties.Resources.ResourceManager.GetObject(HeroName.GetDescription()) ?? HeroName}{(HeroType == Enums.HeroType.T ? "(T)" : HeroType == HeroType.S ? "(S)" : "")}";

        public string ImageName => $"{HeroName}{HeroType}";
    }
}
