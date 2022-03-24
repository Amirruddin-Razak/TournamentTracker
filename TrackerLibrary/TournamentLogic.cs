using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateNewTournament(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.TeamList);

            CalculateRoundAndByes(randomizedTeams.Count, out int numOfRound, out int byes);

            model.Rounds.Add(CreateFirstRound(randomizedTeams, byes));

            for (int i = 1; i < numOfRound; i++)
            {
                model.Rounds.Add(CreateNextRound(model.Rounds[i - 1]));
            }

            model.Prizes.ToList().ForEach(x => GlobalConfig.connection.SaveNewPrize(x));

            GlobalConfig.connection.SaveNewTournament(model);

            model.AlertUserToNewRound();

            //Update result for matchup with byes in tournament
            UpdateTournamentResult(model);
        }

        public static void UpdateTournamentResult(TournamentModel model)
        {
            int startingRound = model.CheckCurrentRound();
            List<MatchupModel> MatchupsToScore = new List<MatchupModel>();

            foreach (List<MatchupModel> r in model.Rounds)
            {
                foreach (MatchupModel m in r)
                {
                    if (m.Winner == null && (m.Entries.Any(x => x.Score != 0) || m.Entries.Count == 1))
                    {
                        MatchupsToScore.Add(m);
                    }
                }
            }

            MarkMatchupWinner(MatchupsToScore);

            AdvanceWinner(MatchupsToScore, model);

            MatchupsToScore.ForEach(x => GlobalConfig.connection.UpdateMatchup(x));

            int endingRound = model.CheckCurrentRound();

            if (endingRound > startingRound)
            {
                model.AlertUserToNewRound();
            }
        }

        private static void AlertUserToNewRound(this TournamentModel model)
        {
            int currentRoundNumber = model.CheckCurrentRound();

            List<MatchupModel> currentRound = model.Rounds.Find(x => x.First().MatchupRound == currentRoundNumber);

            foreach (MatchupModel matchup in currentRound)
            {
                foreach (MatchupEntryModel entry in matchup.Entries)
                {
                    AlertPersonToNewRound(entry.TeamCompeting, matchup.Entries.Find(x => x.TeamCompeting != entry.TeamCompeting));
                }
            }
        }

        private static void AlertPersonToNewRound(TeamModel model, MatchupEntryModel competitor)
        {
            List<string> toAddress = new List<string>();

            model.TeamMembers.ForEach(x => toAddress.Add(x.EmailAddress));

            string subject;
            StringBuilder body = new StringBuilder();

            if (competitor == null)
            {
                subject = $"Your team : {model.TeamName} get a bye this round";

                body.AppendLine("<h1>You Get A Bye</h1>");
                body.AppendLine("See you in next round");
                body.AppendLine();
                body.Append("~Tournament Tracker~");
            }
            else
            {
                subject = $"Your team : {model.TeamName} has a new matchup with { competitor.TeamCompeting.TeamName }";

                body.AppendLine("<h1>Your team has a new matchup</h1>");
                body.Append("<strong> your opponent is : </strong>");
                body.AppendLine($"{ competitor.TeamCompeting.TeamName }");
                body.AppendLine();
                body.AppendLine();
                body.Append("~Tournament Tracker~");
            }

            EmailLogic.SendEmail(toAddress, new List<string>(), subject, body.ToString());
        }

        private static int CheckCurrentRound(this TournamentModel model)
        {
            int currentRoundNumber = 1;

            foreach (List<MatchupModel> round in model.Rounds)
            {
                if (round.All(x => x.Winner != null))
                {
                    currentRoundNumber += 1;
                }
                else
                {
                    return currentRoundNumber;
                }
            }

            CompleteTournament(model);
            return currentRoundNumber - 1;
        }

        private static void CompleteTournament(TournamentModel model)
        {
            GlobalConfig.connection.CompleteTournament(model);

            TeamModel winner = model.Rounds.Last().First().Winner;
            TeamModel runnerUp = model.Rounds.Last().First().Entries.Find(x => x.TeamCompeting.Id != winner.Id).TeamCompeting;

            decimal winnerPrize = 0;
            decimal runnerUpPrize = 0;

            if (model.Prizes.Count > 0)
            {
                decimal totalIncome = model.TeamList.Count * model.EntryFee;

                PrizeModel firstPlacePrize = model.Prizes.Find(x => x.PlaceNumber == 1);
                PrizeModel secondPlacePrize = model.Prizes.Find(x => x.PlaceNumber == 2);

                if (firstPlacePrize != null)
                {
                    winnerPrize = firstPlacePrize.CalculatePrize(totalIncome);
                }
                if (secondPlacePrize != null)
                {
                    runnerUpPrize = secondPlacePrize.CalculatePrize(totalIncome);
                }
            }

            AlertPersonToTournamentEnd(model, winner, runnerUp, winnerPrize, runnerUpPrize);
        }

        private static void AlertPersonToTournamentEnd(TournamentModel model, TeamModel winner, TeamModel runnerUp, decimal winnerPrize, decimal runnerUpPrize)
        {
            List<string> bccAddress = new List<string>();

            model.TeamList.ForEach(x => x.TeamMembers.ForEach(y => bccAddress.Add(y.EmailAddress)));

            string subject = $"team { winner.TeamName } has won the { model.TournamentName } tournament";

            StringBuilder body = new StringBuilder();
            body.AppendLine("<h1>Tournament Ended</h1>");
            body.AppendLine($"<strong>{ winner.TeamName } has won the tournament</strong>");
            body.AppendLine("<br />");

            if (winnerPrize > 0)
            {
                body.AppendLine($"The winner, team { winner.TeamName } will recieve a prize of ${ winnerPrize }");
            }
            if (runnerUpPrize > 0)
            {
                body.AppendLine($"The runner up, team { runnerUp.TeamName } will recieve a prize of ${ runnerUpPrize }");
            }

            body.AppendLine("<br />");
            body.AppendLine("Thank you to all participating team. See you again in the next tournament");
            body.AppendLine("<br />");
            body.AppendLine("<br />");
            body.AppendLine("~Tournament Tracker~");

            EmailLogic.SendEmail(new List<string>(), bccAddress, subject, body.ToString());

            model.CompleteTournament();
        }

        public static decimal CalculatePrize(this PrizeModel prize, decimal totalIncome)
            => decimal.Add(prize.PrizeAmount, decimal.Multiply(totalIncome, Convert.ToDecimal(prize.PrizePercentage / 100)));

        private static void AdvanceWinner(List<MatchupModel> models, TournamentModel tournament) 
        {
            foreach (MatchupModel m in models)
            {
                foreach (List<MatchupModel> round in tournament.Rounds)
                {
                    foreach (MatchupModel rm in round)
                    {
                        foreach (MatchupEntryModel me in rm.Entries)
                        {
                            if (me.ParentMatchup != null && me.ParentMatchup.Id == m.Id)
                            {
                                me.TeamCompeting = m.Winner;
                                GlobalConfig.connection.UpdateMatchup(rm);
                            }
                        }
                    }
                }
            }
        }

        private static void MarkMatchupWinner(List<MatchupModel> models)
        {
            bool HighScoreWin = bool.Parse(GlobalConfig.AppKeyLookup("highScoreWin"));

            foreach (MatchupModel m in models)
            {
                // set winner for matchup with bye
                if (m.Entries.Count == 1)
                {
                    m.Winner = m.Entries[0].TeamCompeting;
                    continue;
                }

                if (HighScoreWin)
                {
                    if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if(m.Entries[1].Score > m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("Tie match are not allowed");
                    }
                }
                else
                {
                    if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if(m.Entries[1].Score < m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("Tie match are not allowed");
                    }
                } 
            }
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
