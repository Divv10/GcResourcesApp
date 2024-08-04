using GCManagementApp.Enums;
using GCManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    public static class GearSlotsUpgradingCost
    {
        public static GearSlotCost CalculateCostForSlot(GearSlot slot)
        {
            int matsCost = 0;
            int stonesCost = 0;

            int currentUpgrade = slot.UpgradeLevel;
            GearSlotRankEnum currentRank = slot.Rank;

            if (currentRank == GearSlotRankEnum.Normal)
            {
                matsCost += (732 / 9) * ((int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
                stonesCost += 105;
                currentUpgrade = 0;
                currentRank = GearSlotRankEnum.Premium;
            }
            if (currentRank == GearSlotRankEnum.Premium)
            {
                matsCost += (1468 / 9) * ((int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
                stonesCost += 327;
                currentUpgrade = 0;
                currentRank = GearSlotRankEnum.Rare;
            }
            if (currentRank == GearSlotRankEnum.Rare)
            {
                matsCost += (2212 / 9) * ((int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
                stonesCost += 655;
                currentUpgrade = 0;
                currentRank = GearSlotRankEnum.Unique;
            }
            if (currentRank == GearSlotRankEnum.Unique)
            {
                matsCost += (2938 / 9) * ((int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
            }

            return new GearSlotCost() { RaidMatsCost = matsCost, BlueStonesCost= stonesCost };
        }

        public static GearSlotCost CalculateCostForClass(HeroClass heroClass)
        {
            ClassGearSlots slots = null;

            switch (heroClass)
            {
                default:
                    slots = ProfileGrowth.Profile.TankGearSlots; break;
                case HeroClass.Healer:
                    slots = ProfileGrowth.Profile.HealerGearSlots; break;
                case HeroClass.Ranger:
                    slots = ProfileGrowth.Profile.RangerGearSlots; break;
                case HeroClass.Assault:
                    slots = ProfileGrowth.Profile.AssaultGearSlots; break;
                case HeroClass.Mage:
                    slots = ProfileGrowth.Profile.MageGearSlots; break;
            }

            var weaponCost = CalculateCostForSlot(slots.Weapon);
            var subWCost = CalculateCostForSlot(slots.SecondaryWeapon);
            var armorCost = CalculateCostForSlot(slots.Armor);
            var sArmor1Cost = CalculateCostForSlot(slots.SecondaryArmorOne);
            var sArmor2Cost = CalculateCostForSlot(slots.SecondaryArmorTwo);

            var totalMats = weaponCost.RaidMatsCost + subWCost.RaidMatsCost + armorCost.RaidMatsCost + sArmor1Cost.RaidMatsCost + sArmor2Cost.RaidMatsCost;
            var totalStones = weaponCost.BlueStonesCost + subWCost.BlueStonesCost + armorCost.BlueStonesCost + sArmor1Cost.BlueStonesCost + sArmor2Cost.BlueStonesCost;

            return new GearSlotCost() { RaidMatsCost = totalMats, BlueStonesCost = totalStones };
        }

        public static GearSlotCost CalculateCostForAllClasses()
        {
            var tank = CalculateCostForClass(HeroClass.Tank);
            var healer = CalculateCostForClass(HeroClass.Healer);
            var ranger = CalculateCostForClass(HeroClass.Ranger);
            var assault = CalculateCostForClass(HeroClass.Assault);
            var mage = CalculateCostForClass(HeroClass.Mage);

            var totalMats = tank.RaidMatsCost + healer.RaidMatsCost + ranger.RaidMatsCost + assault.RaidMatsCost + mage.RaidMatsCost;
            var totalStones = tank.BlueStonesCost + healer.BlueStonesCost + ranger.BlueStonesCost + assault.BlueStonesCost + mage.BlueStonesCost;

            return new GearSlotCost() { RaidMatsCost = totalMats, BlueStonesCost = totalStones };
        }
    }
}
