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
        /// <summary>
        /// Save new prize to SQL Database
        /// </summary>
        /// <param name="prize"></param>
        /// <returns></returns>
        public PrizeModel CreatePrize(PrizeModel prize)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString("TournamentTracker_DB")))
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
