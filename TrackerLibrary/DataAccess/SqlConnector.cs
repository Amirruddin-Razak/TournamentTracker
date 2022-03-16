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
        /// <param name="person"></param>
        /// <returns></returns>
        public PersonModel CreatePerson(PersonModel person)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                var p = new DynamicParameters();

                p.Add("@Id", person.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@FirstName", person.FirstName);
                p.Add("@LastName", person.LastName);
                p.Add("@EmailAddress", person.EmailAddress);
                p.Add("@PhoneNumber", person.PhoneNumber);

                connection.Execute("dbo.spPerson_Insert", p, commandType: CommandType.StoredProcedure);

                person.Id = p.Get<int>("@Id");
            }

            return person;
        }

        /// <summary>
        /// Save new prize to SQL Database
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        public PrizeModel CreatePrize(PrizeModel prize)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                var p = new DynamicParameters();

                p.Add("@Id", prize.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@PlaceNumber", prize.PlaceNumber);
                p.Add("@PrizeName", prize.PrizeName);
                p.Add("@PrizeAmount", prize.PrizeAmount);
                p.Add("@PrizePercentage", prize.PrizePercentage);

                connection.Execute("dbo.spPrize_Insert", p, commandType: CommandType.StoredProcedure);

                prize.Id = p.Get<int>("@Id");

                return prize;
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> people;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName))) 
            {
                people = connection.Query<PersonModel>("dbo.spPerson_GetAll").ToList();
            }

            return people;
        }
    }
}
