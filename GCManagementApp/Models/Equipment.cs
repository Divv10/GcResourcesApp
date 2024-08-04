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
    [Serializable]
    public class Equipment : NotifyPropertyChanged
    {
        private WeaponPiece _weapon = default!;
        public WeaponPiece Weapon
        {
            get => _weapon;
            set => SetProperty(ref _weapon, value);
        }

        private GearPiece _supportWeapon = default!;
        public GearPiece SupportWeapon
        {
            get => _supportWeapon;
            set => SetProperty(ref _supportWeapon, value);
        }

        private ArmorPiece _armor = default!;
        public ArmorPiece Armor
        {
            get => _armor;
            set => SetProperty(ref _armor, value);
        }

        private ArmorPiece _supportArmorFirst = default!;
        public ArmorPiece SupportArmorFirst
        {
            get => _supportArmorFirst;
            set => SetProperty(ref _supportArmorFirst, value);
        }

        private ArmorPiece _supportArmorSecond = default!;
        public ArmorPiece SupportArmorSecond
        {
            get => _supportArmorSecond;
            set => SetProperty(ref _supportArmorSecond, value);
        }

        private bool _isExclusiveWeaponOwned = default!;
        public bool IsExclusiveWeaponOwned
        {
            get => _isExclusiveWeaponOwned;
            set => SetProperty(ref _isExclusiveWeaponOwned, value);
        }

        private int _exclusiveWeaponUpgrade = default!;
        public int ExclusiveWeaponUpgrade
        {
            get => _exclusiveWeaponUpgrade;
            set => SetProperty(ref _exclusiveWeaponUpgrade, value);
        }

        private ArtifactTier _artifactTier;
        public ArtifactTier ArtifactTier
        {
            get => _artifactTier;
            set => SetProperty(ref _artifactTier, value);
        }

        private ArtifactType _artifactType;
        public ArtifactType ArtifactType
        {
            get => _artifactType;
            set => SetProperty(ref _artifactType, value);
        }

        private int _artifactUpgrade = default!;
        public int ArtifactUpgrade
        {
            get => _artifactUpgrade;
            set => SetProperty(ref _artifactUpgrade, value);
        }

        [XmlIgnore]
        [JsonIgnore]
        public string SetSummary
        {
            get
            {
                if (Weapon.Set == Armor.Set && Armor.Set == SupportArmorFirst.Set && SupportArmorFirst.Set == SupportArmorSecond.Set)
                {
                    return Weapon.Set.GetDescription();
                }
                if (Weapon.Set == GearSet.None || Armor.Set == GearSet.None || SupportArmorFirst.Set == GearSet.None || SupportArmorSecond.Set == GearSet.None)
                {
                    return "Mixed (Missing parts)";
                }
                return "Mixed";
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public string WeaponTranscendenceSummary
        {
            get => $"Weapon T{Weapon.Transcendence}";
        }

        [XmlIgnore]
        [JsonIgnore]
        public string TierSummary
        {
            get
            {
                var equip = new[] { Weapon.Tier, SupportWeapon.Tier, Armor.Tier, SupportArmorFirst.Tier, SupportArmorSecond.Tier };
                var min = (GearTier)equip.Min(x => (int)x);
                var max = (GearTier)equip.Max(x => (int)x);
                if (min != max)
                {
                    return $"{min.GetDescription()} - {max.GetDescription()}";
                }
                return min.GetDescription();
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public string EwSummary => IsExclusiveWeaponOwned ? $"+{ExclusiveWeaponUpgrade}" : "None";

        [XmlIgnore]
        [JsonIgnore]
        public string ArtifactSummary => ArtifactTier == ArtifactTier.None ? ArtifactTier.None.GetDescription() : $"{ArtifactTier.GetDescription().Split(new char[] {' '}).First()} {ArtifactType} +{ArtifactUpgrade}";

        [XmlIgnore]
        [JsonIgnore]
        public string ArtifactShortSummary => ArtifactTier == ArtifactTier.None ? ArtifactTier.None.GetDescription() : $"{ArtifactTier.GetDescription().Split(new char[] { ' ' }).First()} +{ArtifactUpgrade}";

        [XmlIgnore]
        [JsonIgnore]
        public int EquipmentTierSum => (int)Weapon.Tier + (int)SupportWeapon.Tier + (int)Armor.Tier + (int)SupportArmorFirst.Tier + (int)SupportArmorSecond.Tier;

        public Equipment()
        {
            Weapon = new WeaponPiece();
            SupportWeapon = new GearPiece();
            Armor = new ArmorPiece();
            SupportArmorFirst = new ArmorPiece();
            SupportArmorSecond = new ArmorPiece();            
        }

        public void UpdateSummaries()
        {
            OnPropertyChanged(nameof(SetSummary));
            OnPropertyChanged(nameof(WeaponTranscendenceSummary));
            OnPropertyChanged(nameof(TierSummary));
            OnPropertyChanged(nameof(EwSummary));
            OnPropertyChanged(nameof(ArtifactSummary));
            OnPropertyChanged(nameof(ArtifactShortSummary));
        }
    }
}
