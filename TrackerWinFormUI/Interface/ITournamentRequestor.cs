using TrackerUI.Library.Models;

namespace TrackerWinFormUI.Interface;

public interface ITournamentRequestor
{
    void NewTournamentComplete(TournamentModel tournament);
}
