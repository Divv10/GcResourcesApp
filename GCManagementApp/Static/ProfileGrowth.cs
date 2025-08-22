using GCManagementApp.Helpers;
using GCManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    internal static class ProfileGrowth
    {
        public static Profile Profile { get; set; }
        public static List<HeroGrowth> Heroes { get; set; }

        public static event EventHandler GearSlotChangedEvent = delegate { };

        public static void Initialize()
        {
            //Task.Run(() => Profile.CheckJsonFile()).Wait();
            //Profile = Profile.LoadFromJson();
            Task.Run(() => ProfileManager.Initialize()).Wait();
            InitializeImpl();
        }

        public static void InitializeAfterDownload()
        {
            InitializeImpl();
        }

        private static void InitializeImpl()
        {
            var profileHeroes = Profile.HeroesOwned;
            var allHeroes = Hero.GetHeroesCollection;
            var currentHeroes = new List<HeroGrowth>();
            foreach (var hero in allHeroes)
            {
                var heroGrowth = new HeroGrowth();
                heroGrowth.Hero = hero;
                if (profileHeroes != null && profileHeroes.FirstOrDefault(h => h.HeroName == hero.HeroName && h.HeroType == hero.HeroType) is ProfileHeroGrowth profileHero)
                {
                    heroGrowth.Level = profileHero.Level;
                    heroGrowth.ChaserLevel = profileHero.ChaserLevel;
                    heroGrowth.DescentLevel = profileHero.DescentLevel;
                    heroGrowth.PetLevel = profileHero.PetLevel;
                    heroGrowth.SiLevel = profileHero.SiLevel;
                    heroGrowth.TranscendenceLevel = profileHero.TranscendenceLevel;
                    heroGrowth.IsOwned = profileHero.IsOwned;
                    heroGrowth.IsCoreOpen = profileHero.IsCoreOpen;
                    heroGrowth.TraitsOpen = profileHero.TraitsOpen;
                    heroGrowth.BP = profileHero.BP;
                    heroGrowth.TransPercentage = profileHero.TransPercentage;
                    if (heroGrowth.TraitsOpen < AvailableSiTraits.MinimumTraits(heroGrowth.SiLevel))
                    {
                        heroGrowth.TraitsOpen = (int)AvailableSiTraits.MinimumTraits(heroGrowth.SiLevel);
                    }
                    if (heroGrowth.TraitsOpen > AvailableSiTraits.MaximumTraits(heroGrowth.SiLevel, heroGrowth.IsCoreOpen))
                    {
                        heroGrowth.TraitsOpen = (int)AvailableSiTraits.MaximumTraits(heroGrowth.SiLevel, heroGrowth.IsCoreOpen);
                    }
                    heroGrowth.Ring = new Accessory() 
                    { 
                        AccessoryTier = (Enums.AccessoryTierEnum)profileHero.RingTier, 
                        AccessoryUpgradeLevel= profileHero.RingUpgradeLevel,
                        AccessorySet = (Enums.AccessorySetEnum)profileHero.RingSet
                    };
                    heroGrowth.Necklace = new Accessory()
                    {
                        AccessoryTier = (Enums.AccessoryTierEnum)profileHero.NecklaceTier,
                        AccessoryUpgradeLevel = profileHero.NecklaceUpgradeLevel,
                        AccessorySet = (Enums.AccessorySetEnum)profileHero.NecklaceSet
                    };
                    heroGrowth.Earrings = new Accessory()
                    {
                        AccessoryTier = (Enums.AccessoryTierEnum)profileHero.EarringsTier,
                        AccessoryUpgradeLevel = profileHero.EarringsUpgradeLevel,
                        AccessorySet = (Enums.AccessorySetEnum)profileHero.EarringsSet
                    };
                    heroGrowth.Equipment = new Equipment()
                    {
                        Weapon = profileHero.Equipment?.Weapon ?? new WeaponPiece(),
                        SupportWeapon = profileHero.Equipment?.SupportWeapon ?? new GearPiece(),
                        Armor = profileHero.Equipment?.Armor ?? new ArmorPiece(),
                        SupportArmorFirst = profileHero.Equipment?.SupportArmorFirst ?? new ArmorPiece(),
                        SupportArmorSecond = profileHero.Equipment?.SupportArmorSecond ?? new ArmorPiece(),
                        ArtifactTier = profileHero.Equipment?.ArtifactTier ?? Enums.ArtifactTier.None,
                        ArtifactType = profileHero.Equipment?.ArtifactType ?? Enums.ArtifactType.Normal,
                        ExclusiveWeaponUpgrade = profileHero.Equipment?.ExclusiveWeaponUpgrade ?? 0,
                        IsExclusiveWeaponOwned = profileHero.Equipment?.IsExclusiveWeaponOwned ?? false,
                        ArtifactUpgrade = profileHero.Equipment?.ArtifactUpgrade ?? 0,
                    };
                }
                if (heroGrowth.Equipment == null)
                    heroGrowth.Equipment = new Equipment();

                if (heroGrowth.Earrings == null) 
                    heroGrowth.Earrings = new Accessory() { AccessoryTier = 0, AccessoryUpgradeLevel = 0  };
                if (heroGrowth.Ring == null)
                    heroGrowth.Ring = new Accessory() { AccessoryTier = 0, AccessoryUpgradeLevel = 0 };
                if (heroGrowth.Necklace == null)
                    heroGrowth.Necklace = new Accessory() { AccessoryTier = 0, AccessoryUpgradeLevel = 0 };
                currentHeroes.Add(heroGrowth);
            }

            if (Profile.MaterialsInventory == null)
            {
                Profile.MaterialsInventory = new Inventory();
            }

            if (Profile.Settings == null)
            {
                Profile.Settings = new Settings() { EnergyAdsShop = 900, VulcaRanksClear = 0, VulcaRankTier = Enums.VulcanusRankEnum.Diamond5 };
            }

            if (Profile.HeroPlans == null)
            {
                Profile.HeroPlans = new List<HeroPlan>();
            }

            if (Profile.EwPlans == null)
            {
                Profile.EwPlans = new List<EwPlan>();
            }

            if (Profile.AssaultGearSlots == null)
            {
                Profile.AssaultGearSlots = new ClassGearSlots();
            }
            if (Profile.TankGearSlots == null)
            {
                Profile.TankGearSlots = new ClassGearSlots();
            }
            if (Profile.RangerGearSlots == null)
            {
                Profile.RangerGearSlots = new ClassGearSlots();
            }
            if (Profile.MageGearSlots == null)
            {
                Profile.MageGearSlots = new ClassGearSlots();
            }
            if (Profile.HealerGearSlots == null)
            {
                Profile.HealerGearSlots = new ClassGearSlots();
            }
            if (Profile.VulcanusTeams == null)
            {
                Profile.VulcanusTeams = new List<List<Hero>>()
                {
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                    new List<Hero>(),
                };
            }
            if (Profile.VulcanusSweeps == null)
            {
                Profile.VulcanusSweeps = new bool[12];
            }
            if (Profile.ContentTeams == null)
            {
                Profile.ContentTeams = new List<SerializableContentTeam>();
            }
            if (Profile.TierListTeams == null)
            {
                Profile.TierListTeams = new List<TierListTeam>();
            }

            Profile.MaterialsInventory.PropertyChanged += (s, e) => { Profile.SaveToJson(); };
            Profile.Settings.PropertyChanged += (s, e) => { Profile.SaveToJson(); };

            Profile.HealerGearSlots.PropertyChanged += (s, e) => { GearSlotChanged(); };
            Profile.MageGearSlots.PropertyChanged += (s, e) => { GearSlotChanged(); };
            Profile.RangerGearSlots.PropertyChanged += (s, e) => { GearSlotChanged(); };
            Profile.TankGearSlots.PropertyChanged += (s, e) => { GearSlotChanged(); };
            Profile.AssaultGearSlots.PropertyChanged += (s, e) => { GearSlotChanged(); };

            Heroes = currentHeroes;
        }

        public static void GearSlotChanged()
        {
            Profile.SaveToJson();
            GearSlotChangedEvent(null, EventArgs.Empty);
        }

        public static void CreateBackup()
        {
            Profile?.CreateBackup();
        }

        public static void SaveToFile()
        {
            var profileHeroes = GetProfileHeroGrowth(Heroes);

            Profile.HeroesOwned = profileHeroes;
            Profile.SaveToJson();
        }

        public static async Task ForceSaveToFile()
        {
            var profileHeroes = GetProfileHeroGrowth(Heroes);

            Profile.HeroesOwned = profileHeroes;
            await Profile.ForceSaveToJson();
        }

        private static List<ProfileHeroGrowth> GetProfileHeroGrowth(List<HeroGrowth> Heroes)
        {
            return Heroes.Select(h => new ProfileHeroGrowth()
            {
                HeroName = h.Hero.HeroName,
                HeroType = h.Hero.HeroType,
                IsOwned = h.IsOwned,
                IsCoreOpen = h.IsCoreOpen,
                TraitsOpen = h.TraitsOpen,
                BP = h.BP,
                TransPercentage = h.TransPercentage,
                Level = h.Level,
                ChaserLevel = h.ChaserLevel,
                PetLevel = h.PetLevel,
                DescentLevel = h.DescentLevel,
                TranscendenceLevel = h.TranscendenceLevel,
                SiLevel = h.SiLevel,
                RingTier = (int)(h.Ring?.AccessoryTier ?? 0),
                RingUpgradeLevel = h.Ring?.AccessoryUpgradeLevel ?? 0,
                RingSet = (int)(h.Ring?.AccessorySet ?? 0),
                NecklaceTier = (int)(h.Necklace?.AccessoryTier ?? 0),
                NecklaceUpgradeLevel = h.Necklace?.AccessoryUpgradeLevel ?? 0,
                NecklaceSet = (int)(h.Necklace?.AccessorySet ?? 0),
                EarringsTier = (int)(h.Earrings?.AccessoryTier ?? 0),
                EarringsUpgradeLevel = h.Earrings?.AccessoryUpgradeLevel ?? 0,
                EarringsSet = (int)(h.Earrings?.AccessorySet ?? 0),
                Equipment = h.Equipment,

            }).ToList();
        }
    }
}
