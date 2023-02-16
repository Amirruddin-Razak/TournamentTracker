using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TrackerUI.Library.Api;
using TrackerUI.Library.Api.Helper;
using TrackerUI.Library.Models;
using TrackerWinFormUI.Interface;

namespace TrackerWinFormUI
{
    public partial class NewTeamForm : Form
    {
        private readonly ITeamRequestor _callingForm;
        private readonly ITeamEndpoint _teamEndpoint;
        private readonly IPersonEndpoint _personEndpoint;
        private readonly BindingList<PersonModel> _selectedMembers = new();
        private BindingList<PersonModel> _availableMembers = new();

        public NewTeamForm(ITeamRequestor caller, IApiConnector apiConnector)
        {
            InitializeComponent();

            _callingForm = caller;
            _teamEndpoint = new TeamEndpoint(apiConnector);
            _personEndpoint = new PersonEndpoint(apiConnector);
        }

        private async void NewTeamForm_Load(object sender, EventArgs e)
        {
            try
            {
                var personList = await _personEndpoint.GetAllPersonAsync();
                _availableMembers = new BindingList<PersonModel>(personList);

                selectMemberComboBox.DataSource = _availableMembers;
                selectMemberComboBox.DisplayMember = "FullName";

                memberListBox.DataSource = _selectedMembers;
                memberListBox.DisplayMember = "FullName";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurs. Please try again later");
                Close();
            }
        }

        private void AddMemberButton_Click(object sender, EventArgs e)
        {
            if (selectMemberComboBox.SelectedItem != null)
            {
                PersonModel member = (PersonModel)selectMemberComboBox.SelectedItem;

                _selectedMembers.Add(member);
                _availableMembers.Remove(member);
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

                _availableMembers.Add(member);
                _selectedMembers.Remove(member);
            }
            else
            {
                MessageBox.Show("Please select a member to remove", "Error");
            }
        }

        private async void CreateMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateNewMember() == false)
            {
                return;
            }

            PersonModel person = new()
            {
                FirstName = firstNameTextBox.Text,
                LastName = lastNameTextBox.Text,
                EmailAddress = emailAddressTextBox.Text,
                PhoneNumber = phoneNumberTextBox.Text
            };

            try
            {
                person.Id = await _personEndpoint.CreatePersonAsync(person);

                _selectedMembers.Add(person);

                firstNameTextBox.Clear();
                lastNameTextBox.Clear();
                emailAddressTextBox.Clear();
                phoneNumberTextBox.Clear();
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurs. Please try again later");
            }
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

        private async void CreateTeamButton_Click(object sender, EventArgs e)
        {
            if (ValidateTeam() == false)
            {
                return;
            }

            TeamModel team = new()
            {
                TeamName = teamNameTextBox.Text,
                TeamMembers = _selectedMembers.ToList()
            };

           team.Id = await _teamEndpoint.CreateTeamAsync(team);

            _callingForm.NewTeamComplete(team);

            Close();
        }

        private bool ValidateTeam()
        {
            string errorMessage = "";
            bool output = true;

            if (string.IsNullOrWhiteSpace(teamNameTextBox.Text))
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
