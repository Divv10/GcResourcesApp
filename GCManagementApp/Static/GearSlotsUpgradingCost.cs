using GCManagementApp.Enums;
using GCManagementApp.Models;
using NetSparkleUpdater.Interfaces;
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
                matsCost += GearSlotFormula(currentRank, (int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
                stonesCost += 80;
                currentUpgrade = 0;
                currentRank = GearSlotRankEnum.Premium;
            }
            if (currentRank == GearSlotRankEnum.Premium)
            {
                matsCost += GearSlotFormula(currentRank, (int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
                stonesCost += 209;
                currentUpgrade = 0;
                currentRank = GearSlotRankEnum.Rare;
            }
            if (currentRank == GearSlotRankEnum.Rare)
            {
                matsCost += GearSlotFormula(currentRank, (int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
                stonesCost += 358;
                currentUpgrade = 0;
                currentRank = GearSlotRankEnum.Unique;
            }
            if (currentRank == GearSlotRankEnum.Unique)
            {
                matsCost += GearSlotFormula(currentRank, (int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
                stonesCost += 455;
                currentUpgrade = 0;
                currentRank = GearSlotRankEnum.Legendary;
            }
            if (currentRank == GearSlotRankEnum.Legendary)
            {
                matsCost += GearSlotFormula(currentRank, (int)StaticValues.MaxGearSlotUpgrade - currentUpgrade);
                currentUpgrade = 0;
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

        private static int GearSlotFormula(GearSlotRankEnum slotRank, int upgradeLevel)
        {
            int raid15Material = 170;
            int matCost = 0;

            if (slotRank == GearSlotRankEnum.Normal)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matCost += 0;
                            break;
                        case 1:
                            matCost += 25760 / raid15Material; // 151
                            break;
                        case 2:
                            matCost += 23040 / raid15Material + 1; // 136
                            break;
                        case 3:
                            matCost += 20480 / raid15Material; // 120
                            break;
                        case 4:
                            matCost += 17920 / raid15Material + 1; // 106
                            break;
                        case 5:
                            matCost += 15360 / raid15Material; // 90
                            break;
                        case 6:
                            matCost += 12800 / raid15Material; // 75
                            break;
                        case 7:
                            matCost += 6400 / raid15Material + 1; // 38
                            break;
                        case 8:
                            matCost += 3840 / raid15Material; // 22
                            break;
                        case 9:
                            matCost += 2560 / raid15Material + 1; // 16
                            break;
                    }
                }
            }
            if (slotRank == GearSlotRankEnum.Premium)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matCost += 0;
                            break;
                        case 1:
                            matCost += 51520 / raid15Material; // 303
                            break;
                        case 2:
                            matCost += 46240 / raid15Material; // 272
                            break;
                        case 3:
                            matCost += 41120 / raid15Material; // 242
                            break;
                        case 4:
                            matCost += 36000 / raid15Material + 1; // 212
                            break;
                        case 5:
                            matCost += 30880 / raid15Material; // 181
                            break;
                        case 6:
                            matCost += 25760 / raid15Material; // 152
                            break;
                        case 7:
                            matCost += 12800 / raid15Material; // 75
                            break;
                        case 8:
                            matCost += 7680 / raid15Material; // 45
                            break;
                        case 9:
                            matCost += 5120 / raid15Material + 1; // 31
                            break;
                    }
                }
            }
            if (slotRank == GearSlotRankEnum.Rare)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matCost += 0;
                            break;
                        case 1:
                            matCost += 77280 / raid15Material + 1; // 455
                            break;
                        case 2:
                            matCost += 69440 / raid15Material + 1; // 409
                            break;
                        case 3:
                            matCost += 61760 / raid15Material; // 363
                            break;
                        case 4:
                            matCost += 54080 / raid15Material + 1; // 106
                            break;
                        case 5:
                            matCost += 46240 / raid15Material; // 272
                            break;
                        case 6:
                            matCost += 38560 / raid15Material + 1; // 227
                            break;
                        case 7:
                            matCost += 19200 / raid15Material + 1; // 113
                            break;
                        case 8:
                            matCost += 11520 / raid15Material; // 67
                            break;
                        case 9:
                            matCost += 7680 / raid15Material + 1; // 46
                            break;
                    }
                }
            }
            if (slotRank == GearSlotRankEnum.Unique)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matCost += 0;
                            break;
                        case 1:
                            matCost += 103040 / raid15Material; // 606
                            break;
                        case 2:
                            matCost += 92640 / raid15Material + 1; // 545
                            break;
                        case 3:
                            matCost += 82400 / raid15Material + 1; // 485
                            break;
                        case 4:
                            matCost += 72000 / raid15Material; // 403
                            break;
                        case 5:
                            matCost += 61760 / raid15Material; // 363
                            break;
                        case 6:
                            matCost += 51520 / raid15Material; // 303
                            break;
                        case 7:
                            matCost += 25760 / raid15Material + 1; // 152
                            break;
                        case 8:
                            matCost += 15360 / raid15Material; // 90
                            break;
                        case 9:
                            matCost += 10240 / raid15Material + 1; // 61
                            break;
                    }
                }
            }
            if (slotRank == GearSlotRankEnum.Legendary)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matCost += 0;
                            break;
                        case 1:
                            matCost += 292920 / raid15Material + 1; // 1724
                            break;
                        case 2:
                            matCost += 263355 / raid15Material; // 1549
                            break;
                        case 3:
                            matCost += 234245 / raid15Material + 1; // 1378
                            break;
                        case 4:
                            matCost += 204680 / raid15Material; // 1204
                            break;
                        case 5:
                            matCost += 175570 / raid15Material + 1; // 1033
                            break;
                        case 6:
                            matCost += 146570 / raid15Material; // 862
                            break;
                        case 7:
                            matCost += 73230 / raid15Material; // 430
                            break;
                        case 8:
                            matCost += 43665 / raid15Material + 1; // 257
                            break;
                        case 9:
                            matCost += 29110 / raid15Material + 1; // 172
                            break;
                    }
                }
            }

            return matCost;
        }
    }
}
