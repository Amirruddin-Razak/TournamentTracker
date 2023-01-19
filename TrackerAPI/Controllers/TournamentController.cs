using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class TournamentController : ControllerBase
{
    [HttpGet]
    public List<TournamentModel> GetActiveTournament()
    {
        List<TournamentModel> activeTournaments = GlobalConfig.connection.GetTournament_All().FindAll(x => x.Active);
        return activeTournaments;
    }

    [HttpPost]
    public void UpdateTournamentResult(TournamentModel tournament)
    {
        TournamentLogic.UpdateTournamentResult(tournament);
    }
}
