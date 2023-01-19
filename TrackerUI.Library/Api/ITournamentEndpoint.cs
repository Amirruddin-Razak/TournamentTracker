using TrackerUI.Library.Models;

namespace TrackerUI.Library.Api;

public interface ITournamentEndpoint
{
    Task<List<TournamentModel>> GetActiveTournamentAsync();
    Task UpdateTournamentResultAsync(TournamentModel tournament);
    Task CreateTournamentAsync(TournamentModel tournament);
}