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
            int raidMaterial = 175;
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
                            matCost += 25760 / raidMaterial; // 151
                            break;
                        case 2:
                            matCost += 23040 / raidMaterial + 1; // 136
                            break;
                        case 3:
                            matCost += 20480 / raidMaterial; // 120
                            break;
                        case 4:
                            matCost += 17920 / raidMaterial + 1; // 106
                            break;
                        case 5:
                            matCost += 15360 / raidMaterial; // 90
                            break;
                        case 6:
                            matCost += 12800 / raidMaterial; // 75
                            break;
                        case 7:
                            matCost += 6400 / raidMaterial + 1; // 38
                            break;
                        case 8:
                            matCost += 3840 / raidMaterial; // 22
                            break;
                        case 9:
                            matCost += 2560 / raidMaterial + 1; // 16
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
                            matCost += 51520 / raidMaterial + 1; // 1470
                            break;
                        case 2:
                            matCost += 46240 / raidMaterial; // 1175
                            break;
                        case 3:
                            matCost += 41120 / raidMaterial + 1; // 911
                            break;
                        case 4:
                            matCost += 36000 / raidMaterial + 1; // 676
                            break;
                        case 5:
                            matCost += 30880 / raidMaterial; // 470
                            break;
                        case 6:
                            matCost += 25760 / raidMaterial; // 294
                            break;
                        case 7:
                            matCost += 12800 / raidMaterial; // 147
                            break;
                        case 8:
                            matCost += 7680 / raidMaterial + 1; // 74
                            break;
                        case 9:
                            matCost += 5120 / raidMaterial; // 30
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
                            matCost += 0;
                            break;
                        case 1:
                            matCost += 77280 / raidMaterial + 1; // 2205
                            break;
                        case 2:
                            matCost += 69440 / raidMaterial + 1; // 1763
                            break;
                        case 3:
                            matCost += 61760 / raidMaterial; // 1366
                            break;
                        case 4:
                            matCost += 54080 / raidMaterial + 1; // 1014
                            break;
                        case 5:
                            matCost += 46240 / raidMaterial; // 704
                            break;
                        case 6:
                            matCost += 38560 / raidMaterial + 1; // 440
                            break;
                        case 7:
                            matCost += 19200 / raidMaterial + 1; // 220
                            break;
                        case 8:
                            matCost += 11520 / raidMaterial; // 110
                            break;
                        case 9:
                            matCost += 7680 / raidMaterial + 1; // 44
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
                            matCost += 0;
                            break;
                        case 1:
                            matCost += 103040 / raidMaterial + 1; // 2942
                            break;
                        case 2:
                            matCost += 92640 / raidMaterial; // 2353
                            break;
                        case 3:
                            matCost += 82400 / raidMaterial + 1; // 1824
                            break;
                        case 4:
                            matCost += 72000 / raidMaterial + 1; // 1353
                            break;
                        case 5:
                            matCost += 61760 / raidMaterial + 1; // 941
                            break;
                        case 6:
                            matCost += 51520 / raidMaterial; // 588
                            break;
                        case 7:
                            matCost += 25760 / raidMaterial; // 294
                            break;
                        case 8:
                            matCost += 15360 / raidMaterial + 1; // 147
                            break;
                        case 9:
                            matCost += 10240 / raidMaterial + 1; // 59
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
                            matCost += 0;
                            break;
                        case 1:
                            matCost += 292920 / raidMaterial + 1; // 8362
                            break;
                        case 2:
                            matCost += 263355 / raidMaterial + 1; // 6688
                            break;
                        case 3:
                            matCost += 234245 / raidMaterial; // 5183
                            break;
                        case 4:
                            matCost += 204680 / raidMaterial + 1; // 3845
                            break;
                        case 5:
                            matCost += 175570 / raidMaterial; // 2675
                            break;
                        case 6:
                            matCost += 146570 / raidMaterial; // 1672
                            break;
                        case 7:
                            matCost += 73230 / raidMaterial + 1; // 835
                            break;
                        case 8:
                            matCost += 43665 / raidMaterial; // 416
                            break;
                        case 9:
                            matCost += 29110 / raidMaterial + 1; // 167
                            break;
                    }
                }
            } // DONE

            return matCost;
        }
    }
}
