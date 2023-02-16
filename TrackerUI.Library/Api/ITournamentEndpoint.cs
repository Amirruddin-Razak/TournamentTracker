using TrackerUI.Library.Models;

namespace TrackerUI.Library.Api;

public interface ITournamentEndpoint
{
    Task<List<TournamentModel>> GetActiveTournamentAsync();
    Task<TournamentModel> UpdateTournamentResultAsync(TournamentModel tournament);
    Task<TournamentModel> CreateTournamentAsync(TournamentModel tournament);
}