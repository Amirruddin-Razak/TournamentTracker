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

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsAsync<List<TournamentModel>>();
                return result;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<TournamentModel>();
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

    public async Task<TournamentModel> UpdateTournamentResultAsync(TournamentModel tournament)
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.PostAsJsonAsync("Tournament/UpdateTournamentResult", tournament);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<TournamentModel>();
                return result;
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

    public async Task<TournamentModel> CreateTournamentAsync(TournamentModel tournament)
    {
        try
        {
            using HttpResponseMessage response = await _apiConnector.ApiClient.PostAsJsonAsync("Tournament/CreateTournament", tournament);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var result = await response.Content.ReadAsAsync<TournamentModel>();
                return result;
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
