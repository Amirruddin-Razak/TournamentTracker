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
        /// Track team competing in this matchup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Track score of the team competing in the matchup
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Track the matchup from previous round that lead to this matchup
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }

    }
}
