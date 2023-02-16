using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerUI.Library.Models;

public class MatchupModel : IDataModel
{
    public int Id { get; set; }
    public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
    public string DisplayName
    {
        get
        {
            string output = "";

            foreach (MatchupEntryModel me in Entries)
            {
                if (me.TeamCompeting != null)
                {
                    if (output.Length == 0)
                    {
                        output = $"{ me.TeamCompeting.TeamName }";
                    }
                    else
                    {
                        output += $" vs ";
                        output += $"{ me.TeamCompeting.TeamName }";
                    }
                }
                else
                {
                    output = "Not Determined Yet";
                    break;
                }
            }

            return output;
        }
    }
    public int WinnerId { get; set; }
    public TeamModel Winner { get; set; }
    public int MatchupRound { get; set; }

    public MatchupModel()
    {

    }

    public MatchupModel(TrackerLibrary.Models.MatchupModel model)
    {
        if (model == null)
        {
            return;
        }

        Id = model.Id;
        Entries = model.Entries.Select(entry => new MatchupEntryModel(entry)).ToList();
        WinnerId = model.WinnerId;
        Winner = new TeamModel(model.Winner);
        MatchupRound = model.MatchupRound;
    }
}
