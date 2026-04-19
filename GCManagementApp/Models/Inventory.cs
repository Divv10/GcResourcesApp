using GCManagementApp.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GCManagementApp.Models
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    [Serializable]
    public class Inventory : NotifyPropertyChanged
    {
        public override string ToString()
        {
            return $"DC={DivineCrystals}, GC={GrowthCubes} | GE= {AssaultGE}A, {RangerGE}R, {TankGE}T, {HealerGE}H, {MageGE}M | EW= {AssaultEWOrb}A, {RangerEWOrb}R, {TankEWOrb}T, {HealerEWOrb}H, {MageEWOrb}M";
        }

        private int _growthCubes;
        public int GrowthCubes
        {
            get => _growthCubes;
            set => SetProperty(ref _growthCubes, value);
        }

        private int _divineCrystals;
        public int DivineCrystals
        {
            get => _divineCrystals;
            set => SetProperty(ref _divineCrystals, value);
        }

        private int _assaultGE;
        public int AssaultGE
        {
            get => _assaultGE;
            set
            {
                SetProperty(ref _assaultGE, value);
                OnPropertyChanged(nameof(GrowthEssencesOwned));
            }
        }

        private int _mageGE;
        public int MageGE
        {
            get => _mageGE;
            set
            {
                SetProperty(ref _mageGE, value);
                OnPropertyChanged(nameof(GrowthEssencesOwned));
            }
        }

        private int _rangerGE;
        public int RangerGE
        {
            get => _rangerGE;
            set
            {
                SetProperty(ref _rangerGE, value);
                OnPropertyChanged(nameof(GrowthEssencesOwned));
            }
        }

        private int _healerGE;
        public int HealerGE
        {
            get => _healerGE;
            set
            {
                SetProperty(ref _healerGE, value);
                OnPropertyChanged(nameof(GrowthEssencesOwned));
            }
        }

        private int _tankGE;
        public int TankGE
        {
            get => _tankGE;
            set
            {
                SetProperty(ref _tankGE, value);
                OnPropertyChanged(nameof(GrowthEssencesOwned));
            }
        }

        private int _tankEWOrb;
        public int TankEWOrb
        {
            get => _tankEWOrb;
            set
            {
                SetProperty(ref _tankEWOrb, value);
                OnPropertyChanged(nameof(ExclusiveWeapOrbOwned));
            }
        }

        private int _assaultEWOrb;
        public int AssaultEWOrb
        {
            get => _assaultEWOrb;
            set
            {
                SetProperty(ref _assaultEWOrb, value);
                OnPropertyChanged(nameof(ExclusiveWeapOrbOwned));
            }
        }

        private int _mageEWOrb;
        public int MageEWOrb
        {
            get => _mageEWOrb;
            set
            {
                SetProperty(ref _mageEWOrb, value);
                OnPropertyChanged(nameof(ExclusiveWeapOrbOwned));
            }
        }

        private int _healerEWOrb;
        public int HealerEWOrb
        {
            get => _healerEWOrb;
            set
            {
                SetProperty(ref _healerEWOrb, value);
                OnPropertyChanged(nameof(ExclusiveWeapOrbOwned));
            }
        }
        private int _rangerEWOrb;
        public int RangerEWOrb
        {
            get => _rangerEWOrb;
            set
            {
                SetProperty(ref _rangerEWOrb, value);
                OnPropertyChanged(nameof(ExclusiveWeapOrbOwned));
            }
        }
        private int _blueGems;
        public int BlueGems
        {
            get => _blueGems;
            set 
            { 
                SetProperty(ref _blueGems, value);
                OnPropertyChanged(nameof(BlueGemsToolTip));
            }
        }

        private long _gold;
        public long Gold
        {
            get => _gold;
            set => SetProperty(ref _gold, value);
        }

        private string _goldFormatted;
        [XmlIgnore]
        [JsonIgnore]
        public string GoldFormatted
        {
            get 
            { 
                return GoldToShortNumber(Gold); 
            }
            set
            {
                _goldFormatted = value;
                Gold = ShortNumberToGold(value);
            }
        }

        private int _ewMats;
        public int EwMats
        {
            get => _ewMats;
            set => SetProperty(ref _ewMats, value);
        }

        public string GrowthEssencesOwned => $"Assault: {AssaultGE} GE, Mage: {MageGE} GE, Ranger: {RangerGE} GE, Healer: {HealerGE} GE, Tank: {TankGE} GE";
        public string ExclusiveWeapOrbOwned => $"Assault: {AssaultEWOrb} EW Orb, Healer: {HealerEWOrb} EW Orb, Mage: {MageEWOrb} EW Orb, Ranger: {RangerEWOrb} EW Orb, Tank: {TankEWOrb} EW Orb";
        public string BlueGemsToolTip => string.Format(Properties.Resources.EnoughBGToBuyGC, Math.Floor(BlueGems / 120d), Environment.NewLine, Math.Round((BlueGems / 120d) / 1200, 2));
        public int GrowthCubesFromBlueGems => (int)Math.Floor(BlueGems / 120d);

        public int this[InventoryType type]
        {
            get
            {
                switch (type.heroClass)
                {
                    case HeroClass.Healer:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return HealerGE;
                            case MaterialType.EW: return HealerEWOrb;
                            default: return 0;
                        }
                    case HeroClass.Ranger:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return RangerGE;
                            case MaterialType.EW: return RangerEWOrb;
                            default: return 0;
                        }
                    case HeroClass.Assault:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return AssaultGE;
                            case MaterialType.EW: return AssaultEWOrb;
                            default: return 0;
                        }
                    case HeroClass.Tank:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return TankGE;
                            case MaterialType.EW: return TankEWOrb;
                            default: return 0;
                        }
                    default:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return MageGE;
                            case MaterialType.EW: return MageEWOrb;
                            default: return 0;
                        }
                }
            }
            set
            {
                switch (type.heroClass)
                {
                    case HeroClass.Healer:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: HealerGE = value; break;
                            case MaterialType.EW: HealerEWOrb = value; break;
                        }
                        return;
                    case HeroClass.Ranger:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: RangerGE = value; break;
                            case MaterialType.EW: RangerEWOrb = value; break;
                        }
                        return;
                    case HeroClass.Assault:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: AssaultGE = value; break;
                            case MaterialType.EW: AssaultEWOrb = value; break;
                        }
                        return;
                    case HeroClass.Tank:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: TankGE = value; break;
                            case MaterialType.EW: TankEWOrb = value; break;
                        }
                        return;
                    default:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: MageGE = value; break;
                            case MaterialType.EW: MageEWOrb = value; break;
                        }
                        return;
                }
            }
        }

        public static string GoldToShortNumber(long gold)
        { 
            if (gold > 99000000000)
                return "+99B";
            if (gold >= 1000000000)
                return $"{gold / 1000000000d}B";
            if (gold >= 1000000)
                return $"{gold / 1000000d}M";
            
            return gold.ToString();
        }

        public static long ShortNumberToGold(string _gold)
        {
            if (long.TryParse(_gold, out long gold))
            {
                return gold;
            }

            if (!string.IsNullOrWhiteSpace(_gold))
            {
                var goldS = _gold.Trim().Replace(" ", "")
                    .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                    .ToLower();

                if (goldS.Last() == 'b')
                {
                    goldS = goldS.Remove(goldS.Length - 1);
                    if (double.TryParse(goldS, out double goldB))
                        return Convert.ToInt64(goldB * 1000000000);
                }
                if (goldS.Last() == 'm')
                {
                    goldS = goldS.Remove(goldS.Length - 1);
                    if (double.TryParse(goldS, out double goldM))
                        return Convert.ToInt64(goldM * 1000000);
                }
            }

            return 0;
        }
    }

    public record InventoryType
    {
        public HeroClass heroClass { get; set; }
        public MaterialType materialType { get; set; }
    }

    public enum MaterialType
    {
        GE,
        EW
    }
}
