using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerUI.Library.Api;
using TrackerUI.Library.Api.Helper;
using TrackerUI.Library.Models;
using TrackerWinFormUI.Interface;

namespace TrackerWinFormUI
{
    public partial class TournamentViewerForm : Form
    {
        private readonly TournamentModel _tournament;
        private readonly BindingList<int> _roundNumber = new();
        private readonly BindingList<MatchupModel> _round = new();
        private readonly DashboardForm _caller;
        private readonly ITournamentEndpoint _tournamentEndpoint;

        public TournamentViewerForm(TournamentModel tournamentModel, DashboardForm callingForm, IApiConnector apiConnector)
        {
            InitializeComponent();

            _tournament = tournamentModel;
            _caller = callingForm;
            _tournamentEndpoint = new TournamentEndpoint(apiConnector);
            _tournament.OnTournamentComplete += Tournament_OnTournamentComplete;

            InitializeFormData();
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            string winner = _tournament.Rounds.Find(x => x.First().MatchupRound == _roundNumber.Last()).First().Winner.TeamName;
            MessageBox.Show($"Tournament has ended, The winner is { winner }");

            Close();
        }

        private void InitializeFormData() 
        {
            tournamentNameLabel.Text = _tournament.TournamentName;

            LoadRoundNumber();

            roundDropDown.DataSource = _roundNumber;

            matchupListBox.DataSource = _round;
            matchupListBox.DisplayMember = "DisplayName";

            while (_round.Count == 0 && ((int)roundDropDown.SelectedItem != _roundNumber.Last()))
            {
                roundDropDown.SelectedIndex += 1;
            }
        }

        private void LoadRoundNumber()
        {
            for (int i = 0; i < _tournament.Rounds.Count; i++)
            {
                _roundNumber.Add(i + 1);
            }
        }

        private void RoundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchupList((int)roundDropDown.SelectedItem);
        }

        private void LoadMatchupList(int selectedRoundNumber) 
        {
            _round.Clear();

            foreach (List<MatchupModel> matchups in _tournament.Rounds)
            {
                if (matchups.First().MatchupRound == selectedRoundNumber)
                {
                    foreach (MatchupModel m in matchups)
                    {
                        if (m.Winner == null || !upcomingMatchOnlyCheckBox.Checked)
                        {
                            _round.Add(m);
                        }
                    }
                }
            }

            DisplayMatchupDetail();
        }

        private void DisplayMatchupDetail()
        {
            bool isVisible = matchupListBox.SelectedItem != null;

            team1Label.Visible = isVisible;
            team2Label.Visible = isVisible;
            team1ScoreTextBox.Visible = isVisible;
            team2ScoreTextBox.Visible = isVisible;
            vsLabel.Visible = isVisible;
            saveScoreButton.Visible = isVisible;

            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
        }

        private void LoadMatchup(MatchupModel selectedMatchup)
        {
            if (selectedMatchup == null)
            {
                return;
            }

            if (selectedMatchup.Entries[0].TeamCompeting != null)
            {
                team1Label.Text = selectedMatchup.Entries[0].TeamCompeting.TeamName;
                team1ScoreTextBox.Text = selectedMatchup.Entries[0].Score.ToString();
                team1ScoreTextBox.Enabled = true;
            }
            else
            {
                team1Label.Text = "Not Determined Yet";
                team1ScoreTextBox.Text = "";
                team1ScoreTextBox.Enabled = false;
            }

            if (selectedMatchup.Entries.Count == 1)
            {
                team2Label.Text = "byes";
                team2ScoreTextBox.Text = "";
                team1ScoreTextBox.Enabled = false;
                team2ScoreTextBox.Enabled = false;
            }
            else if (selectedMatchup.Entries[1].TeamCompeting != null)
            {
                team2Label.Text = selectedMatchup.Entries[1].TeamCompeting.TeamName;
                team2ScoreTextBox.Text = selectedMatchup.Entries[1].Score.ToString();
                team2ScoreTextBox.Enabled = true;
            }
            else
            {
                team2Label.Text = "Not Determined Yet";
                team2ScoreTextBox.Text = "";
                team2ScoreTextBox.Enabled = false;
            }
        }

        private void UpcomingMatchOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (roundDropDown.SelectedItem == null)
            {
                return;
            }
            LoadMatchupList((int)roundDropDown.SelectedItem);
        }

        private void MatchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
            DisplayMatchupDetail();
        }

        private async void SaveScoreButton_Click(object sender, EventArgs e)
        {
            MatchupModel selectedMatchup = (MatchupModel)matchupListBox.SelectedItem;

            if (selectedMatchup == null || ValidateScore(selectedMatchup) == false)
            {
                return;
            }

            if (selectedMatchup.Entries.Count == 1)
            {
                selectedMatchup.Entries[0].Score = double.Parse(team1ScoreTextBox.Text);
            }
            else
            {
                selectedMatchup.Entries[0].Score = double.Parse(team1ScoreTextBox.Text);
                selectedMatchup.Entries[1].Score = double.Parse(team2ScoreTextBox.Text);
            }

            try
            {
                await _tournamentEndpoint.UpdateTournamentResultAsync(_tournament);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The application has encountered the following error : { ex.Message }");
            }

            LoadMatchupList((int)roundDropDown.SelectedItem);

            if (_round.Count == 0 && ((int)roundDropDown.SelectedItem != _roundNumber.Last()))
            {
                roundDropDown.SelectedIndex += 1;
            }
        }

        private bool ValidateScore(MatchupModel selectedMatchup)
        {
            bool output = true;
            string errorMessage = "";

            if (selectedMatchup.Entries.Any(x => x.TeamCompeting == null))
            {
                errorMessage += "Both team need to be determined before a score is added \n";
                output = false;
            }

            bool ScoreOneValid = double.TryParse(team1ScoreTextBox.Text, out double teamOneScore);
            bool ScoreTwoValid = double.TryParse(team2ScoreTextBox.Text, out double teamTwoScore);

            if (!ScoreOneValid || !ScoreTwoValid)
            {
                errorMessage += "Please enter a valid score for both Team 1 and Team 2\n";
                output = false;
            }

            if (teamOneScore == teamTwoScore)
            {
                errorMessage += "Tie match are not allowed \n";
                output = false;
            }

            if (output == false)
            {
                MessageBox.Show(errorMessage, "Error");
            }

            return output;
        }

        private void TournamentViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _caller.ShowInTaskbar = true;
            _caller.WindowState = FormWindowState.Normal;
        }
    }
}
