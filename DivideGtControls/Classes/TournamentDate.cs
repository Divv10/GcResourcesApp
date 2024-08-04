using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideGT
{
    public class TournamentDate : NotifyPropertyChanged
    {
        private DateTime _finalDay;
        public DateTime FinalDay
        {
            get => _finalDay;
            set => SetProperty(ref _finalDay, value);
        }

        public TournamentDate(DateTime date)
        {
            if (date.DayOfWeek != DayOfWeek.Sunday)
            {
                date = date.AddDays(7 - (int)date.DayOfWeek);
            }
            this.FinalDay = date;
        }

        public int Week => FinalDay.GetIso8601WeekOfYear();

        public string DisplayName => $"Week {Week} (Final on {FinalDay.ToShortDateString()})";
    }
}
