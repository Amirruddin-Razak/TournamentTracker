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
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel tournament;
        private BindingList<int> roundNumber = new BindingList<int>();
        private BindingList<MatchupModel> round = new BindingList<MatchupModel>();

        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();

            tournament = tournamentModel;

            LoadFormData();
            LoadRound();
            WireUpList();
        }

        private void LoadFormData() 
        {
            tournamentNameLabel.Text = tournament.TournamentName;
        }

        private void LoadRound()
        {
            for (int i = 0; i < tournament.Rounds.Count; i++)
            {
                roundNumber.Add(i + 1);
            }
        }

        private void WireUpList()
        {
            roundDropDown.DataSource = roundNumber;

            matchupListBox.DataSource = round;
            matchupListBox.DisplayMember = "DisplayName";
        }

        private void RoundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchupList((int)roundDropDown.SelectedItem);
        }

        private void LoadMatchupList(int selectedRoundNumber) 
        {
            round.Clear();

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == selectedRoundNumber)
                {
                    foreach (MatchupModel m in matchups)
                    {
                        if (m.Winner == null || !upcomingMatchOnlyCheckBox.Checked)
                        {
                            round.Add(m);
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

        private void UpcomingMatchOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchupList((int)roundDropDown.SelectedItem);
        }

        private void MatchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup((MatchupModel)matchupListBox.SelectedItem);
            DisplayMatchupDetail();
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

        private void SaveScoreButton_Click(object sender, EventArgs e)
        {
            MatchupModel selectedMatchup = (MatchupModel)matchupListBox.SelectedItem;

            if (selectedMatchup == null)
            {
                return;
            }

            string errorMeesage = "";

            if (ValidateScore(ref errorMeesage, selectedMatchup))
            {
                if (selectedMatchup.Entries.Count == 1)
                {
                    selectedMatchup.Entries[0].Score = double.Parse(team1ScoreTextBox.Text);
                    selectedMatchup.Winner = selectedMatchup.Entries[0].TeamCompeting;
                }
                else
                {
                    selectedMatchup.Entries[0].Score = double.Parse(team1ScoreTextBox.Text);
                    selectedMatchup.Entries[1].Score = double.Parse(team2ScoreTextBox.Text);

                    if (selectedMatchup.Entries[0].Score > selectedMatchup.Entries[1].Score)
                    {
                        selectedMatchup.Winner = selectedMatchup.Entries[0].TeamCompeting;
                    }
                    else if(selectedMatchup.Entries[1].Score > selectedMatchup.Entries[0].Score)
                    {
                        selectedMatchup.Winner = selectedMatchup.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        MessageBox.Show("Tie game are not allowed", "Error");
                    }
                }

                foreach (List<MatchupModel> r in tournament.Rounds)
                {
                    foreach (MatchupModel m in r)
                    {
                        foreach (MatchupEntryModel me in m.Entries)
                        {
                            if (me.ParentMatchup != null && me.ParentMatchup.Id == selectedMatchup.Id)
                            {
                                me.TeamCompeting = selectedMatchup.Winner;
                                GlobalConfig.connection.UpdateMatchup(m);
                            }
                        }
                    } 
                }

                GlobalConfig.connection.UpdateMatchup(selectedMatchup);

                LoadMatchupList((int)roundDropDown.SelectedItem);
            }
            else
            {
                MessageBox.Show(errorMeesage, "Error");
            }
        }

        private bool ValidateScore(ref string errorMessage, MatchupModel selectedMatchup)
        {
            bool output = true;
            bool scoreValid;

            if (selectedMatchup.Entries[0].TeamCompeting == null)
            {
                errorMessage += "team 1 need to be determined before a match is played \n";
                output = false;
            }
            else
            {
                scoreValid = double.TryParse(team1ScoreTextBox.Text, out _);
                if (!scoreValid)
                {
                    errorMessage += "Please enter a valid score for Team 1 \n";
                    output = false;
                }
            }

            if (selectedMatchup.Entries.Count == 2)
            {
                if (selectedMatchup.Entries[1].TeamCompeting == null)
                {
                    errorMessage += "team 2 need to be determined before a match is played \n";
                    output = false;
                }
                else
                {
                    scoreValid = double.TryParse(team2ScoreTextBox.Text, out _);
                    if (!scoreValid)
                    {
                        errorMessage += "Please enter a valid score for Team 2 \n";
                        output = false;
                    }
                }
            }

            return output;
        }
    }
}
