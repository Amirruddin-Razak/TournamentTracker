using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class PersonController : ControllerBase
{
    [HttpPost]
    public IActionResult CreatePerson(PersonModel person)
    {
        GlobalConfig.connection.SaveNewPerson(person);
        return StatusCode(StatusCodes.Status201Created, person);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var personList = GlobalConfig.connection.GetPerson_All().ToList();
        
        if (personList.Count == 0)
        {
            return NotFound();
        }
        else
        {
            return Ok(personList);
        }
    }
}
