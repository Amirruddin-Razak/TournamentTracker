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
using TrackerWinFormUI.Interface;

namespace TrackerWinFormUI
{
    public partial class NewTournamentForm : Form, ITeamRequestor
    {
        private BindingList<TeamModel> availableTeams = new BindingList<TeamModel>(GlobalConfig.connection.GetTeam_All());
        private BindingList<TeamModel> selectedTeams = new BindingList<TeamModel>();
        private BindingList<PrizeModel> selectedPrizes = new BindingList<PrizeModel>();
        private ITournamentRequestor callingForm;

        public NewTournamentForm(ITournamentRequestor caller)
        {
            InitializeComponent();
            callingForm = caller;

            InitializeFormData();
        }

        private void InitializeFormData()
        {
            teamComboBox.DataSource = availableTeams;
            teamComboBox.DisplayMember = "TeamName";

            teamListBox.DataSource = selectedTeams;
            teamListBox.DisplayMember = "TeamName";

            prizeListBox.DataSource = selectedPrizes;
            prizeListBox.DisplayMember = "PrizeName";
        }

        private void CreatePrizeButton_Click(object sender, EventArgs e)
        {
            bool usePrizeAmount = prizeAmountRadioButton.Checked;

            if (ValidatePrize(usePrizeAmount) == false)
            {
                return;
            }

            PrizeModel prize = new PrizeModel(prizeNameTextBox.Text, prizePlaceNumberTextBox.Text, usePrizeAmount,
                    prizeValueTextBox.Text);

            selectedPrizes.Add(prize);

            prizeNameTextBox.Clear();
            prizePlaceNumberTextBox.Clear();
            prizeValueTextBox.Text = "0";
        }

        private bool ValidatePrize(bool usePrizeAmount)
        {
            bool output = true;
            string errorMessage = "";

            if (prizeNameTextBox.Text == "")
            {
                errorMessage += "Prize Name must be filled \n";
                output = false;
            }

            bool validPlaceNumber = int.TryParse(prizePlaceNumberTextBox.Text, out int prizePlaceNumber);

            if (prizePlaceNumberTextBox.Text == "" || validPlaceNumber == false)
            {
                errorMessage += "Prize Place Number must be filled with a numeric value \n";
                output = false;
            }
            else if (prizePlaceNumber < 1)
            {
                errorMessage += "Prize Place Number must be greater than 0 \n";
                output = false;
            }

            if (usePrizeAmount)
            {
                bool validPrizeAmount = decimal.TryParse(prizeValueTextBox.Text, out decimal prizeAmount);

                if (prizeValueTextBox.Text == "" || validPrizeAmount == false)
                {
                    errorMessage += "Prize Amount must be numeric value \n";
                    output = false;
                }
                else if (prizeAmount <= 0)
                {
                    errorMessage += "Negative or Zero Prize Amount are not allowed \n";
                    output = false;
                }
            }
            else
            {
                bool validPrizePercentage = double.TryParse(prizeValueTextBox.Text, out double prizePercentage);

                if (prizeValueTextBox.Text == "" || validPrizePercentage == false)
                {
                    errorMessage += "Prize Percentage must be numeric value \n";
                    output = false;
                }
                else if (prizePercentage <= 0 || prizePercentage > 100)
                {
                    errorMessage += "Prize Percentage must be more than 0 and up to 100 \n";
                    output = false;
                }
            }

            if (output == false)
            {
                MessageBox.Show(errorMessage, "Error");
            }

            return output;
        }

        private void PrizeAmountRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            prizeValueLabel.Text = "Prize Amount      :";
            prizeValueTextBox.Clear();
        }

        private void PrizePercentageRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            prizeValueLabel.Text = "Prize Percentage :";
            prizeValueTextBox.Clear();
        }

        private void NewTeamLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewTeamForm frm = new NewTeamForm(this);
            frm.Show();
        }

        private void AddTeamButton_Click(object sender, EventArgs e)
        {
            if (teamComboBox.SelectedItem != null)
            {
                TeamModel team = (TeamModel)teamComboBox.SelectedItem;
                selectedTeams.Add(team);
                availableTeams.Remove(team);
            }
            else
            {
                MessageBox.Show("Please select a team to add", "Error");
            }
        }

        private void RemoveTeamButton_Click(object sender, EventArgs e)
        {
            if (teamListBox.SelectedItem != null)
            {
                TeamModel team = (TeamModel)teamListBox.SelectedItem;
                selectedTeams.Remove(team);
                availableTeams.Add(team);
            }
            else
            {
                MessageBox.Show("Please select a team to remove", "Error");
            }
        }

        private void DeletePrizeButton_Click(object sender, EventArgs e)
        {
            if (prizeListBox.SelectedItem != null)
            {
                selectedPrizes.Remove((PrizeModel)prizeListBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select a prize to delete", "Error");
            }
        }

        public void NewTeamComplete(TeamModel team)
        {
            selectedTeams.Add(team);
        }

        private void CreateTournamentButton_Click(object sender, EventArgs e)
        {
            if (ValidateTournamemnt() == false)
            {
                return;
            }

            TournamentModel tournament = new TournamentModel()
            {
                TournamentName = tournamentNameTextBox.Text,
                EntryFee = decimal.Parse(entreeFeeTextBox.Text),
                TeamList = selectedTeams.ToList(),
                Prizes = selectedPrizes.ToList()
            };

            TournamentLogic.CreateNewTournament(tournament);

            callingForm.NewTournamentComplete(tournament);

            Close();
        }

        private bool ValidateTournamemnt()
        {
            bool output = true;
            string errorMessage = "";

            if (tournamentNameTextBox.Text.Length < 1 || tournamentNameTextBox.Text.Length > 50)
            {
                errorMessage += "Tournament Name must be between 1 to 50 character long \n";
                output = false;
            }

            bool isValidFee = decimal.TryParse(entreeFeeTextBox.Text, out decimal entreeFee);

            if (!isValidFee)
            {
                errorMessage += "Tournament Fee must be 0 or greater \n";
                output = false;
            }
            else if (entreeFee < 0)
            {
                errorMessage += "Tournament Fee must be a positive value \n";
                output = false;
            }
            else
            {
                decimal totalPrize = 0;
                decimal totalFees = decimal.Multiply(entreeFee, selectedTeams.Count);

                foreach (PrizeModel p in selectedPrizes)
                {
                    totalPrize = decimal.Add(totalPrize, p.CalculatePrize(totalFees));
                }

                if (totalPrize > totalFees)
                {
                    errorMessage += $"Total prize amount = { totalPrize } exceed the total collected fees by { totalPrize - totalFees } \n";
                    output = false;
                }
            }

            if (output == false)
            {
                MessageBox.Show(errorMessage, "Error");
            }

            return output;
        }
    }
}
