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
            if (currentRank == GearSlotRankEnum.Ancient)
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
            double raidMaterial = 180;
            double matTotal = 0;
            double matCost = 0;
            double slotExpCost;
            double slotEXP = 0;

            if (slotRank == GearSlotRankEnum.Normal)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matTotal += 0;
                            break;
                        case 1:
                            slotExpCost = 25760 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 2:
                            slotExpCost = 23040 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 3:
                            slotExpCost = 20480 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 4:
                            slotExpCost = 17920 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 5:
                            slotExpCost = 15360 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 6:
                            slotExpCost = 12800 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 7:
                            slotExpCost = 6400 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 8:
                            slotExpCost = 3840 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 9:
                            slotExpCost = 2560 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
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
                            matTotal += 0;
                            break;
                        case 1:
                            slotExpCost = 51520 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 2:
                            slotExpCost = 46240 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 3:
                            slotExpCost = 41120 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 4:
                            slotExpCost = 36000 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 5:
                            slotExpCost = 30880 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 6:
                            slotExpCost = 25760 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 7:
                            slotExpCost = 12800 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 8:
                            slotExpCost = 51520 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 9:
                            slotExpCost = 51520 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                    }
                }
            } // DONE
            if (slotRank == GearSlotRankEnum.Rare)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matTotal += 0;
                            break;
                        case 1:
                            slotExpCost = 77280 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 2:
                            slotExpCost = 69440 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 3:
                            slotExpCost = 61760 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 4:
                            slotExpCost = 54080 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 5:
                            slotExpCost = 46240 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 6:
                            slotExpCost = 38560 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 7:
                            slotExpCost = 19200 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 8:
                            slotExpCost = 11520 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 9:
                            slotExpCost = 7680 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                    }
                }
            } // DONE
            if (slotRank == GearSlotRankEnum.Unique)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matTotal += 0;
                            break;
                        case 1:
                            slotExpCost = 103040 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 2:
                            slotExpCost = 92640 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 3:
                            slotExpCost = 82400 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 4:
                            slotExpCost = 72000 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 5:
                            slotExpCost = 61760 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 6:
                            slotExpCost = 51520 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 7:
                            slotExpCost = 25760 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 8:
                            slotExpCost = 15360 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 9:
                            slotExpCost = 10240 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                    }
                }
            } // DONE
            if (slotRank == GearSlotRankEnum.Legendary)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch (i)
                    {
                        case 0:
                            matTotal += 0;
                            break;
                        case 1:
                            slotExpCost = 292920 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 2:
                            slotExpCost = 263355 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 3:
                            slotExpCost = 234245 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 4:
                            slotExpCost = 204680 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 5:
                            slotExpCost = 175570 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 6:
                            slotExpCost = 146570 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 7:
                            slotExpCost = 73230 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 8:
                            slotExpCost = 43665 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 9:
                            slotExpCost = 29110 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                    }
                }
            } // DONE

            if (slotRank == GearSlotRankEnum.Ancient)
            {
                for (int i = 0; i <= upgradeLevel; i++)
                {
                    switch(i)
                    {
                        case 0:
                            matTotal += 0;
                            break;
                        case 1:
                            slotExpCost = 311140 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 2:
                            slotExpCost = 279730 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 3: // Estimated
                            slotExpCost = 250000 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 4:
                            slotExpCost = 248810 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 5:
                            slotExpCost = 217410 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 6:
                            slotExpCost = 155570 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 7:
                            slotExpCost = 77780 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 8:
                            slotExpCost = 46380 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                        case 9:
                            slotExpCost = 30920 + slotEXP;
                            matCost = slotExpCost / raidMaterial;
                            matTotal += matCost;
                            slotEXP = slotExpCost % raidMaterial;
                            break;
                    }
                }
            }
            return (int)matTotal;
        }
    }
}
