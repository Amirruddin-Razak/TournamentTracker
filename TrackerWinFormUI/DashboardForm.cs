using System;
using System.ComponentModel;
using System.Windows.Forms;
using TrackerUI.Library.Api;
using TrackerUI.Library.Api.Helper;
using TrackerUI.Library.Models;
using TrackerWinFormUI.Interface;

namespace TrackerWinFormUI
{
    public partial class DashboardForm : Form, ITournamentRequestor
    {
        private BindingList<TournamentModel> _tournaments = new();
        private readonly IApiConnector _apiConnector;
        private readonly ITournamentEndpoint _tournamentEndpoint;

        public DashboardForm(IApiConnector apiConnector)
        {
            InitializeComponent();

            _apiConnector = apiConnector;
            _tournamentEndpoint = new TournamentEndpoint(apiConnector);

            InitializeFormData();
        }

        private async void InitializeFormData()
        {
            try
            {
                var result = await _tournamentEndpoint.GetActiveTournamentAsync();
                _tournaments = new BindingList<TournamentModel>(result);
                tournamentListBox.DataSource = _tournaments;
                tournamentListBox.DisplayMember = "TournamentName";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error occurs. Please try again later");
            }
        }

        private void CreateTournamentButton_Click(object sender, EventArgs e)
        {
            NewTournamentForm frm = new(this);
            frm.Show();

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        public void NewTournamentComplete(TournamentModel tournament)
        {
            _tournaments.Add(tournament);

            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void ViewTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel tournament = (TournamentModel)tournamentListBox?.SelectedItem;

            if (tournament == null)
            {
                MessageBox.Show("Please select a tournament to view", "Error");
                return;
            }

            tournament.OnTournamentComplete += Tournament_OnTournamentComplete;

            TournamentViewerForm frm = new(tournament, this, _apiConnector);
            frm.Show();

            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            InitializeFormData();
        }
    }
}
