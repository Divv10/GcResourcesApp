using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideGT
{
    public interface ITournamentProvider
    {
        TournamentCollection GetCollection();
        List<TournamentDate> GetTournaments();
    }
}
