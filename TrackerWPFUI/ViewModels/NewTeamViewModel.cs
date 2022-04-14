using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerWPFUI.Commands;
using TrackerWPFUI.Stores;
using TrackerWPFUI.ViewModels.Base;

namespace TrackerWPFUI.ViewModels
{
    public class NewTeamViewModel : ViewModelBase
    {
        private string _teamName;
        private PersonModel _selectedPlayer;
        private ObservableCollection<PersonModel> _playerList;
        private ObservableCollection<PersonModel> _memberList = new ObservableCollection<PersonModel>();
        private PersonModel _selectedMember;
        private RelayCommand _addMemberCommand;
        private RelayCommand _removeMemberCommand;
        private RelayCommand _createMemberCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _createTeamCommand;
        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _emailAddress;
        private readonly NewTournamentViewModel _newTournamentViewModel;
        private readonly NavigationStore _navigationStore;

        public NewTeamViewModel(NavigationStore navigationStore, NewTournamentViewModel newTournamentViewModel)
        {
            _playerList = new ObservableCollection<PersonModel>(GlobalConfig.connection.GetPerson_All());
            _newTournamentViewModel = newTournamentViewModel;
            _navigationStore = navigationStore;

            AddMemberCommand = new RelayCommand(AddMember, CanAddMember);
            RemoveMemberCommand = new RelayCommand(RemoveMember, CanRemoveMember);
            CreateMemberCommand = new RelayCommand(CreateMember, CanCreateMember);
            CancelCommand = new RelayCommand(Cancel);
            CreateTeamCommand = new RelayCommand(CreateTeam, CanCreateTeam);

            MemberList.CollectionChanged += MemberList_CollectionChanged;
        }

        private void MemberList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _createTeamCommand.OnCanExecuteChanged(this, EventArgs.Empty);
        }

        public string TeamName
        {
            get => _teamName;
            set
            {
                _teamName = value;
                _createTeamCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<PersonModel> PlayerList
        {
            get => _playerList;
            set => _playerList = value;
        }
        public PersonModel SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                _selectedPlayer = value;
                _addMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<PersonModel> MemberList
        {
            get => _memberList;
            set => _memberList = value;
        }
        public PersonModel SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                _removeMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public RelayCommand AddMemberCommand
        {
            get => _addMemberCommand;
            set => _addMemberCommand = value;
        }
        public RelayCommand RemoveMemberCommand
        {
            get => _removeMemberCommand;
            set => _removeMemberCommand = value;
        }
        public RelayCommand CreateMemberCommand
        {
            get => _createMemberCommand;
            set => _createMemberCommand = value;
        }
        public RelayCommand CancelCommand
        {
            get => _cancelCommand;
            set => _cancelCommand = value;
        }
        public RelayCommand CreateTeamCommand
        {
            get => _createTeamCommand;
            set => _createTeamCommand = value;
        }


        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                _createMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                _createMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                _createMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                _createMemberCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }


        private void Cancel(object parameter)
        {
            _navigationStore.CurrentViewModel = _newTournamentViewModel;
        }

        private bool CanRemoveMember(object parameter) => SelectedMember != null;

        private void RemoveMember(object parameter)
        {
            _playerList.Add(SelectedMember);
            _memberList.Remove(SelectedMember);
        }

        private bool CanAddMember(object parameter) => SelectedPlayer != null;

        private void AddMember(object parameter)
        {
            _memberList.Add(SelectedPlayer);
            _playerList.Remove(SelectedPlayer);
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
            TeamModel team = new TeamModel()
            {
                TeamName = TeamName,
                TeamMembers = MemberList.ToList()
            };

            GlobalConfig.connection.SaveNewTeam(team);

            _newTournamentViewModel.EnteredTeam.Add(team);

            _navigationStore.CurrentViewModel = _newTournamentViewModel;
        }
    }
}
