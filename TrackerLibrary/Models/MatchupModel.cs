﻿using System;
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
        /// Track the Matchup id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// List of all individual in current matchup
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();

        /// <summary>
        /// Used by database to lookup winner
        /// </summary>
        public int WinnerId { get; set; }

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
