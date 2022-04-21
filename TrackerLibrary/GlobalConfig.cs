using Microsoft.Extensions.Configuration;
using System.Configuration;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PersonFileName = "PersonModels.csv";
        public const string PrizeFileName = "PrizeModels.csv";
        public const string TeamFileName = "TeamModels.csv";
        public const string MatchupFileName = "MatchupModels.csv";
        public const string MatchupEntryFileName = "MatchupEntryModels.csv";
        public const string TournamentFileName = "TournamentModels.csv";

        public static IDataConnection connection;
        private static IConfiguration _config;

        public static void InitiallizeConnection(DatabaseType databaseType, IConfiguration config)
        {
            _config = config;

            if (databaseType == DatabaseType.Sql)
            {
                connection = new SqlConnector();
            }
            else if (databaseType == DatabaseType.TextFile)
            {
                connection = new TextConnector();
            }
        }

        public static string GetCnnString(string name) => _config.GetConnectionString(name);

        public static string AppKeyLookup(string key) => _config.GetSection("appSettings")[key];
    }
}
