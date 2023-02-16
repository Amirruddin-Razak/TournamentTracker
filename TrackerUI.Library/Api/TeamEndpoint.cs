using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsAsync<List<TeamModel>>();
                return result;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<TeamModel>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        catch (Exception)
        {
            // TODO add log
            throw;
        }
    }

    public async Task<TeamModel> CreateTeamAsync(TeamModel team)
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.PostAsJsonAsync("Team/CreateTeam", team);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var createdTeam = await response.Content.ReadAsAsync<TeamModel>();
                return createdTeam;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        catch (Exception)
        {
            // TODO add log
            throw;
        }
    }
}
