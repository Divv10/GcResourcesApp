using DivideGtCommons.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideGT
{
    public static class ExtensionMethods
    {
        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime GetNextFinalDate(this DateTime date)
        {
            DateTime finalsDate = date;
            if (finalsDate.DayOfWeek != DayOfWeek.Sunday && finalsDate.DayOfWeek != DayOfWeek.Monday)
            {
                finalsDate = finalsDate.AddDays(7 - (int)finalsDate.DayOfWeek);
            }
            if (finalsDate.DayOfWeek == DayOfWeek.Monday)
            {
                finalsDate = finalsDate.AddDays(-1);
            }
            return finalsDate.Date;
        }

        public static TournamentRecord GetTournamentRecord(this string line)
        {
            line = line.TrimEnd();
            if (line.EndsWith(","))
            {
                line = line.Substring(0, line.Length - 1);
            }
            var arr = line.Split(new char[] { ',' }, StringSplitOptions.None);
            if (arr.Length != 34 && arr.Length != 38)
            {
                return new TournamentRecord()
                {
                    FullString = line,
                    PlayerName = "ERR!"
                };
            }
            var rec = new TournamentRecord()
            {
                FullString = line,
                TournamentDate = DateTime.ParseExact(arr[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Round = (RoundEnum)(int.Parse(arr[1])),
                Guild = (GuildEnum)(int.Parse(arr[2])),
                PlayerName = arr[3],
                Position = int.Parse(arr[4]),                
                AttackEnemyName = arr[34],
            };

            if (!string.IsNullOrEmpty(arr[5]))
            {
                rec.DefenseInfo = new Defense()
                {
                    Defense1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[5], true),
                    Defense2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[6], true),
                    Defense3 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[7], true),
                    Defense4 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[8], true),
                    Defense5 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[9], true),
                    Defense6 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[10], true),
                    Defense7 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[11], true),
                    Defense8 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[12], true),
                    Hidden1 = int.Parse(arr[13]),
                    Hidden2 = int.Parse(arr[14]),
                    Stars = (StarsEnum)int.Parse(arr[15]),
                    Bp = int.Parse(arr[16]),
                };
            }
            if (!string.IsNullOrEmpty(arr[17]))
            {
                rec.AttackInfo = new Attack()
                {
                    Attack1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[17], true),
                    Attack2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[18], true),
                    Attack3 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[19], true),
                    Attack4 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[20], true),
                    Bp = int.Parse(arr[21]),
                };
            }
            if (!string.IsNullOrEmpty(arr[22]))
            {
                rec.AttackDefenseInfo = new Defense()
                {
                    Defense1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[22], true),
                    Defense2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[23], true),
                    Defense3 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[24], true),
                    Defense4 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[25], true),
                    Defense5 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[26], true),
                    Defense6 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[27], true),
                    Defense7 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[28], true),
                    Defense8 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[29], true),
                    Hidden1 = int.Parse(arr[30]),
                    Hidden2 = int.Parse(arr[31]),
                    Stars = (StarsEnum)int.Parse(arr[32]),
                    Bp = int.Parse(arr[33]),
                };
            }

            if (arr.Length == 38)
            {
                rec.DefenseInfo.Pet1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[35], true);
                rec.DefenseInfo.Pet2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[36], true);
                rec.AttackInfo.Pet = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[37], true);
            }

            if (rec.DefenseInfo.Defense1 != HeroEnum.Unknown
                || rec.DefenseInfo.Defense2 != HeroEnum.Unknown
                || rec.DefenseInfo.Defense3 != HeroEnum.Unknown
                || rec.DefenseInfo.Defense4 != HeroEnum.Unknown
                || rec.DefenseInfo.Defense5 != HeroEnum.Unknown
                || rec.DefenseInfo.Defense6 != HeroEnum.Unknown
                || rec.DefenseInfo.Defense7 != HeroEnum.Unknown
                || rec.DefenseInfo.Defense8 != HeroEnum.Unknown)
            {
                rec.RecordType = TournamentRecordEnum.Defense;
            }

            if (rec.AttackInfo.Attack1 != HeroEnum.Unknown
                    || rec.AttackInfo.Attack2 != HeroEnum.Unknown
                    || rec.AttackInfo.Attack3 != HeroEnum.Unknown
                    || rec.AttackInfo.Attack4 != HeroEnum.Unknown)
            {
                if (rec.RecordType == TournamentRecordEnum.Defense)
                    rec.RecordType = TournamentRecordEnum.Both;
                else
                    rec.RecordType = TournamentRecordEnum.Attack;
            }

            return rec;
        }

        public static TournamentRecord GetTournamentRecordScrcpy(this string line, RoundEnum round, GuildEnum guild, IEnumerable<TournamentRecord> previousRecords, bool isCurrentRound)
        {
            if (line == null)
            {
                return new TournamentRecord();
            }
            line = line.TrimEnd();

            if (line.EndsWith(","))
            {
                line = line.Substring(0, line.Length - 1);
            }
            var arr = line?.Split(new char[] { ',' }, StringSplitOptions.None);
            if (arr == null)
                return null;
            if (arr.Length != 15 && arr.Length != 23)
            {
                return new TournamentRecord()
                {
                    FullString = line,
                    PlayerName = "ERR!"
                };
            }
            var rec = new TournamentRecord()
            {
                RecordType = arr[0] == "A" ? TournamentRecordEnum.Attack : TournamentRecordEnum.Defense,
                FullString = line,
                TournamentDate = DateTime.Now.GetNextFinalDate(),
                Round = round,
                Guild = guild,
                PlayerName = arr[2],
                //Position = int.Parse(arr[1]),
                DefenseInfo = new Defense()
                {
                    Defense1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[4], true),
                    Defense2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[5], true),
                    Defense3 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[6], true),
                    Defense4 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[7], true),
                    Defense5 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[8], true),
                    Defense6 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[9], true),
                    Defense7 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[10], true),
                    Defense8 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[11], true),
                    Hidden1 = int.Parse(arr[12]),
                    Hidden2 = int.Parse(arr[13]),
                    Stars = (StarsEnum)int.Parse(arr[3]),
                    //Bp = int.Parse(arr[14]),
                },
                AttackInfo = new Attack()
                {
                    Attack1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[15], true),
                    Attack2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[16], true),
                    Attack3 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[17], true),
                    Attack4 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[18], true),
                },
                AttackDefenseInfo = new Defense()
                {
                    Defense1 = HeroEnum.Unknown,
                    Defense2 = HeroEnum.Unknown,
                    Defense3 = HeroEnum.Unknown,
                    Defense4 = HeroEnum.Unknown,
                    Defense5 = HeroEnum.Unknown,
                    Defense6 = HeroEnum.Unknown,
                    Defense7 = HeroEnum.Unknown,
                    Defense8 = HeroEnum.Unknown,
                    Hidden1 = 1,
                    Hidden2 = 2,
                    Stars = (StarsEnum)int.Parse(arr[3]),
                    Bp = 0,
                },
                AttackEnemyName = null,

            };

            if (rec.DefenseInfo.Defense1 != HeroEnum.Unknown
                    || rec.DefenseInfo.Defense2 != HeroEnum.Unknown
                    || rec.DefenseInfo.Defense3 != HeroEnum.Unknown
                    || rec.DefenseInfo.Defense4 != HeroEnum.Unknown
                    || rec.DefenseInfo.Defense5 != HeroEnum.Unknown
                    || rec.DefenseInfo.Defense6 != HeroEnum.Unknown
                    || rec.DefenseInfo.Defense7 != HeroEnum.Unknown
                    || rec.DefenseInfo.Defense8 != HeroEnum.Unknown)
            {
                rec.RecordType = TournamentRecordEnum.Defense;
            }

            if (rec.AttackInfo.Attack1 != HeroEnum.Unknown
                    || rec.AttackInfo.Attack2 != HeroEnum.Unknown
                    || rec.AttackInfo.Attack3 != HeroEnum.Unknown
                    || rec.AttackInfo.Attack4 != HeroEnum.Unknown)
            {
                if (rec.RecordType == TournamentRecordEnum.Defense)
                    rec.RecordType = TournamentRecordEnum.Both;
                else
                    rec.RecordType = TournamentRecordEnum.Attack;
            }

            if (arr.Length == 23)
            {
                rec.DefenseInfo.Pet1 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[20], true);
                rec.DefenseInfo.Pet2 = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[21], true);
                rec.AttackInfo.Pet = (HeroEnum)Enum.Parse(typeof(HeroEnum), arr[22], true);
            }

            int position;
            int bp;
            int bpAtk;
            rec.ErrorMsg = string.Empty;

            if (!int.TryParse(arr[1], out position) || position > 30 || position < 1)
            {
                rec.IsError = true;
                rec.ErrorMsg += " Invalid position;";
            }
            if ((!int.TryParse(arr[14], out bp) || ((bp > 2100000 || bp < 1000000) && bp != 5400)) && rec.RecordType == TournamentRecordEnum.Defense)
            {
                rec.IsError = true;
                rec.ErrorMsg += " Invalid BP;";
            }
            if ((!int.TryParse(arr[19], out bpAtk) || bpAtk > 2100000 || bpAtk < 1000000) && rec.RecordType == TournamentRecordEnum.Attack)
            {
                rec.IsError = true;
                rec.ErrorMsg += " Invalid BP;";
            }
            if (!previousRecords.Any(r => r.PlayerName == rec.PlayerName))
            {
                rec.IsError = true;
                rec.ErrorMsg += " No such player in previous rounds!;";
            }

            if (rec.RecordType == TournamentRecordEnum.Both || rec.RecordType == TournamentRecordEnum.Defense)
            {
                if (!isCurrentRound && (rec.DefenseInfo.Defense1 == HeroEnum.Unknown
                    || rec.DefenseInfo.Defense2 == HeroEnum.Unknown
                    || rec.DefenseInfo.Defense3 == HeroEnum.Unknown
                    || rec.DefenseInfo.Defense4 == HeroEnum.Unknown
                    || rec.DefenseInfo.Defense5 == HeroEnum.Unknown
                    || rec.DefenseInfo.Defense6 == HeroEnum.Unknown
                    || rec.DefenseInfo.Defense7 == HeroEnum.Unknown
                    || rec.DefenseInfo.Defense8 == HeroEnum.Unknown))
                {
                    rec.IsError = true;
                    rec.ErrorMsg += " Hero not recognized;";
                }

                if (rec.DefenseInfo.Pet1 == HeroEnum.Unknown || rec.DefenseInfo.Pet2 == HeroEnum.Unknown)
                {
                    rec.IsError = true;
                    rec.ErrorMsg += " Pet not recognized;";
                }
            }
            else
            {
                if (rec.AttackInfo.Attack1 == HeroEnum.Unknown
                    || rec.AttackInfo.Attack2 == HeroEnum.Unknown
                    || rec.AttackInfo.Attack3== HeroEnum.Unknown
                    || rec.AttackInfo.Attack4 == HeroEnum.Unknown)
                {
                    rec.IsError = true;
                    rec.ErrorMsg += " Hero not recognized;";
                }

                if (rec.AttackInfo.Pet == HeroEnum.Unknown)
                {
                    rec.IsError = true;
                    rec.ErrorMsg += " Pet not recognized;";
                }
            }

            if (isCurrentRound && ((rec.DefenseInfo.Defense1 == HeroEnum.Unknown && rec.DefenseInfo.Hidden1 != 1 && rec.DefenseInfo.Hidden2 != 1)
                || (rec.DefenseInfo.Defense2 == HeroEnum.Unknown && rec.DefenseInfo.Hidden1 != 2 && rec.DefenseInfo.Hidden2 != 2)
                || (rec.DefenseInfo.Defense3 == HeroEnum.Unknown && rec.DefenseInfo.Hidden1 != 3 && rec.DefenseInfo.Hidden2 != 3)
                || (rec.DefenseInfo.Defense4 == HeroEnum.Unknown && rec.DefenseInfo.Hidden1 != 4 && rec.DefenseInfo.Hidden2 != 4)
                || (rec.DefenseInfo.Defense5 == HeroEnum.Unknown && rec.DefenseInfo.Hidden1 != 5 && rec.DefenseInfo.Hidden2 != 5)
                || (rec.DefenseInfo.Defense6 == HeroEnum.Unknown && rec.DefenseInfo.Hidden1 != 6 && rec.DefenseInfo.Hidden2 != 6)
                || (rec.DefenseInfo.Defense7 == HeroEnum.Unknown && rec.DefenseInfo.Hidden1 != 7 && rec.DefenseInfo.Hidden2 != 7)
                || (rec.DefenseInfo.Defense8 == HeroEnum.Unknown && rec.DefenseInfo.Hidden1 != 8 && rec.DefenseInfo.Hidden2 != 8)))
            {
                rec.IsError = true;
                rec.ErrorMsg += " Hero not recognized;";
            }
            rec.Position = position;
            rec.DefenseInfo.Bp = bp;
            rec.AttackInfo.Bp = bpAtk;
            return rec;
        }
    }
}
