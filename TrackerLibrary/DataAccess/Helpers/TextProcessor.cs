using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.Helpers
{
    public static class TextProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PersonModel> ConvertTextToPersonModel(this List<string> lines) 
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PersonModel person = new PersonModel();
                person.Id = int.Parse(cols[0]);
                person.FirstName = cols[1];
                person.LastName = cols[2];
                person.EmailAddress = cols[3];
                person.PhoneNumber = cols[4];

                output.Add(person);
            }

            return output;
        }

        public static List<PrizeModel> ConvertTextToPrizeModel(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel prize = new PrizeModel();
                prize.Id = int.Parse(cols[0]);
                prize.PlaceNumber = int.Parse(cols[1]);
                prize.PrizeName = (cols[2]);
                prize.PrizeAmount = decimal.Parse(cols[3]);
                prize.PrizePercentage = float.Parse(cols[4]);

                output.Add(prize);
            }

            return output;
        }

        public static List<TeamModel> ConvertTextToTeamModel(this List<string> lines)
        {
            List<TeamModel> output = new List<TeamModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel team = new TeamModel();
                team.Id = int.Parse(cols[0]);
                team.TeamName = cols[1];
                team.TeamMembers = FindPersonById(cols[2]);

                output.Add(team);
            }

            return output;
        }

        public static List<TournamentModel> ConvertTextToTournamentModel(this List<string> lines)
        {
            List<TournamentModel> output = new List<TournamentModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TournamentModel tournamentModel = new TournamentModel();
                tournamentModel.Id = int.Parse(cols[0]);
                tournamentModel.TournamentName = cols[1];
                tournamentModel.EntryFee = decimal.Parse(cols[2]);

                tournamentModel.TeamList = FindTeamById(cols[3]);

                tournamentModel.Prizes = FindPrizeById(cols[4]);

                string[] round = cols[5].Split('|');

                foreach (string r in round)
                {
                    tournamentModel.Rounds.Add(FindMatchupById(r));
                }

                output.Add(tournamentModel);
            }

            return output;
        }

        public static List<MatchupModel> ConvertTextToMatchupModel(this List<string> lines)
        {
            List<MatchupModel> output = new List<MatchupModel>();

            foreach (string line in lines)
            {
                MatchupModel model = new MatchupModel();

                string[] cols = line.Split(',');
                model.Id = int.Parse(cols[0]);

                model.Entries = FindMatchupEntryById(cols[1]);

                model.Winner = FindTeamById(cols[2])?.First();

                model.MatchupRound = int.Parse(cols[3]);

                output.Add(model);
            }

            return output;
        }

        public static List<MatchupEntryModel> ConvertTextToMatchupEntryModel(this List<string> lines)
        {
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();

            foreach (string line in lines)
            {
                MatchupEntryModel model = new MatchupEntryModel();

                string[] cols = line.Split(',');

                model.Id = int.Parse(cols[0]);

                model.TeamCompeting = FindTeamById(cols[1])?.First();

                if (cols[2] != "")
                {
                    model.Score = double.Parse(cols[2]);
                }

                model.ParentMatchup = FindMatchupById(cols[3])?.First();

                output.Add(model);
            }

            return output;
        }

        public static void SaveToPersonFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.PhoneNumber }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PrizeName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName) 
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPersonListToString(t.TeamMembers) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTournamentFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                lines.Add($"{ tm.Id },{ tm.TournamentName },{ tm.EntryFee },{ ConvertTeamListToString(tm.TeamList) }," +
                    $"{ ConvertPrizeListToString(tm.Prizes) },{ ConvertRoundListToString(tm.Rounds) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToMatchupFile(this List<MatchupModel> models, string fileName) 
        {
            List<string> lines = new List<string>();

            foreach (MatchupModel m in models)
            {
                string winnerId = "";
                if (m.Winner != null)
                {
                    winnerId = $"{ m.Winner.Id }";
                }
                
                lines.Add($"{ m.Id },{ ConvertEntryListToString(m.Entries) },{ winnerId },{ m.MatchupRound }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToMatchupEntryFile(this List<MatchupEntryModel> models, string fileName) 
        {
            List<string> lines = new List<string>();

            foreach (MatchupEntryModel entry in models)
            {
                string teamId = "";
                string parentId = "";

                if (entry.TeamCompeting != null)
                {
                    teamId = $"{ entry.TeamCompeting.Id }";
                }

                if (entry.ParentMatchup != null)
                {
                    parentId = $"{ entry.ParentMatchup.Id }";
                }

                lines.Add($"{ entry.Id },{ teamId },{ entry.Score },{ parentId }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertEntryListToString(List<MatchupEntryModel> models) 
        {
            string output = "";

            if (models.Count == 0)
            {
                return output;
            }

            foreach (MatchupEntryModel entry in models)
            {
                output += $"{ entry.Id }|";
            }

            output = output.Remove(output.Length - 1, 1);

            return output;
        }

        private static string ConvertPersonListToString(List<PersonModel> models) 
        {
            string output = "";

            if (models.Count == 0)
            {
                return output;
            }

            foreach (PersonModel member in models)
            {
                output += $"{ member.Id }|";
            }

            output = output.Remove(output.Length - 1, 1);

            return output;
        }

        private static string ConvertTeamListToString(List<TeamModel> models)
        {
            string output = "";

            if (models.Count == 0)
            {
                return output;
            }

            foreach (TeamModel member in models)
            {
                output += $"{ member.Id }|";
            }

            output = output.Remove(output.Length - 1, 1);

            return output;
        }

        private static string ConvertPrizeListToString(List<PrizeModel> models)
        {
            string output = "";

            if (models == null || models.Count == 0)
            {
                return output;
            }

            foreach (PrizeModel member in models)
            {
                output += $"{ member.Id }|";
            }

            output = output.Remove(output.Length - 1, 1);

            return output;
        }

        private static string ConvertRoundListToString(List<List<MatchupModel>> round)
        {
            string output = "";

            if (round.Count == 0)
            {
                return output;
            }

            foreach (List<MatchupModel> member in round)
            {
                output += $"{ ConvertMatchupListToString(member) }|";
            }

            output = output.Remove(output.Length - 1, 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<MatchupModel> models)
        {
            string output = "";

            if (models.Count == 0)
            {
                return output;
            }

            foreach (MatchupModel member in models)
            {
                output += $"{ member.Id }^";
            }

            output = output.Remove(output.Length - 1, 1);

            return output;
        }

        private static List<PersonModel> FindPersonById(string idString)
        {
            List<string> peopleText = GlobalConfig.PersonFileName.FullFilePath().LoadFile();
            List<string> matchedPeople = new List<string>();

            string[] ids = idString.Split('|');

            foreach (string id in ids)
            {
                foreach (string line in peopleText)
                {
                    string[] cols = line.Split(',');

                    if (cols[0] == id)
                    {
                        matchedPeople.Add(line);
                    }
                }
            }

            List<PersonModel> output = matchedPeople.ConvertTextToPersonModel();

            return output.Count == 0 ? null : output;
        }

        private static List<TeamModel> FindTeamById(string idString)
        {
            List<string> teamText = GlobalConfig.TeamFileName.FullFilePath().LoadFile();
            List<string> matchedTeam = new List<string>();

            string[] ids = idString.Split('|');

            foreach (string id in ids)
            {
                foreach (string line in teamText)
                {
                    string[] cols = line.Split(',');

                    if (cols[0] == id)
                    {
                        matchedTeam.Add(line);
                    }
                }
            }

            List<TeamModel> output = matchedTeam.ConvertTextToTeamModel();

            return output.Count == 0 ? null : output;
        }

        private static List<PrizeModel> FindPrizeById(string idString)
        {
            List<string> prizeText = GlobalConfig.PrizeFileName.FullFilePath().LoadFile();
            List<string> matchedPrize = new List<string>();

            string[] ids = idString.Split('|');

            foreach (string id in ids)
            {
                foreach (string line in prizeText)
                {
                    string[] cols = line.Split(',');

                    if (cols[0] == id)
                    {
                        matchedPrize.Add(line);
                    }
                }
            }

            List<PrizeModel> output = matchedPrize.ConvertTextToPrizeModel();

            return output.Count == 0 ? null : output;
        }

        private static List<MatchupModel> FindMatchupById(string idString)
        {
            List<string> matchupText = GlobalConfig.MatchupFileName.FullFilePath().LoadFile();
            List<string> matchedMatchup = new List<string>();

            string[] ids = idString.Split('^');

            foreach (string id in ids)
            {
                foreach (string line in matchupText)
                {
                    string[] cols = line.Split(',');

                    if (cols[0] == id)
                    {
                        matchedMatchup.Add(line);
                    }
                }
            }

            List<MatchupModel> output = matchedMatchup.ConvertTextToMatchupModel();

            return output.Count == 0 ? null : output;
        }

        private static List<MatchupEntryModel> FindMatchupEntryById(string idString)
        {
            List<string> matchupEntryText = GlobalConfig.MatchupEntryFileName.FullFilePath().LoadFile();
            List<string> matchedMatchupEntry = new List<string>();

            string[] ids = idString.Split('|');

            foreach (string id in ids)
            {
                foreach (string line in matchupEntryText)
                {
                    string[] cols = line.Split(',');

                    if (cols[0] == id)
                    {
                        matchedMatchupEntry.Add(line);
                    }
                }
            }

            List<MatchupEntryModel> output = matchedMatchupEntry.ConvertTextToMatchupEntryModel();

            return output.Count == 0 ? null : output;
        }
    }
}
