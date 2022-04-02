using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess.Internal;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        public void SaveNewPerson(PersonModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var p = new
            {
                model.FirstName,
                model.LastName,
                model.EmailAddress,
                model.PhoneNumber
            };

            sql.CreateData("dbo.spPerson_Insert", p, out int id);
            model.Id = id;
        }

        public void SaveNewPrize(PrizeModel model)
        {
            var p = new
            {
                model.PlaceNumber,
                model.PrizeName,
                model.PrizePercentage,
                model.PrizeAmount
            };

            SqlDataAccess sql = new SqlDataAccess();

            sql.CreateData("dbo.spPrize_Insert", p, out int id);
            model.Id = id;
        }

        public void SaveNewTeam(TeamModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            sql.CreateData("dbo.spTeam_Insert", new { model.TeamName }, out int id);
            model.Id = id;

            foreach (PersonModel member in model.TeamMembers)
            {
                sql.CreateData("dbo.spTeamMember_Insert", new { PersonId = member.Id, TeamId = model.Id });
            }
        }

        public void SaveNewTournament(TournamentModel model)
        {
            SaveTournament(model);

            SaveTournamentPrize(model);

            SaveTournamentEntry(model);

            SaveTournamentRound(model);
        }

        private void SaveTournament(TournamentModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            sql.CreateData("dbo.spTournament_Insert", new { model.TournamentName, model.EntreeFee }, out int id);
            model.Id = id;
        }

        private void SaveTournamentPrize(TournamentModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();
            foreach (PrizeModel prize in model.Prizes)
            {
                sql.CreateData("dbo.spTournamentPrize_Insert", new { TournamentId = model.Id, PrizeId = prize.Id });
            }
        }

        private void SaveTournamentEntry(TournamentModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            foreach (TeamModel team in model.TeamList)
            {
                sql.CreateData("dbo.spTournamentEntry_Insert", new { TournamentId = model.Id, TeamId = team.Id });
            }
        }

        private void SaveTournamentRound(TournamentModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    var p = new { TournamentId = model.Id, MatchupRound = matchup.MatchupRound };

                    sql.CreateData("dbo.spMatchup_Insert", p, out int id);
                    matchup.Id = id;

                    SaveMatchupEntry(matchup);
                }
            }
        }

        private void SaveMatchupEntry(MatchupModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            foreach (MatchupEntryModel entry in model.Entries)
            {
                var p = new { TeamId = entry.TeamCompeting?.Id, MatchupId = model.Id, ParentMatchupId = entry.ParentMatchup?.Id };

                sql.CreateData("dbo.spMatchupEntry_Insert", p, out int id);
                entry.Id = id;
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            SqlDataAccess sql = new SqlDataAccess();

            return sql.ReadData<PersonModel, dynamic>("dbo.spPerson_GetAll", new { });
        }

        public List<TeamModel> GetTeam_All()
        {
            SqlDataAccess sql = new SqlDataAccess();

            List<TeamModel> teams = sql.ReadData<TeamModel, dynamic>("dbo.spTeam_GetAll", new { });

            GetTeamMember_ByTeam(teams);

            return teams;
        }

        private void GetTeam_ByTournament(TournamentModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            model.TeamList = sql.ReadData<TeamModel, dynamic>("dbo.spTeam_GetByTournament", new { TournamentId = model.Id });

            GetTeamMember_ByTeam(model.TeamList);
        }

        private TeamModel GetTeam_ById(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            List<TeamModel> teams = sql.ReadData<TeamModel, dynamic>("dbo.spTeam_GetById", new { TeamId = id });

            GetTeamMember_ByTeam(teams);

            return teams.Count == 0 ? null : teams.First();
        }

        private void GetTeamMember_ByTeam(List<TeamModel> models)
        {
            SqlDataAccess sql = new SqlDataAccess();

            foreach (TeamModel team in models)
            {
                team.TeamMembers = sql.ReadData<PersonModel, dynamic>("dbo.spTeamMember_GetByTeam", new { TeamId = team.Id });
            }
        }

        public List<TournamentModel> GetTournament_All()
        {
            SqlDataAccess sql = new SqlDataAccess();

            List<TournamentModel> output = sql.ReadData<TournamentModel, dynamic>("dbo.spTournament_GetAll", new { });

            foreach (TournamentModel tournament in output)
            {
                tournament.Prizes = sql.ReadData<PrizeModel, dynamic>("dbo.spPrize_GetByTournament",
                    new { TournamentId = tournament.Id });

                GetTeam_ByTournament(tournament);

                GetRound_ByTournament(tournament);
            }

            return output;
        }

        private void GetRound_ByTournament(TournamentModel model)
        {
            List<MatchupModel> matchupList = GetMatchup_ByTournament(model.Id);

            int numOfRound = 0;
            if (matchupList.Count != 0)
            {
                numOfRound = matchupList.OrderByDescending(x => x.MatchupRound).First().MatchupRound;
            }

            for (int i = 1; i <= numOfRound; i++)
            {
                model.Rounds.Add(matchupList.FindAll(x => x.MatchupRound == i));
            }
        }

        private List<MatchupModel> GetMatchup_ByTournament(int id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            List<MatchupModel> matchups = sql.ReadData<MatchupModel, dynamic>("dbo.spMatchup_GetByTournament",
                new { TournamentId = id });

            foreach (MatchupModel m in matchups)
            {
                if (m.WinnerId > 0)
                {
                    m.Winner = GetTeam_ById(m.WinnerId);
                }

                m.Entries = sql.ReadData<MatchupEntryModel, dynamic>("dbo.spMatchupEntry_GetByMatchup",
                    new { MatchupId = m.Id });

                foreach (MatchupEntryModel me in m.Entries)
                {
                    if (me.TeamId > 0)
                    {
                        me.TeamCompeting = GetTeam_ById(me.TeamId);
                    }

                    if (me.ParentMatchupId != 0)
                    {
                        me.ParentMatchup = matchups.Where(x => x.Id == me.ParentMatchupId)?.First();
                    }
                }
            }

            return matchups;
        }

        public void UpdateMatchupWinner(MatchupModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            if (model.Winner != null)
            {
                sql.UpdateData("dbo.spMatchup_UpdateWinner", new { Id = model.Id, WinnerId = model.Winner.Id });
            }

            foreach (MatchupEntryModel entry in model.Entries)
            {
                if (entry.TeamCompeting != null)
                {
                    var p = new
                    {
                        Id = entry.Id,
                        TeamId = entry.TeamCompeting.Id,
                        Score = entry.Score
                    };

                    sql.UpdateData("dbo.spMatchupEntry_Update", p);
                }
            }
        }

        public void UpdateTournamentComplete(TournamentModel model)
        {
            SqlDataAccess sql = new SqlDataAccess();

            sql.UpdateData("dbo.spTournament_Complete", new { model.Id });
        }
    }
}
