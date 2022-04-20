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
using TrackerWPFUI.Events;
using TrackerWPFUI.Services;
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
        private readonly INotificationService _notificationService;
        private readonly ViewModelValidation _viewModelValidation;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public NewTeamViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            _viewModelValidation = new ViewModelValidation();

            PlayerList = new ObservableCollection<PersonModel>(GlobalConfig.connection.GetPerson_All());

            AddMemberCommand = new RelayCommand(AddMember, CanAddMember);
            RemoveMemberCommand = new RelayCommand(RemoveMember, CanRemoveMember);
            CreateMemberCommand = new RelayCommand(CreateMember, CanCreateMember);
            CancelCommand = new RelayCommand(Cancel);
            CreateTeamCommand = new RelayCommand(CreateTeam, CanCreateTeam);

            MemberList.CollectionChanged += MemberList_CollectionChanged;
            _viewModelValidation.ErrorsChanged += ViewModelValidation_ErrorsChanged;
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

        public bool HasErrors => _viewModelValidation.HasErrors;

        private void Cancel(object parameter)
        {
            _notificationService.Notify(new CreateTeamCompletedEvent(typeof(NewTeamViewModel), null));
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
        private void ValidateMember()
        {
            _viewModelValidation.ClearError(nameof(FirstName));
            _viewModelValidation.ClearError(nameof(LastName));
            _viewModelValidation.ClearError(nameof(EmailAddress));
            _viewModelValidation.ClearError(nameof(PhoneNumber));

            if (FirstName.Length > 100)
            {
                _viewModelValidation.AddError(nameof(FirstName), "First Name cannot be more than 100 character long");
            }

            if (LastName.Length > 100)
            {
                _viewModelValidation.AddError(nameof(LastName), "Last Name cannot be more than 100 character long");
            }

            if (EmailAddress.Length > 200)
            {
                _viewModelValidation.AddError(nameof(EmailAddress), "Email Address cannot be more than 200 character long");
            }
            else if (!Regex.IsMatch(EmailAddress, @"^(?:[\.A-z0-9!#$%&'*+/=?^_`{|}~-]+@(?:[A-z0-9-])+(?:.)+[A-z0-9])$"))
            {
                _viewModelValidation.AddError(nameof(EmailAddress), "Please enter a valid email address");
            }

            if (PhoneNumber.Length > 20)
            {
                _viewModelValidation.AddError(nameof(PhoneNumber), "Phone Number cannot be more than 20 character long");
            }
            else if (!Regex.IsMatch(PhoneNumber, @"^[0-9+\s-]*$"))
            {
                _viewModelValidation.AddError(nameof(PhoneNumber), "Please enter a valid phone number");
            }
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

            _notificationService.Notify(new CreateTeamCompletedEvent(typeof(NewTeamViewModel), team));
        }
        private void ValidateTeam()
        {
            _viewModelValidation.ClearError(nameof(TeamName));

            if (TeamName.Length > 20)
            {
                _viewModelValidation.AddError(nameof(TeamName), "Team Name cannot be more than 20 character long");
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _viewModelValidation.GetErrors(propertyName);
        }

        private void ViewModelValidation_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }
    }
}
