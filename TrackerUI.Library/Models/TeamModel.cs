using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerUI.Library.Models
{
    public class TeamModel : IDataModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

        public TeamModel()
        {

        }

        public TeamModel(TrackerLibrary.Models.TeamModel model)
        {
            if (model == null)
            {
                return;
            }

            Id = model.Id;
            TeamName = model.TeamName;
            TeamMembers = model.TeamMembers.Select(member => new PersonModel(member)).ToList();
        }
    }
}
