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
        public void SaveNewPerson(PersonModel model)
        {
            List<PersonModel> people = GlobalConfig.PersonFileName.FullFilePath().LoadFile().ConvertTextToPersonModel();

            int currentId = 1;
            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            people.Add(model);

            people.SaveToPersonFile();
        }

        public void SaveNewPrize(PrizeModel model)
        {
            List<PrizeModel> prizes = GlobalConfig.PrizeFileName.FullFilePath().LoadFile().ConvertTextToPrizeModel();

            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            prizes.Add(model);

            prizes.SaveToPrizeFile();
        }

        public void SaveNewTeam(TeamModel model)
        {
            List<TeamModel> teams = GlobalConfig.TeamFileName.FullFilePath().LoadFile().ConvertTextToTeamModel();

            int currentId = 1;
            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            teams.Add(model);

            teams.SaveToTeamFile();
        }

        public void SaveNewTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFileName.FullFilePath().LoadFile().ConvertTextToTournamentModel();

            int currentId = 1;
            if (tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;

            SaveNewMatchup(model);

            tournaments.Add(model);

            tournaments.SaveToTournamentFile();
        }

        private void SaveNewMatchup(TournamentModel model)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFileName.FullFilePath().LoadFile().ConvertTextToMatchupModel();

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

                    SaveNewMatchupEntry(m);

                    matchups.Add(m);
                }
            }

            matchups.SaveToMatchupFile();
        }

        private void SaveNewMatchupEntry(MatchupModel model)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFileName.FullFilePath().LoadFile().ConvertTextToMatchupEntryModel();

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

            entries.SaveToMatchupEntryFile();
        }

        public List<PersonModel> GetPerson_All()
        {
            return GlobalConfig.PersonFileName.FullFilePath().LoadFile().ConvertTextToPersonModel();
        }

        public List<TeamModel> GetTeam_All()
        {
            return GlobalConfig.TeamFileName.FullFilePath().LoadFile().ConvertTextToTeamModel();
        }

        public List<TournamentModel> GetTournament_All()
        {
            return GlobalConfig.TournamentFileName.FullFilePath().LoadFile().ConvertTextToTournamentModel();
        }

        public void UpdateMatchupWinner(MatchupModel model)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFileName.FullFilePath().LoadFile().ConvertTextToMatchupModel();

            int currIndex = matchups.FindIndex(x => x.Id == model.Id);
            matchups[currIndex] = model;

            UpdateMatchupEntry(model);

            matchups.SaveToMatchupFile();
        }

        private void UpdateMatchupEntry(MatchupModel model)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFileName
                .FullFilePath()
                .LoadFile()
                .ConvertTextToMatchupEntryModel();

            foreach (MatchupEntryModel entry in model.Entries)
            {
                int currIndex = entries.FindIndex(x => x.Id == entry.Id);
                entries[currIndex] = entry;
            }

            entries.SaveToMatchupEntryFile();
        }

        public void UpdateTournamentComplete(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFileName.FullFilePath().LoadFile().ConvertTextToTournamentModel();

            model.Active = false;
            int currIndex = tournaments.FindIndex(x => x.Id == model.Id);

            tournaments[currIndex] = model;

            tournaments.SaveToTournamentFile();
        }
    }
}
