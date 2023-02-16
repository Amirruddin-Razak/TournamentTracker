using TrackerUI.Library.Models;

namespace TrackerWinFormUI.Interface;

public interface ITeamRequestor
{
    void NewTeamComplete(TeamModel team);
}
