using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerUI.Library.Api.Helper;
using TrackerUI.Library.Models;

namespace TrackerUI.Library.Api;

public class TournamentEndpoint : ITournamentEndpoint
{
    private readonly IApiConnector _apiConnector;

    public TournamentEndpoint(IApiConnector apiConnector)
    {
        _apiConnector = apiConnector;
    }

    public async Task<List<TournamentModel>> GetActiveTournamentAsync()
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.GetAsync("Tournament/GetActiveTournament");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<TournamentModel>>();
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

    public async Task UpdateTournamentResultAsync(TournamentModel tournament)
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.PostAsJsonAsync("Tournament/UpdateTournamentResult", tournament);

            if (!response.IsSuccessStatusCode)
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
