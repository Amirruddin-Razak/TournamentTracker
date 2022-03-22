using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Track each individual matchup entry
    /// </summary>
    public class MatchupEntryModel
    {
        /// <summary>
        /// Track the Matchup Entry id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Use by database to lookup the competing team
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Track team competing in this matchup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Track score of the team competing in the matchup
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Use by database to lookup the ParentMatchup
        /// </summary>
        public int ParentMatchupId { get; set; }

        /// <summary>
        /// Track the matchup from previous round that lead to this matchup
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }

    }
}
