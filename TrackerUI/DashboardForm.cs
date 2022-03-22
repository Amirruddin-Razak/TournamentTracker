using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class DashboardForm : Form, ITournamentRequestor
    {
        List<TournamentModel> availableTournament = GlobalConfig.connection.GetTournament_All();

        public DashboardForm()
        {
            InitializeComponent();
            WireUpList();
        }

        private void WireUpList() 
        {
            tournamentListBox.DataSource = availableTournament;
            tournamentListBox.DisplayMember = "TournamentName";
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            NewTournamentForm frm = new NewTournamentForm(this);
            frm.Show();
        }

        public void NewTournamentComplete(TournamentModel tournament)
        {
            availableTournament.Add(tournament);
        }
    }
}
