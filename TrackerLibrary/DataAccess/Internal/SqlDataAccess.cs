using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.DataAccess.Internal
{
    internal class SqlDataAccess
    {
        private const string DbName = "TournamentTracker_DB";

        internal void CreateData<T>(string storedProcedure, T parameter)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                connection.Execute(storedProcedure, parameter, commandType: CommandType.StoredProcedure);
            }
        }

        internal void CreateData<T>(string storedProcedure, T parameter, out int id)
        {
            id = 0;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                var p = new DynamicParameters(parameter);
                p.Add("@Id", id, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute(storedProcedure, p, commandType: CommandType.StoredProcedure);

                id = p.Get<int>("@Id");
            }
        }

        internal List<T> ReadData<T, U>(string storedProcedure, U parameter)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                List<T> output = connection.Query<T>(storedProcedure, parameter, commandType: CommandType.StoredProcedure).ToList();
                return output;
            }
        }

        internal void UpdateData<T>(string storedProcedure, T parameter)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.GetCnnString(DbName)))
            {
                connection.Execute(storedProcedure, parameter, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
