using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerWPF.Models
{
    public class MatchupDisplayModel
    {
        public int MatchupId { get; set; }
        public string FirstTeamName { get; set; }
        public double FirstTeamScore { get; set; }
        public string SecondTeamName { get; set; }
        public double SecondTeamScore { get; set; }
    }
}
