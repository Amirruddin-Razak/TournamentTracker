using System;
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

            GlobalConfig.InitiallizeConnection(DatabaseType.Sql);
            Application.Run(new DashboardForm());
        }
    }
}
