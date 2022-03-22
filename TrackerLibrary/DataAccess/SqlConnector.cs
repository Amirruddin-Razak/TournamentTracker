using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        private const string DbName = "TournamentTracker_DB";

        /// <summary>
        /// Save new person to SQL database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                var p = new DynamicParameters();

                p.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@PhoneNumber", model.PhoneNumber);

                connection.Execute("dbo.spPerson_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");
            }

            return model;
        }

        /// <summary>
        /// Save new prize to SQL Database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                var p = new DynamicParameters();

                p.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PrizeName", model.PrizeName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);

                connection.Execute("dbo.spPrize_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");

                return model;
            }
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName))) 
            {
                var p = new DynamicParameters();
                p.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@TeamName", model.TeamName);

                connection.Execute("dbo.spTeam_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");

                foreach (PersonModel member in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@PersonId", member.Id);
                    p.Add("@TeamId", model.Id);

                    connection.Execute("dbo.spTeamMember_Insert", p, commandType: CommandType.StoredProcedure);
                }
            }

            return model;
        }

        public TournamentModel CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                SaveTournament(connection, model);

                SaveTournamentPrize(connection, model);

                SaveTournamentEntry(connection, model);

                SaveTournamentRound(connection, model);
            }

            return model;
        }

        private void SaveTournament(IDbConnection connection, TournamentModel model)
        {
            var p = new DynamicParameters();
            p.Add("@Id", model.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntreeFee", model.EntryFee);

            connection.Execute("dbo.spTournament_Insert", p, commandType: CommandType.StoredProcedure);

            model.Id = p.Get<int>("@Id");
        }

        private void SaveTournamentPrize(IDbConnection connection, TournamentModel model)
        {
            foreach (PrizeModel prize in model.Prizes)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", prize.Id);

                connection.Execute("dbo.spTournamentPrize_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }

        private void SaveTournamentEntry(IDbConnection connection, TournamentModel model) 
        {
            foreach (TeamModel team in model.TeamList)
            {
                var p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", team.Id);

                connection.Execute("dbo.spTournamentEntry_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }

        private void SaveTournamentRound(IDbConnection connection, TournamentModel model)
        {
            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@Id", matchup.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);

                    connection.Execute("dbo.spMatchup_Insert", p, commandType: CommandType.StoredProcedure);

                    matchup.Id = p.Get<int>("@Id");

                    SaveMatchupEntry(connection, matchup);
                }
            }
        }

        private void SaveMatchupEntry(IDbConnection connection, MatchupModel matchup)
        {
            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                var p = new DynamicParameters();
                p.Add("@Id", entry.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@TeamId", entry.TeamCompeting?.Id);
                p.Add("@MatchupId", matchup.Id);
                p.Add("@ParentMatchupId", entry.ParentMatchup?.Id);

                connection.Execute("dbo.spMatchupEntry_Insert", p, commandType: CommandType.StoredProcedure);

                entry.Id = p.Get<int>("@Id");
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName))) 
            {
                output = connection.Query<PersonModel>("dbo.spPerson_GetAll").ToList();
            }

            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output = new List<TeamModel>();

            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                List<TeamModel> teams = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();

                output = GetTeamMember_ByTeam(connection, teams);
            }

            return output;
        }

        private List<TeamModel> GetTeam_ByTournament(IDbConnection connection, TournamentModel tournament)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentId", tournament.Id);

            tournament.TeamList = connection.Query<TeamModel>("dbo.spTeam_GetByTournament", p,
                        commandType: CommandType.StoredProcedure).ToList();

            tournament.TeamList = GetTeamMember_ByTeam(connection, tournament.TeamList);

            return tournament.TeamList;
        }

        private TeamModel GetTeam_ById(IDbConnection connection, int id)
        {
            var p = new DynamicParameters();
            p.Add("@TeamId", id);

            List<TeamModel> teams = connection.Query<TeamModel>("dbo.spTeam_GetById", p,
                commandType: CommandType.StoredProcedure).ToList();

            teams = GetTeamMember_ByTeam(connection, teams);

            if (teams.Count == 0)
            {
                return null;
            }

            return teams.First();
        }

        private List<TeamModel> GetTeamMember_ByTeam(IDbConnection connection, List<TeamModel> teams)
        {
            List<TeamModel> output = new List<TeamModel>();

            foreach (TeamModel t in teams)
            {
                var p = new DynamicParameters();
                p.Add("@TeamId", t.Id);

                t.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMember_GetByTeam", p,
                    commandType: CommandType.StoredProcedure).ToList();
                output.Add(t);
            }

            return output;
        }

        public List<TournamentModel> GetTournament_All()
        {
            List<TournamentModel> output = new List<TournamentModel>();

            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                output = connection.Query<TournamentModel>("dbo.spTournament_GetAll").ToList();

                foreach (TournamentModel tournament in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", tournament.Id);

                    tournament.Prizes = connection.Query<PrizeModel>("dbo.spPrize_GetByTournament", p,
                        commandType: CommandType.StoredProcedure).ToList();

                    tournament.TeamList = GetTeam_ByTournament(connection, tournament);

                    tournament.Rounds = GetRound_ByTournament(connection, tournament);
                }
            }

            return output;
        }

        private List<List<MatchupModel>> GetRound_ByTournament(IDbConnection connection, TournamentModel tournament)
        {
            List<MatchupModel> matchupList = GetMatchup_ByTournament(connection, tournament.Id);

            int numOfRound = 0;
            if (matchupList.Count != 0)
            {
                numOfRound = matchupList.OrderByDescending(x => x.MatchupRound).First().MatchupRound;
            }

            for (int i = 1; i <= numOfRound; i++)
            {
                tournament.Rounds.Add(matchupList.FindAll(x => x.MatchupRound == i));
            }

            return tournament.Rounds;
        }

        private List<MatchupModel> GetMatchup_ByTournament(IDbConnection connection, int id)
        {
            var p = new DynamicParameters();
            p.Add("@TournamentId", id);

            List<MatchupModel> matchups = connection.Query<MatchupModel>("dbo.spMatchup_GetByTournament", p,
                commandType: CommandType.StoredProcedure).ToList();

            foreach (MatchupModel m in matchups)
            {
                if (m.WinnerId > 0)
                {
                    m.Winner = GetTeam_ById(connection, m.WinnerId);
                }

                p = new DynamicParameters();
                p.Add("@MatchupId", m.Id);

                m.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchupEntry_GetByMatchup", p,
                    commandType: CommandType.StoredProcedure).ToList();

                foreach (MatchupEntryModel me in m.Entries)
                {
                    if (me.TeamId > 0)
                    {
                        me.TeamCompeting = GetTeam_ById(connection, me.TeamId);
                    }

                    if (me.ParentMatchupId != 0)
                    {
                        me.ParentMatchup = matchups.Where(x => x.Id == me.ParentMatchupId)?.First();
                    }
                }
            }

            return matchups;
        }
    }
}
