using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class TeamController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllTeam()
    {
        List<TeamModel> teamList = GlobalConfig.connection.GetTeam_All();

        if (teamList.Count == 0)
        {
            return NotFound();
        }
        else
        {
            return Ok(teamList);
        }
    }

    [HttpPost]
    public IActionResult CreateTeam(TeamModel team)
    {
        GlobalConfig.connection.SaveNewTeam(team);
        return StatusCode(StatusCodes.Status201Created, team);
    }
}
