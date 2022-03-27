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
            return $"{GlobalConfig.AppKeyLookup("filePath") }\\{ fileName }";
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
                tournamentModel.Rounds.AddRange(cols[5].Split('|').Select(r => FindMatchupById(r)));
                tournamentModel.Active = bool.Parse(cols[6]);

                output.Add(tournamentModel);
            }

            return output;
        }

        public static List<MatchupModel> ConvertTextToMatchupModel(this List<string> lines)
        {
            List<MatchupModel> output = new List<MatchupModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupModel model = new MatchupModel();
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
                string[] cols = line.Split(',');

                MatchupEntryModel model = new MatchupEntryModel();
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

        public static void SaveToPersonFile(this List<PersonModel> models)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.PhoneNumber }");
            }

            File.WriteAllLines(GlobalConfig.PersonFileName.FullFilePath(), lines);
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PrizeName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(GlobalConfig.PrizeFileName.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models) 
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertModelListToString(t.TeamMembers, '|') }");
            }

            File.WriteAllLines(GlobalConfig.TeamFileName.FullFilePath(), lines);
        }

        public static void SaveToTournamentFile(this List<TournamentModel> models)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                lines.Add($"{ tm.Id },{ tm.TournamentName },{ tm.EntryFee },{ ConvertModelListToString(tm.TeamList, '|') }," +
                    $"{ ConvertModelListToString(tm.Prizes, '|') },{ ConvertRoundToString(tm.Rounds) },{ tm.Active }");
            }

            File.WriteAllLines(GlobalConfig.TournamentFileName.FullFilePath(), lines);
        }

        public static void SaveToMatchupFile(this List<MatchupModel> models) 
        {
            List<string> lines = new List<string>();

            foreach (MatchupModel m in models)
            {
                string winnerId = "";
                if (m.Winner != null)
                {
                    winnerId = $"{ m.Winner.Id }";
                }
                
                lines.Add($"{ m.Id },{ ConvertModelListToString(m.Entries, '|') },{ winnerId },{ m.MatchupRound }");
            }

            File.WriteAllLines(GlobalConfig.MatchupFileName.FullFilePath(), lines);
        }

        public static void SaveToMatchupEntryFile(this List<MatchupEntryModel> models) 
        {
            List<string> lines = new List<string>();

            foreach (MatchupEntryModel entry in models)
            {
                string teamId = "";
                if (entry.TeamCompeting != null)
                {
                    teamId = $"{ entry.TeamCompeting.Id }";
                }

                string parentId = "";
                if (entry.ParentMatchup != null)
                {
                    parentId = $"{ entry.ParentMatchup.Id }";
                }

                lines.Add($"{ entry.Id },{ teamId },{ entry.Score },{ parentId }");
            }

            File.WriteAllLines(GlobalConfig.MatchupEntryFileName.FullFilePath(), lines);
        }

        private static string ConvertRoundToString(List<List<MatchupModel>> round)
        {
            StringBuilder builder = new StringBuilder();
            string output = "";

            if (round == null || round.Count == 0)
            {
                return output;
            }
            
            foreach (List<MatchupModel> member in round)
            {
                builder.Append(ConvertModelListToString(member, '^'));
                builder.Append('|');
            }

            output = builder.Remove(builder.Length - 1, 1).ToString();

            return output;
        }

        private static string ConvertModelListToString<T>(List<T> models, char delimiter) where T : IDataModel
        {
            StringBuilder builder = new StringBuilder();
            string output = "";

            if (models == null || models.Count == 0)
            {
                return output;
            }

            foreach (T model in models)
            {
                builder.Append(model.Id);
                builder.Append(delimiter);
            }

            output = builder.Remove(builder.Length - 1, 1).ToString();

            return output;
        }

        private static List<PersonModel> FindPersonById(string idString)
        {
            List<string> matchedPeople = FindById(GlobalConfig.PersonFileName.FullFilePath().LoadFile(), idString);

            List<PersonModel> output = matchedPeople.ConvertTextToPersonModel();

            return output.Count == 0 ? null : output;
        }

        private static List<TeamModel> FindTeamById(string idString)
        {
            List<string> matchedTeam = FindById(GlobalConfig.TeamFileName.FullFilePath().LoadFile(), idString);

            List<TeamModel> output = matchedTeam.ConvertTextToTeamModel();

            return output.Count == 0 ? null : output;
        }

        private static List<PrizeModel> FindPrizeById(string idString)
        {
            List<string> matchedPrize = FindById(GlobalConfig.PrizeFileName.FullFilePath().LoadFile(), idString);

            List<PrizeModel> output = matchedPrize.ConvertTextToPrizeModel();

            return output.Count == 0 ? null : output;
        }

        private static List<MatchupModel> FindMatchupById(string idString)
        {
            List<string> matchedMatchup = FindById(GlobalConfig.MatchupFileName.FullFilePath().LoadFile(), idString);

            List<MatchupModel> output = matchedMatchup.ConvertTextToMatchupModel();

            return output.Count == 0 ? null : output;
        }

        private static List<MatchupEntryModel> FindMatchupEntryById(string idString)
        {
            List<string> matchedMatchupEntry = FindById(GlobalConfig.MatchupEntryFileName.FullFilePath().LoadFile(), idString);

            List<MatchupEntryModel> output = matchedMatchupEntry.ConvertTextToMatchupEntryModel();

            return output.Count == 0 ? null : output;
        }

        private static List<string> FindById(List<string> modelsText, string idString)
        {
            List<string> output = new List<string>();

            string[] ids = idString.Split(new[] { '|', '^' });

            foreach (string id in ids)
            {
                foreach (string line in modelsText)
                {
                    string[] cols = line.Split(',');

                    if (cols[0] == id)
                    {
                        output.Add(line);
                    }
                }
            }

            return output;
        }
    }
}
