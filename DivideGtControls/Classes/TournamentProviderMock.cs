using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideGT
{
    public class TournamentProviderMock : ITournamentProvider
    {
        private TournamentCollection _collection;
        public TournamentCollection GetCollection()
        {
            if (_collection == null)
            {
                _collection = new TournamentCollection();
                var record = new TournamentRecord()
                {
                    Guild = GuildEnum.bpcy,
                    PlayerName = "Rocks",
                    Position = 1,
                    Round = RoundEnum.RoundOf64,
                    TournamentDate = DateTime.Now.AddDays(-5),
                    AttackEnemyName = "Xanthe",
                    AttackInfo = new Attack()
                    {
                        Attack1 = HeroEnum.Elesis,
                        Attack2 = HeroEnum.Harpe,
                        Attack3 = HeroEnum.Hwarin,
                        Attack4 = HeroEnum.Ryan,
                    },
                    AttackDefenseInfo = new Defense()
                    {
                        Bp = 2000000,
                        Stars = StarsEnum.ThreeStar,
                        Defense1 = HeroEnum.Ronan,
                        Defense2 = HeroEnum.Rin,
                        Defense3 = HeroEnum.Nelia,
                        Defense4 = HeroEnum.Lime,
                        Defense5 = HeroEnum.Grandiel,
                        Defense6 = HeroEnum.Harpe,
                        Defense7 = HeroEnum.Hwarin,
                        Defense8 = HeroEnum.Dio,
                        Hidden1 = 1,
                        Hidden2 = 2,
                    },
                    DefenseInfo = new Defense()
                    {
                        Bp = 2000000,
                        Stars = StarsEnum.ThreeStar,
                        Defense1 = HeroEnum.Ronan,
                        Defense2 = HeroEnum.Rin,
                        Defense3 = HeroEnum.Nelia,
                        Defense4 = HeroEnum.Lime,
                        Defense5 = HeroEnum.Grandiel,
                        Defense6 = HeroEnum.Harpe,
                        Defense7 = HeroEnum.Hwarin,
                        Defense8 = HeroEnum.Dio,
                        Hidden1 = 1,
                        Hidden2 = 2,
                    }
                };
                _collection.AddRecord(record);

                record = new TournamentRecord()
                {
                    Guild = GuildEnum.Vanquish,
                    PlayerName = "Xanthe",
                    Position = 1,
                    Round = RoundEnum.RoundOf64,
                    TournamentDate = DateTime.Now.AddDays(-5),
                    AttackEnemyName = "Rocks",
                    AttackInfo = new Attack()
                    {
                        Attack1 = HeroEnum.Elesis,
                        Attack2 = HeroEnum.Harpe,
                        Attack3 = HeroEnum.Hwarin,
                        Attack4 = HeroEnum.Ryan,
                    },
                    AttackDefenseInfo = new Defense()
                    {
                        Bp = 2000000,
                        Stars = StarsEnum.ThreeStar,
                        Defense1 = HeroEnum.Ronan,
                        Defense2 = HeroEnum.Rin,
                        Defense3 = HeroEnum.Nelia,
                        Defense4 = HeroEnum.Lime,
                        Defense5 = HeroEnum.Grandiel,
                        Defense6 = HeroEnum.Harpe,
                        Defense7 = HeroEnum.Hwarin,
                        Defense8 = HeroEnum.Dio,
                        Hidden1 = 1,
                        Hidden2 = 2,
                    },
                    DefenseInfo = new Defense()
                    {
                        Bp = 2000000,
                        Stars = StarsEnum.ThreeStar,
                        Defense1 = HeroEnum.Ronan,
                        Defense2 = HeroEnum.Rin,
                        Defense3 = HeroEnum.Nelia,
                        Defense4 = HeroEnum.Lime,
                        Defense5 = HeroEnum.Grandiel,
                        Defense6 = HeroEnum.Harpe,
                        Defense7 = HeroEnum.Hwarin,
                        Defense8 = HeroEnum.Dio,
                        Hidden1 = 1,
                        Hidden2 = 2,
                    }
                };
                _collection.AddRecord(record);

                record = new TournamentRecord()
                {
                    Guild = GuildEnum.Imperious,
                    PlayerName = "Halko",
                    Position = 1,
                    Round = RoundEnum.RoundOf64,
                    TournamentDate = DateTime.Now.AddDays(-5),
                    AttackEnemyName = "Lime",
                    AttackInfo = new Attack()
                    {
                        Attack1 = HeroEnum.Elesis,
                        Attack2 = HeroEnum.Harpe,
                        Attack3 = HeroEnum.Hwarin,
                        Attack4 = HeroEnum.Ryan,
                    },
                    AttackDefenseInfo = new Defense()
                    {
                        Bp = 2000000,
                        Stars = StarsEnum.ThreeStar,
                        Defense1 = HeroEnum.Ronan,
                        Defense2 = HeroEnum.Rin,
                        Defense3 = HeroEnum.Nelia,
                        Defense4 = HeroEnum.Lime,
                        Defense5 = HeroEnum.Grandiel,
                        Defense6 = HeroEnum.Harpe,
                        Defense7 = HeroEnum.Hwarin,
                        Defense8 = HeroEnum.Dio,
                        Hidden1 = 1,
                        Hidden2 = 2,
                    },
                    DefenseInfo = new Defense()
                    {
                        Bp = 2000000,
                        Stars = StarsEnum.ThreeStar,
                        Defense1 = HeroEnum.Ronan,
                        Defense2 = HeroEnum.Rin,
                        Defense3 = HeroEnum.Nelia,
                        Defense4 = HeroEnum.Lime,
                        Defense5 = HeroEnum.Grandiel,
                        Defense6 = HeroEnum.Harpe,
                        Defense7 = HeroEnum.Hwarin,
                        Defense8 = HeroEnum.Dio,
                        Hidden1 = 1,
                        Hidden2 = 2,
                    }
                };
                _collection.AddRecord(record);
            }
            return _collection;
        }

        public List<TournamentDate> GetTournaments()
        {
            return GetCollection().GetAvailableTournaments();
        }
    }
}
