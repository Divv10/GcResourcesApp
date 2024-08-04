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
            return $"SI={SiCubes},CC={ChaserCubes} | AC={AssaultAC}A, {RangerAC}R, {TankAC}T, {HealerAC}H, {MageAC}M | CC= {AssaultCC}A, {RangerCC}R, {TankCC}T, {HealerCC}H, {MageCC}M | SE= {AssaultSE}A, {RangerSE}R, {TankSE}T, {HealerSE}H, {MageSE}M";
        }

        private int _chaserCubes;
        public int ChaserCubes
        {
            get => _chaserCubes;
            set => SetProperty(ref _chaserCubes, value);
        }

        private int _assaultCC;
        public int AssaultCC
        {
            get => _assaultCC;
            set
            {
                SetProperty(ref _assaultCC, value);
                OnPropertyChanged(nameof(ChaserCrystalsOwned));
            }
        }

        private int _mageCC;
        public int MageCC
        {
            get => _mageCC;
            set
            {
                SetProperty(ref _mageCC, value);
                OnPropertyChanged(nameof(ChaserCrystalsOwned));
            }
        }

        private int _rangerCC;
        public int RangerCC
        {
            get => _rangerCC;
            set
            {
                SetProperty(ref _rangerCC, value);
                OnPropertyChanged(nameof(ChaserCrystalsOwned));
            }
        }

        private int _healerCC;
        public int HealerCC
        {
            get => _healerCC;
            set
            {
                SetProperty(ref _healerCC, value);
                OnPropertyChanged(nameof(ChaserCrystalsOwned));
            }
        }

        private int _tankCC;
        public int TankCC
        {
            get => _tankCC;
            set
            {
                SetProperty(ref _tankCC, value);
                OnPropertyChanged(nameof(ChaserCrystalsOwned));
            }
        }

        private int _siCubes;
        public int SiCubes
        {
            get => _siCubes;
            set => SetProperty(ref _siCubes, value);
        }

        private int _assaultSE;
        public int AssaultSE
        {
            get => _assaultSE;
            set
            {
                SetProperty(ref _assaultSE, value);
                OnPropertyChanged(nameof(SoulEssencesOwned));
            }
        }

        private int _mageSE;
        public int MageSE
        {
            get => _mageSE;
            set
            {
                SetProperty(ref _mageSE, value);
                OnPropertyChanged(nameof(SoulEssencesOwned));
            }
        }

        private int _rangerSE;
        public int RangerSE
        {
            get => _rangerSE;
            set
            {
                SetProperty(ref _rangerSE, value);
                OnPropertyChanged(nameof(SoulEssencesOwned));
            }
        }

        private int _healerSE;
        public int HealerSE
        {
            get => _healerSE;
            set
            {
                SetProperty(ref _healerSE, value);
                OnPropertyChanged(nameof(SoulEssencesOwned));
            }
        }

        private int _tankSE;
        public int TankSE
        {
            get => _tankSE;
            set 
            {
                SetProperty(ref _tankSE, value); 
                OnPropertyChanged(nameof(SoulEssencesOwned));
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

        private int _healerAC;
        public int HealerAC
        {
            get => _healerAC;
            set => SetProperty(ref _healerAC, value);
        }

        private int _tankAC;
        public int TankAC
        {
            get => _tankAC;
            set => SetProperty(ref _tankAC, value);
        }

        private int _rangerAC;
        public int RangerAC
        {
            get => _rangerAC;
            set => SetProperty(ref _rangerAC, value);
        }

        private int _mageAC;
        public int MageAC
        {
            get => _mageAC;
            set => SetProperty(ref _mageAC, value);
        }

        private int _assaultAC;
        public int AssaultAC
        {
            get => _assaultAC;
            set => SetProperty(ref _assaultAC, value);
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

        private int _anniCoins;
        public int AnniCoins
        {
            get => _anniCoins;
            set 
            { 
                SetProperty(ref _anniCoins, value);
                OnPropertyChanged(nameof(AnniCoinsToolTip));
            }
        }

        public string ChaserCrystalsOwned => $"Assault: {AssaultCC} CC, Mage: {MageCC} CC, Ranger: {RangerCC} CC, Healer: {HealerCC} CC, Tank: {TankCC} CC";
        public string SoulEssencesOwned => $"Assault: {AssaultSE} SE, Mage: {MageSE} SE, Ranger: {RangerSE} SE, Healer: {HealerSE} SE, Tank: {TankSE} SE";
        public string BlueGemsToolTip => string.Format(Properties.Resources.EnoughBGToBuyCC, Math.Floor(BlueGems / 120d), Environment.NewLine, Math.Round((BlueGems / 120d) / 600, 2), Math.Floor(BlueGems / 60d), Math.Round((BlueGems / 60d) / 600, 2));
        public string AnniCoinsToolTip => string.Format(Properties.Resources.EnoughCoinsToBuyCC, Math.Floor(AnniCoins / 20d), Environment.NewLine, Math.Floor(AnniCoins / 10d));
        public int SiCubesFromBlueGems => (int)Math.Floor(BlueGems / 120d);
        public int SiCubesFromAnniCoins => (int)Math.Floor(AnniCoins / 20d);

        public int CraftableSoulEssences => Math.Max((new[] { AssaultCC, MageCC, RangerCC, HealerCC, TankCC }).Min(), 0);

        public int this[InventoryType type]
        {
            get
            {
                switch (type.heroClass)
                {
                    case HeroClass.Healer:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: return HealerAC;
                            case MaterialType.CC: return HealerCC;
                            case MaterialType.SE: return HealerSE;
                            default: return 0;
                        }
                    case HeroClass.Ranger:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: return RangerAC;
                            case MaterialType.CC: return RangerCC;
                            case MaterialType.SE: return RangerSE;
                            default: return 0;
                        }
                    case HeroClass.Assault:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: return AssaultAC;
                            case MaterialType.CC: return AssaultCC;
                            case MaterialType.SE: return AssaultSE;
                            default: return 0;
                        }
                    case HeroClass.Tank:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: return TankAC;
                            case MaterialType.CC: return TankCC;
                            case MaterialType.SE: return TankSE;
                            default: return 0;
                        }
                    default:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: return MageAC;
                            case MaterialType.CC: return MageCC;
                            case MaterialType.SE: return MageSE;
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
                            case MaterialType.AC: HealerAC = value; break;
                            case MaterialType.CC: HealerCC = value; break;
                            case MaterialType.SE: HealerSE = value; break;
                        }
                        return;
                    case HeroClass.Ranger:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: RangerAC = value; break;
                            case MaterialType.CC: RangerCC = value; break;
                            case MaterialType.SE: RangerSE = value; break;
                        }
                        return;
                    case HeroClass.Assault:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: AssaultAC = value; break;
                            case MaterialType.CC: AssaultCC = value; break;
                            case MaterialType.SE: AssaultSE = value; break;
                        }
                        return;
                    case HeroClass.Tank:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: TankAC = value; break;
                            case MaterialType.CC: TankCC = value; break;
                            case MaterialType.SE: TankSE = value; break;
                        }
                        return;
                    default:
                        switch (type.materialType)
                        {
                            case MaterialType.AC: MageAC = value; break;
                            case MaterialType.CC: MageCC = value; break;
                            case MaterialType.SE: MageSE = value; break;
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
        AC,
        SE,
        CC,
    }
}
