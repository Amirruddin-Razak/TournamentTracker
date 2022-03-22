using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.Helpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PersonFileName = "PersonModels.csv";
        private const string PrizeFileName = "PrizeModels.csv";
        private const string TeamFileName = "TeamModels.csv";
        private const string MatchupFileName = "MatchupModels.csv";
        private const string MatchupEntryFileName = "MatchupEntryModels.csv";
        private const string TournamentFileName = "TournamentModels.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> people = PersonFileName.FullFilePath().LoadFile().ConvertTextToPersonModel();

            int currentId = 1;
            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            people.Add(model);

            people.SaveToPersonFile(PersonFileName);

            return model;
        }

        /// <summary>
        /// Save new prize to text
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizeFileName.FullFilePath().LoadFile().ConvertTextToPrizeModel();

            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            prizes.Add(model);

            prizes.SaveToPrizeFile(PrizeFileName);

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = TeamFileName.FullFilePath().LoadFile().ConvertTextToTeamModel();

            int currentId = 1;
            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            teams.Add(model);

            teams.SaveToTeamFile(TeamFileName);

            return model;
        }

        public TournamentModel CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = TournamentFileName.FullFilePath().LoadFile().ConvertTextToTournamentModel();

            int currentId = 1;
            if (tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            CreateMatchup(model);

            tournaments.Add(model);

            tournaments.SaveToTournamentFile(TournamentFileName);

            return model;
        }

        private void CreateMatchup(TournamentModel model) 
        {
            List<MatchupModel> matchups = MatchupFileName.FullFilePath().LoadFile().ConvertTextToMatchupModel();

            int currentId = 1;
            if (matchups.Count > 0)
            {
                currentId = matchups.OrderByDescending(x => x.Id).First().Id + 1;
            }

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel m in round)
                {
                    m.Id = currentId;
                    currentId += 1;

                    CreateMatchupEntry(m);

                    matchups.Add(m);
                }
            }

            matchups.SaveToMatchupFile(MatchupFileName);
        }

        private void CreateMatchupEntry(MatchupModel model) 
        {
            List<MatchupEntryModel> entries = MatchupEntryFileName.FullFilePath().LoadFile().ConvertTextToMatchupEntryModel();

            int currentId = 1;
            if (entries.Count > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }

            foreach (MatchupEntryModel entry in model.Entries)
            {
                entry.Id = currentId;
                currentId += 1;
                entries.Add(entry);
            }

            entries.SaveToMatchupEntryFile(MatchupEntryFileName);
        }

        public List<PersonModel> GetPerson_All()
        {
            return PersonFileName.FullFilePath().LoadFile().ConvertTextToPersonModel();
        }

        public List<TeamModel> GetTeam_All()
        {
            return TeamFileName.FullFilePath().LoadFile().ConvertTextToTeamModel();
        }

        public List<TournamentModel> GetTournament_All()
        {
            return TournamentFileName.FullFilePath().LoadFile().ConvertTextToTournamentModel();
        }
    }
}
