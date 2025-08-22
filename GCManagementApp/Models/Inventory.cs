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
            return $"GC={GrowthCubes} | GE= {AssaultGE}A, {RangerGE}R, {TankGE}T, {HealerGE}H, {MageGE}M";
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
                            default: return 0;
                        }
                    case HeroClass.Ranger:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return RangerGE;
                            default: return 0;
                        }
                    case HeroClass.Assault:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return AssaultGE;
                            default: return 0;
                        }
                    case HeroClass.Tank:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return TankGE;
                            default: return 0;
                        }
                    default:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: return MageGE;
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
                        }
                        return;
                    case HeroClass.Ranger:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: RangerGE = value; break;
                        }
                        return;
                    case HeroClass.Assault:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: AssaultGE = value; break;
                        }
                        return;
                    case HeroClass.Tank:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: TankGE = value; break;
                        }
                        return;
                    default:
                        switch (type.materialType)
                        {
                            case MaterialType.GE: MageGE = value; break;
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
        GE
    }
}
