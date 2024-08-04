using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideGT
{
    public class TournamentCollection : NotifyPropertyChanged
    {
        private List<TournamentRecord> _collection;
        public List<TournamentRecord> Collection
        {
            get => _collection;
            set => SetProperty(ref _collection, value);
        }

        public TournamentCollection()
        {
            Collection = new List<TournamentRecord>();
        }

        public void AddRecord(TournamentRecord record)
        {
            Collection.Add(record);
        }

        public List<TournamentRecord> GetGuildRecords(DateTime date, GuildEnum guild)
        {            
            return Collection.Where(x => x.Guild == guild && x.TournamentDate.GetIso8601WeekOfYear() == date.GetIso8601WeekOfYear()).ToList();
        }

        public List<TournamentRecord> GetGuildRecords(DateTime date, GuildEnum guild, RoundEnum round)
        {
            return GetGuildRecords(date, guild).Where(x => x.Round == round).ToList();
        }

        public List<TournamentDate> GetAvailableTournaments()
        {
            return Collection.Select(x => new TournamentDate(x.TournamentDate)).GroupBy(x => x.Week).Select(x => x.FirstOrDefault()).ToList();
        }

        public List<TournamentRecord> GetFullDetails(GuildEnum guild, string playerName, TournamentDate tournamentDate)
        {
            return Collection.Where(x => x.Guild == guild && x.TournamentDate.GetIso8601WeekOfYear() == tournamentDate.FinalDay.GetIso8601WeekOfYear() && x.PlayerName == playerName).ToList();
        }
    }
}
