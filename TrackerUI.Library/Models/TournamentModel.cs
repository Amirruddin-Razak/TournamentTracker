using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerUI.Library.Models;

/// <summary>
/// Track the whole tournament detail
/// </summary>
public class TournamentModel : IDataModel
{
    public int Id { get; set; }
    public string TournamentName { get; set; }
    public decimal EntreeFee { get; set; }
    public List<TeamModel> TeamList { get; set; } = new List<TeamModel>();
    public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
    public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();
    public bool Active { get; set; } = true;

    public TournamentModel()
    {

    }

    public TournamentModel(TrackerLibrary.Models.TournamentModel model)
    {
        if (model == null)
        {
            return;
        }

        Id = model.Id;
        TournamentName = model.TournamentName;
        EntreeFee = model.EntreeFee;
        TeamList = model.TeamList.Select(team => new TeamModel(team)).ToList();
        Prizes = model.Prizes.Select(prize => new PrizeModel(prize)).ToList();
        Rounds = model.Rounds.Select(round => round.Select(matchup => new MatchupModel(matchup)).ToList()).ToList();
        Active = model.Active;
    }

    public event EventHandler<DateTime> OnTournamentComplete;

    public void CompleteTournament()
    {
        OnTournamentComplete?.Invoke(this, DateTime.Now);
    }
}
