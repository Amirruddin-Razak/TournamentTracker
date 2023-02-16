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
    public IActionResult GetActiveTournament()
    {
        List<TournamentModel> activeTournaments = GlobalConfig.connection.GetTournament_All().FindAll(x => x.Active);

        if (activeTournaments.Count > 0)
        {
            return Ok(activeTournaments);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult UpdateTournamentResult(TournamentModel tournament)
    {
        TournamentLogic.UpdateTournamentResult(tournament);
        return Ok(tournament);
    }

    [HttpPost]
    public IActionResult CreateTournament(TournamentModel tournament)
    {
        TournamentLogic.CreateNewTournament(tournament);
        return StatusCode(StatusCodes.Status201Created, tournament);
    }
}
