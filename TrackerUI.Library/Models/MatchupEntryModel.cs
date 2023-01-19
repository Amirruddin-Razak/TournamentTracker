using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerUI.Library.Models
{
    public class MatchupEntryModel : IDataModel
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public TeamModel TeamCompeting { get; set; }
        public double Score { get; set; }
        public int ParentMatchupId { get; set; }
        public MatchupModel ParentMatchup { get; set; }

        public MatchupEntryModel()
        {

        }

        public MatchupEntryModel(TrackerLibrary.Models.MatchupEntryModel model)
        {
            if (model == null)
            {
                return;
            }

            Id = model.Id;
            TeamId = model.TeamId;
            TeamCompeting = new TeamModel(model.TeamCompeting);
            Score = model.Score;
            ParentMatchupId = model.ParentMatchupId;
            ParentMatchup = new MatchupModel(model.ParentMatchup);
        }
    }
}
