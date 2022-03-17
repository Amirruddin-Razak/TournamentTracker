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
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class NewTeamForm : Form
    {
        private List<PersonModel> availableMembers = GlobalConfig.connection.GetPerson_All();
        private List<PersonModel> selectedMembers = new List<PersonModel>();

        public NewTeamForm()
        {
            InitializeComponent();
            //CreateSampleData();
            UpdateList();
        }

        private void CreateSampleData() 
        {
            availableMembers.Add(new PersonModel()
            {
                FirstName = "Amir",
                LastName = "Din",
                EmailAddress = "AmDin@Mail.com",
                PhoneNumber = "120-923431"
            });

            availableMembers.Add(new PersonModel()
            {
                FirstName = "Storm",
                LastName = "Sue",
                EmailAddress = "SueStorm@Mail.com",
                PhoneNumber = "130-316819"
            });

            selectedMembers.Add(new PersonModel()
            {
                FirstName = "Beth",
                LastName = "Cow",
                EmailAddress = "CowBeth@Mail.com",
                PhoneNumber = "101-827530"
            });

            selectedMembers.Add(new PersonModel()
            {
                FirstName = "John",
                LastName = "Snow",
                EmailAddress = "SnowJohn@Mail.com",
                PhoneNumber = "111-820930"
            });
        }

        private void UpdateList()
        {
            selectMemberComboBox.DataSource = null;
            selectMemberComboBox.DataSource = availableMembers;
            selectMemberComboBox.DisplayMember = "FullName";

            memberListBox.DataSource = null;
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

                UpdateList();
            }
            else
            {
                MessageBox.Show("Please select a member to add","Error");
            }
        }

        private void CreateMemberButton_Click(object sender, EventArgs e)
        {
            string errorMessage = "";

            if (ValidateNewMember(ref errorMessage))
            {
                PersonModel person = new PersonModel() 
                { 
                    FirstName = firstNameTextBox.Text, 
                    LastName = lastNameTextBox.Text, 
                    EmailAddress = emailAddressTextBox.Text,
                    PhoneNumber = phoneNumberTextBox.Text
                };

                person = GlobalConfig.connection.CreatePerson(person);

                selectedMembers.Add(person);

                UpdateList();

                firstNameTextBox.Clear();
                lastNameTextBox.Clear();
                emailAddressTextBox.Clear();
                phoneNumberTextBox.Clear();
            }
            else
            {
                MessageBox.Show(errorMessage, "Error");
            }
        }

        private bool ValidateNewMember(ref string errorMessage)
        {
            bool output = true;

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
            else if (!Regex.IsMatch(emailAddressTextBox.Text, @"^(?:[\.A-z0-9!#$%&'*+/=?^_`{|}~-]+@(?:[A-z0-9-])+(?:.)+[a-z0-9])$"))
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

            return output;
        }

        private void RemoveMemberButton_Click(object sender, EventArgs e)
        {
            if (memberListBox.SelectedItem != null)
            {
                PersonModel member = (PersonModel)memberListBox.SelectedItem;

                availableMembers.Add(member);
                selectedMembers.Remove(member);

                UpdateList();
            }
            else
            {
                MessageBox.Show("Please select a member to remove", "Error");
            }
        }

        private void CreateTeamButton_Click(object sender, EventArgs e)
        {
            string errorMessage = "";
            bool validTeamName = true;

            if (teamNameTextBox.Text == null)
            {
                errorMessage += "Please enter a team name";
                validTeamName = false;
            }
            else if (teamNameTextBox.Text.Length > 20)
            {
                errorMessage += "Team name cannot be more than 20 characters long";
                validTeamName = false;
            }

            if (validTeamName)
            {
                TeamModel team = new TeamModel
                {
                    TeamName = teamNameTextBox.Text,
                    TeamMembers = selectedMembers
                };

                team = GlobalConfig.connection.CreateTeam(team);

                //TO DO close form or reset form 
            }
            else
            {
                MessageBox.Show(errorMessage, "Error");
            }
        }
    }
}
