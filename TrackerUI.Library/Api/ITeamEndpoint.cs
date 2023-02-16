using TrackerUI.Library.Models;

namespace TrackerUI.Library.Api;

public interface ITeamEndpoint
{
    Task<List<TeamModel>> GetAllTeamAsync();
    Task<TeamModel> CreateTeamAsync(TeamModel team);
}