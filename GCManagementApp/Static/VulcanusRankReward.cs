using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCManagementApp.Static
{
    [Serializable]
    public class VulcanusRankReward
    {
        public string Rank {  get; set; }
        public int Energy { get; set; }
        public int BlueGems { get; set; }

        public VulcanusRankReward(string rank, int energy, int blueGems)
        {
            Rank = rank;
            Energy = energy;
            BlueGems = blueGems;
        }

        public static VulcanusRankReward[] GetRankRewards { get; } =
            new[]
            {
                new VulcanusRankReward("Challenger 1", 1500, 40000),
                new VulcanusRankReward("Challenger 2", 1470, 35000),
                new VulcanusRankReward("Challenger 3", 1430, 33000),
                new VulcanusRankReward("Challenger 4-10", 1410, 32000),
                new VulcanusRankReward("Challenger 11-20", 1390, 31000),
                new VulcanusRankReward("Challenger 21-30", 1370, 30500),
                new VulcanusRankReward("Challenger 31-40", 1350, 30000),
                new VulcanusRankReward("Challenger 41-50", 1330, 29500),
                new VulcanusRankReward("Diamond 1", 1310, 29000),
                new VulcanusRankReward("Platinum 1", 1290, 28500),
                new VulcanusRankReward("Platinum 2", 1270, 28000),
                new VulcanusRankReward("Gold 1", 1250, 27500),
                new VulcanusRankReward("Gold 2", 1230, 27000),
                new VulcanusRankReward("Silver 1", 1210, 26500),
                new VulcanusRankReward("Silver 2", 1190, 26000),
                new VulcanusRankReward("Bronze 1", 1170, 25500),
                new VulcanusRankReward("Bronze 2", 1150, 25000),
            };
    }
}
