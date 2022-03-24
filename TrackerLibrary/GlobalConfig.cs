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

        public static void InitiallizeConnection(DatabaseType databaseType)
        {
            if (databaseType == DatabaseType.Sql)
            {
                connection = new SqlConnector();
            }
            else if (databaseType == DatabaseType.TextFile)
            {
                connection = new TextConnector();
            }
        }

        public static string GetCnnString(string name) => ConfigurationManager.ConnectionStrings[name].ConnectionString;

        public static string AppKeyLookup(string key) => ConfigurationManager.AppSettings[key];
    }
}
