using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRound(TournamentModel model) 
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.TeamList);

            CalculateRoundAndByes(randomizedTeams.Count, out int numOfRound, out int byes);

            model.Rounds.Add(CreateFirstRound(randomizedTeams, byes));

            for (int i = 1; i < numOfRound; i++)
            {
                model.Rounds.Add(CreateNextRound(model.Rounds[i - 1]));
            }
        }

        private static List<MatchupModel> CreateNextRound(List<MatchupModel> previousRound)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel matchup = new MatchupModel();

            foreach (MatchupModel parentMatchup in previousRound)
            {
                matchup.Entries.Add(new MatchupEntryModel() { ParentMatchup = parentMatchup });

                if (matchup.Entries.Count > 1)
                {
                    matchup.MatchupRound = parentMatchup.MatchupRound + 1;
                    output.Add(matchup);
                    matchup = new MatchupModel();
                }
            }

            return output;
        }

        private static List<MatchupModel> CreateFirstRound(List<TeamModel> teams, int byes)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel matchup = new MatchupModel();

            foreach (TeamModel t in teams)
            {
                matchup.Entries.Add(new MatchupEntryModel() { TeamCompeting = t });

                if (byes > 0 || matchup.Entries.Count > 1)
                {
                    matchup.MatchupRound = 1;
                    output.Add(matchup);
                    matchup = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static void CalculateRoundAndByes(int teamCount, out int numOfRound, out int byes)
        {
            int bracketSize = 2;
            numOfRound = 1;

            while (bracketSize < teamCount)
            {
                numOfRound += 1;
                bracketSize *= 2;
            }

            byes = bracketSize - teamCount;
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> models)
        {
            Random rang = new Random();      
            List<TeamModel> randomizedTeam = new List<TeamModel>();

            int[] drawBox = Enumerable.Range(0, models.Count).ToArray();

            for (int i = models.Count - 1; i >= 0; i--)
            {
                int drawIdx = rang.Next(i);
                randomizedTeam.Add(models[drawBox[drawIdx]]);
                drawBox[drawIdx] = drawBox[i];
            }

            return randomizedTeam;
        }
    }
}
