using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerWPFUI.Commands;
using TrackerWPFUI.Stores;
using TrackerWPFUI.ViewModels.Base;

namespace TrackerWPFUI.ViewModels
{
    public class NewTeamViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _teamName;
        private PersonModel _selectedPlayer;
        private PersonModel _selectedMember;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _emailAddress;
        private readonly NewTournamentViewModel _newTournamentViewModel;
        private readonly NavigationStore _navigationStore;
        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public NewTeamViewModel(NavigationStore navigationStore, NewTournamentViewModel newTournamentViewModel)
        {
            _newTournamentViewModel = newTournamentViewModel;
            _navigationStore = navigationStore;

            PlayerList = new ObservableCollection<PersonModel>(GlobalConfig.connection.GetPerson_All());

            AddMemberCommand = new RelayCommand(AddMember, CanAddMember);
            RemoveMemberCommand = new RelayCommand(RemoveMember, CanRemoveMember);
            CreateMemberCommand = new RelayCommand(CreateMember, CanCreateMember);
            CancelCommand = new RelayCommand(Cancel);
            CreateTeamCommand = new RelayCommand(CreateTeam, CanCreateTeam);

            MemberList.CollectionChanged += MemberList_CollectionChanged;
        }

        private void MemberList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CreateTeamCommand.OnCanExecuteChanged(this, EventArgs.Empty);
        }

        public string TeamName
        {
            get => _teamName;
            set
            {
                _teamName = value;

                CreateTeamCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<PersonModel> PlayerList { get; set; }
        public PersonModel SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                _selectedPlayer = value;
                AddMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<PersonModel> MemberList { get; set; } = new ObservableCollection<PersonModel>();
        public PersonModel SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                RemoveMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public RelayCommand AddMemberCommand { get; set; }
        public RelayCommand RemoveMemberCommand { get; set; }
        public RelayCommand CreateMemberCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand CreateTeamCommand { get; set; }


        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                CreateMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                CreateMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                CreateMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                CreateMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool HasErrors => _propertyErrors.Any();

        private void Cancel(object parameter)
        {
            _navigationStore.CurrentViewModel = _newTournamentViewModel;
        }

        private bool CanRemoveMember(object parameter) => SelectedMember != null;

        private void RemoveMember(object parameter)
        {
            PlayerList.Add(SelectedMember);
            MemberList.Remove(SelectedMember);
        }

        private bool CanAddMember(object parameter) => SelectedPlayer != null;

        private void AddMember(object parameter)
        {
            MemberList.Add(SelectedPlayer);
            PlayerList.Remove(SelectedPlayer);
        }

        private bool CanCreateMember(object arg)
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                !string.IsNullOrWhiteSpace(LastName) &&
                !string.IsNullOrWhiteSpace(EmailAddress) &&
                !string.IsNullOrWhiteSpace(PhoneNumber);
        }

        private void CreateMember(object parameter)
        {
            ValidateMember();

            if (HasErrors)
            {
                return;
            }

            PersonModel member = new PersonModel()
            {
                FirstName = FirstName,
                LastName = LastName,
                EmailAddress = EmailAddress,
                PhoneNumber = PhoneNumber
            };

            GlobalConfig.connection.SaveNewPerson(member);

            MemberList.Add(member);

            FirstName = null;
            LastName = null;
            EmailAddress = null;
            PhoneNumber = null;

            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(EmailAddress));
            OnPropertyChanged(nameof(PhoneNumber));
        }

        private bool CanCreateTeam(object parameter) => !string.IsNullOrWhiteSpace(TeamName) && MemberList.Count != 0;

        private void CreateTeam(object parameter)
        {
            ValidateTeam();

            if (HasErrors)
            {
                return;
            }

            TeamModel team = new TeamModel()
            {
                TeamName = TeamName,
                TeamMembers = MemberList.ToList()
            };

            GlobalConfig.connection.SaveNewTeam(team);

            _newTournamentViewModel.EnteredTeam.Add(team);

            _navigationStore.CurrentViewModel = _newTournamentViewModel;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            _propertyErrors.TryGetValue(propertyName, out List<string> output);
            return output;
        }

        private void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }

            _propertyErrors[propertyName].Add(errorMessage);

            OnErrorsChanged(propertyName);
        }

        private void ClearError(string propertyName)
        {
            if (_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ValidateMember()
        {
            ClearError(nameof(FirstName));
            ClearError(nameof(LastName));
            ClearError(nameof(EmailAddress));
            ClearError(nameof(PhoneNumber));

            if (FirstName.Length > 100)
            {
                AddError(nameof(FirstName), "First Name cannot be more than 100 character long");
                OnErrorsChanged(nameof(FirstName));
            }

            if (LastName.Length > 100)
            {
                AddError(nameof(LastName), "Last Name cannot be more than 100 character long");
                OnErrorsChanged(nameof(LastName));
            }

            if (EmailAddress.Length > 200)
            {
                AddError(nameof(EmailAddress), "Email Address cannot be more than 200 character long");
                OnErrorsChanged(nameof(EmailAddress));
            }
            else if (!Regex.IsMatch(EmailAddress, @"^(?:[\.A-z0-9!#$%&'*+/=?^_`{|}~-]+@(?:[A-z0-9-])+(?:.)+[A-z0-9])$"))
            {
                AddError(nameof(EmailAddress), "Please enter a valid email address");
                OnErrorsChanged(nameof(EmailAddress));
            }

            if (PhoneNumber.Length > 20)
            {
                AddError(nameof(PhoneNumber), "Phone Number cannot be more than 20 character long");
                OnErrorsChanged(nameof(PhoneNumber));
            }
            else if (!Regex.IsMatch(PhoneNumber, @"^[0-9+\s-]*$"))
            {
                AddError(nameof(PhoneNumber), "Please enter a valid phone number");
                OnErrorsChanged(nameof(PhoneNumber));
            }
        }

        private void ValidateTeam()
        {
            ClearError(nameof(TeamName));

            if (TeamName.Length > 20)
            {
                AddError(nameof(TeamName), "Team Name cannot be more than 20 character long");
                OnErrorsChanged(nameof(TeamName));
            }
        }
    }
}
