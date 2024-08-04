using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideGT
{
    public class EnumDropDown : NotifyPropertyChanged
    {
        private GuildEnum _guild;
        public GuildEnum Guild
        {
            get => _guild;
            set => SetProperty(ref _guild, value);
        }

        public string GuildName => Guild.ToString();
    }
}
