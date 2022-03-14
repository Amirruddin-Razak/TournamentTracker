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
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class NewTournamentForm : Form
    {
        public NewTournamentForm()
        {
            InitializeComponent();
        }

        private void CreatePrizeButton_Click(object sender, EventArgs e)
        {
            bool usePrizeAmount = prizeAmountRadioButton.Checked;
            string errorMessage = "";

            if (ValidatePrize(usePrizeAmount, ref errorMessage))
            {
                PrizeModel prize = new PrizeModel(prizeNameTextBox.Text, prizePlaceNumberTextBox.Text, usePrizeAmount,
                    prizeValueTextBox.Text);

                prize = GlobalConfig.connection.CreatePrize(prize);

                prizeListBox.Items.Add($"Number: {prize.PlaceNumber}, Name: {prize.PrizeName}, Amount: {prize.PrizeAmount}");

                prizeNameTextBox.Clear();
                prizePlaceNumberTextBox.Clear();
                prizeValueTextBox.Text = "0";
            }
            else
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK);
            }
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

        private bool ValidatePrize(bool usePrizeAmount, ref string errorMessage)
        {
            bool output = true;

            if (prizeNameTextBox.Text == "")
            {
                errorMessage += "Prize Name must be filled";
                errorMessage += "\n";
                output = false;
            }

            bool validPlaceNumber = int.TryParse(prizePlaceNumberTextBox.Text, out int prizePlaceNumber);

            if (prizePlaceNumberTextBox.Text == "" || validPlaceNumber == false)
            {
                errorMessage += "Prize Place Number must be filled with a numeric value";
                errorMessage += "\n";
                output = false;
            }
            else if (prizePlaceNumber < 1)
            {
                errorMessage += "Prize Place Number must be greater than 0";
                errorMessage += "\n";
                output = false;
            }

            if (usePrizeAmount)
            {
                bool validPrizeAmount = decimal.TryParse(prizeValueTextBox.Text, out decimal prizeAmount);

                if (prizeValueTextBox.Text == "" || validPrizeAmount == false)
                {
                    errorMessage += "Prize Amount must be numeric value";
                    errorMessage += "\n";
                    output = false;
                }
                else if (prizeAmount <= 0)
                {
                    errorMessage += "Negative or Zero Prize Amount are not allowed";
                    errorMessage += "\n";
                    output = false;
                }
            }
            else
            {
                bool validPrizePercentage = double.TryParse(prizeValueTextBox.Text, out double prizePercentage);

                if (prizeValueTextBox.Text == "" || validPrizePercentage == false)
                {
                    errorMessage += "Prize Percentage must be numeric value";
                    errorMessage += "\n";
                    output = false;
                }
                else if (prizePercentage <= 0 || prizePercentage > 100)
                {
                    errorMessage += "Prize Percentage must be more than 0 and up to 100";
                    errorMessage += "\n";
                    output = false;
                }
            }

            return output;
        }

        private void NewTeamLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }
    }
}
