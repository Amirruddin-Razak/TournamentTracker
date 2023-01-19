using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerUI.Library.Api.Helper;
using TrackerUI.Library.Models;

namespace TrackerUI.Library.Api;

public class TeamEndpoint : ITeamEndpoint
{
    private readonly IApiConnector _apiConnector;

    public TeamEndpoint(IApiConnector apiConnector)
    {
        _apiConnector = apiConnector;
    }

    public async Task<List<TeamModel>> GetAllTeamAsync()
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.GetAsync("Team/GetAllTeam");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<TeamModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            // TODO add log
            throw;
        }
    }
}
