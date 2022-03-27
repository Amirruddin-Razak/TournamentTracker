using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Track each team data
    /// </summary>
    public class TeamModel : IDataModel
    {
        /// <summary>
        /// Store the team id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Store the name of the team
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Track each member of the team
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
    }
}
