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

        public static void SaveToPersonFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.PhoneNumber }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
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

        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PrizeName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<TeamModel> ConvertTextToTeamModel(this List<string> lines, string PersonFileName) 
        {
            List<TeamModel> output = new List<TeamModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel team = new TeamModel();
                team.Id = int.Parse(cols[0]);
                team.TeamName = cols[1];

                string[] membersId = cols[2].Split('|');

                team.TeamMembers = GetMemberById(membersId, PersonFileName);

                output.Add(team);
            }

            return output;
        }

        private static List<PersonModel> GetMemberById(string[] membersId, string PersonFileName)
        {
            List<PersonModel> output = new List<PersonModel>();
            List<PersonModel> people = PersonFileName.FullFilePath().LoadFile().ConvertTextToPersonModel();

            foreach (string id in membersId)
            {
                output.Add(people.Find(x => x.Id == int.Parse(id)));
            }

            return output;
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName) 
        {
            List<string> lines = new List<string>();

            foreach (TeamModel team in models)
            {
                lines.Add($"{ team.Id },{ team.TeamName },{ ConvertPersonListToString(team.TeamMembers) }");
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
    }
}
