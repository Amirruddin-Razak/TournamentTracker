using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection connection;

        public static void InitiallizeConnection(DatabaseType databaseType)
        {
            if (databaseType == DatabaseType.Sql)
            {
                // todo initiallize connection to sql
                connection = new SqlConnector();
            }
            else if (databaseType == DatabaseType.TextFile)
            {
                // todo initiallize connection to text
                connection = new TextConnector();
            }
        }

        public static string GetCnnString(string name) 
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
