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
    public List<TeamModel> GetAllTeam()
    {
        List<TeamModel> teamList = GlobalConfig.connection.GetTeam_All();
        return teamList;
    }
}
