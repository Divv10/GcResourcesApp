using GCManagementApp.Enums;
using GCManagementApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Models
{
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class ProfileHeroGrowth
    {
        public HeroEnum HeroName { get; set; }
        public HeroType HeroType { get; set; }
        public int TranscendenceLevel { get; set; }
        public int Level { get; set; }
        public int SiLevel { get; set; }
        public bool IsCoreOpen { get; set; }
        public int TraitsOpen { get; set; }
        public int BP { get; set; }
        public int ChaserLevel { get; set; }
        public int DescentLevel { get; set; }
        public double TransPercentage { get; set; }
        public int PetLevel { get; set; }
        public bool IsOwned { get; set; }
        public int RingTier { get; set; }
        public int RingUpgradeLevel { get; set; }
        public int RingSet { get; set; }
        public int NecklaceTier { get; set; }
        public int NecklaceUpgradeLevel { get; set; }
        public int NecklaceSet { get; set; }
        public int EarringsTier { get; set; }
        public int EarringsUpgradeLevel { get; set; }
        public int EarringsSet { get; set; }
        public Equipment Equipment { get; set; }
        public override string ToString()
        {
            return $"{Properties.Resources.ResourceManager.GetObject(HeroName.GetDescription()) ?? HeroName}({HeroType})";
        }
    }
}
