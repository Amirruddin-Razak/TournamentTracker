using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Track all entry for current matchup
    /// </summary>
    public class MatchupModel
    {
        /// <summary>
        /// List of all individual in current matchup
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();

        /// <summary>
        /// Track the winner of the current matchup
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// Track the round number of the current matchup
        /// </summary>
        public int MatchupRound { get; set; }

    }
}
