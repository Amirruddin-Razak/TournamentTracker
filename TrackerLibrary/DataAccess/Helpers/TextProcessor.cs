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

        public static List<TeamModel> ConvertTextToTeamModel(this List<string> lines, string fileName)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = fileName.FullFilePath().LoadFile().ConvertTextToPersonModel();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel team = new TeamModel();
                team.Id = int.Parse(cols[0]);
                team.TeamName = cols[1];

                string[] membersId = cols[2].Split('|');

                foreach (string id in membersId)
                {
                    team.TeamMembers.Add(people.Find(x => x.Id == int.Parse(id)));
                }

                output.Add(team);
            }

            return output;
        }

        public static List<TournamentModel> ConvertTextToTournamentModel(this List<string> lines, string personFileName,
            string teamFileName, string prizeFileName)
        {
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertTextToTeamModel(personFileName);
            List<PrizeModel> prizes = prizeFileName.FullFilePath().LoadFile().ConvertTextToPrizeModel();

            //id,tournamentName,EntreeFee,(teamId|teamId),(prizeId|prizeId),(id^id^id|id^id^id|id^id^id)

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TournamentModel model = new TournamentModel();
                model.Id = int.Parse(cols[0]);
                model.TournamentName = cols[1];
                model.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');

                foreach (string id in teamIds)
                {
                    model.TeamList.Add(teams.Find(x => x.Id == int.Parse(id)));
                }

                string[] prizeIds = cols[4].Split('|');

                foreach (string id in prizeIds)
                {
                    model.Prizes.Add(prizes.Find(x => x.Id == int.Parse(id)));
                }

                //TODO Add GameMatch

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
                //TODO Add record for Round
                lines.Add($@"{ tm.Id },
                    { tm.TournamentName },
                    { tm.EntryFee },
                    { ConvertTeamListToString(tm.TeamList) },
                    { ConvertPrizeListToString(tm.Prizes) },
                    { ConvertRoundListToString(tm.Rounds) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
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

            if (models.Count == 0)
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
    }
}
