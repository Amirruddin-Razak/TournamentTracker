using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
     //   @PlaceNumber INT,
     //   @PrizeName NVARCHAR(20),
	    //@PrizeAmount MONEY,
     //   @PrizePercentage FLOAT,
	    //@Id INT = 0 OUTPUT


        /// <summary>
        /// Save new prize to SQL Database
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        // TODO make SQL connector
        public PrizeModel CreatePrize(PrizeModel prize)
        {
            // Simulate saving to database and add ID
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.GetCnnString("TournamentTracker_DB")))
            {
                var p = new DynamicParameters();

                p.Add("@PlaceNumber", prize.PlaceNumber);
                p.Add("@PrizeName", prize.PrizeName);
                p.Add("@PrizeAmount", prize.PrizeAmount);
                p.Add("@PrizePercentage", prize.PrizePercentage);
                p.Add("@Id", prize.Id, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrize_Insert", p, commandType: CommandType.StoredProcedure);

                prize.Id = p.Get<int>("@Id");

                return prize;
            }
        }
    }
}
