using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        void SaveNewPrize(PrizeModel model);
        void SaveNewPerson(PersonModel model);
        void SaveNewTeam(TeamModel model);
        void SaveNewTournament(TournamentModel model);

        void UpdateMatchupWinner(MatchupModel model);
        void UpdateTournamentComplete(TournamentModel model);

        List<PersonModel> GetPerson_All();
        List<TeamModel> GetTeam_All();
        List<TournamentModel> GetTournament_All();
    }
}
