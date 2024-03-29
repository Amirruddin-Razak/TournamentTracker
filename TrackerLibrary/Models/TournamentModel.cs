﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Track the whole tournament detail
    /// </summary>
    public class TournamentModel : IDataModel
    {
        public event EventHandler<DateTime> OnTournamentComplete;

        /// <summary>
        /// Track the tournament id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Store the name of the tournament
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// Record the entry fee for each team to join the tournament 
        /// </summary>
        public decimal EntreeFee { get; set; }

        /// <summary>
        /// Track all team that join the tournament
        /// </summary>
        public List<TeamModel> TeamList { get; set; } = new List<TeamModel>();

        /// <summary>
        /// Track all prize set for the tournament
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// Track all match to be played in this tournament
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();

        /// <summary>
        /// Track if tournament is active
        /// </summary>
        public bool Active { get; set; } = true;

        public void CompleteTournament() 
        {
            OnTournamentComplete?.Invoke(this, DateTime.Now);
        }
    }
}
