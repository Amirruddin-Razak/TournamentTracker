using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Windows.Forms;
using TrackerLibrary;

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

            GlobalConfig.InitiallizeConnection(DatabaseType.TextFile, InitializeConfiguration());
            Application.Run(new DashboardForm());
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
