using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerUI.Library.Api.Helper;

namespace TrackerWinFormUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var config = InitializeConfiguration();
            GlobalConfig.InitiallizeConnection(DatabaseType.TextFile, config);

            IApiConnector apiConnector = new ApiConnector(config.GetSection("appSettings")["ApiUrl"]);

            Application.Run(new DashboardForm(apiConnector));
        }

        private static IConfiguration InitializeConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            return builder.Build();
        }
    }
}
