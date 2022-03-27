using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerWinFormUI.Interface;

namespace TrackerWinFormUI
{
    public partial class NewTeamForm : Form
    {
        private ITeamRequestor callingForm;
        private BindingList<PersonModel> availableMembers = new BindingList<PersonModel>(GlobalConfig.connection.GetPerson_All());
        private BindingList<PersonModel> selectedMembers = new BindingList<PersonModel>();

        public NewTeamForm(ITeamRequestor caller)
        {
            InitializeComponent();

            callingForm = caller;

            InitializeFormData();
        }

        private void InitializeFormData()
        {
            selectMemberComboBox.DataSource = availableMembers;
            selectMemberComboBox.DisplayMember = "FullName";

            memberListBox.DataSource = selectedMembers;
            memberListBox.DisplayMember = "FullName";
        }

        private void AddMemberButton_Click(object sender, EventArgs e)
        {
            if (selectMemberComboBox.SelectedItem != null)
            {
                PersonModel member = (PersonModel)selectMemberComboBox.SelectedItem;

                selectedMembers.Add(member);
                availableMembers.Remove(member);
            }
            else
            {
                MessageBox.Show("Please select a member to add","Error");
            }
        }

        private void RemoveMemberButton_Click(object sender, EventArgs e)
        {
            if (memberListBox.SelectedItem != null)
            {
                PersonModel member = (PersonModel)memberListBox.SelectedItem;

                availableMembers.Add(member);
                selectedMembers.Remove(member);
            }
            else
            {
                MessageBox.Show("Please select a member to remove", "Error");
            }
        }

        private void CreateMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateNewMember() == false)
            {
                return;
            }

            PersonModel person = new PersonModel()
            {
                FirstName = firstNameTextBox.Text,
                LastName = lastNameTextBox.Text,
                EmailAddress = emailAddressTextBox.Text,
                PhoneNumber = phoneNumberTextBox.Text
            };

            GlobalConfig.connection.SaveNewPerson(person);

            selectedMembers.Add(person);

            firstNameTextBox.Clear();
            lastNameTextBox.Clear();
            emailAddressTextBox.Clear();
            phoneNumberTextBox.Clear();
        }

        private bool ValidateNewMember()
        {
            bool output = true;
            string errorMessage = "";

            if (firstNameTextBox.Text == "")
            {
                errorMessage += "First Name cannot be empty \n";
                output = false;
            }
            else if (firstNameTextBox.Text.Length > 100)
            {
                errorMessage += "First Name cannot be more than 100 character \n";
                output = false;
            }

            if (lastNameTextBox.Text.Length > 100)
            {
                errorMessage += "Last Name cannot be more than 100 character \n";
                output = false;
            }

            if (emailAddressTextBox.Text == "")
            {
                errorMessage += "Email Address cannot be empty \n";
                output = false;
            }
            else if (emailAddressTextBox.Text.Length > 200)
            {
                errorMessage += "Email Address cannot be more than 200 character \n";
                output = false;
            }
            else if (!Regex.IsMatch(emailAddressTextBox.Text, @"^(?:[\.A-z0-9!#$%&'*+/=?^_`{|}~-]+@(?:[A-z0-9-])+(?:.)+[A-z0-9])$"))
            {
                errorMessage += "Please enter a valid Email Address \n";
                output = false;
            }

            if (phoneNumberTextBox.Text.Length > 20)
            {
                errorMessage += "Phone Number cannot be more than 20 character \n";
                output = false;
            }
            else if (!Regex.IsMatch(phoneNumberTextBox.Text, @"^[0-9+\s-]*$"))
            {
                errorMessage += "Please enter a valid Phone Number \n";
                output = false;
            }

            if (output == false)
            {
                MessageBox.Show(errorMessage, "Error");
            }

            return output;
        }

        private void CreateTeamButton_Click(object sender, EventArgs e)
        {
            if (ValidateTeam() == false)
            {
                return;
            }

            TeamModel team = new TeamModel
            {
                TeamName = teamNameTextBox.Text,
                TeamMembers = selectedMembers.ToList()
            };

            GlobalConfig.connection.SaveNewTeam(team);
            callingForm.NewTeamComplete(team);

            Close();
        }

        private bool ValidateTeam()
        {
            string errorMessage = "";
            bool output = true;

            if (teamNameTextBox.Text.Length == 0)
            {
                errorMessage += "Please enter a team name \n";
                output = false;
            }
            else if (teamNameTextBox.Text.Length > 20)
            {
                errorMessage += "Team name cannot be more than 20 characters long \n";
                output = false;
            }

            if (output == false)
            {
                MessageBox.Show(errorMessage, "Error");
            }

            return output;
        }
    }
}
