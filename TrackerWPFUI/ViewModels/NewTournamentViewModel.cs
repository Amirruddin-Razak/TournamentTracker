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
    public class NewTournamentViewModel : ViewModelBase
    {
        private string _tournamentName;
        private string _entreefee;
        private ObservableCollection<TeamModel> _teamList;
        private ObservableCollection<TeamModel> _enteredTeam = new ObservableCollection<TeamModel>();
        private ObservableCollection<PrizeModel> _prizelist = new ObservableCollection<PrizeModel>();
        private TeamModel _teamToAdd;
        private TeamModel _teamToRemove;
        private string _prizeName;
        private string _prizeNumber;
        private string _prizeAmount = "0";
        private string _prizePercentage;
        private bool _usePrizeAmount = true;
        private PrizeModel _prizeToDelete;
        private readonly NavigationStore _navigationStore;
        private readonly DashBoardViewModel _dashBoardViewModel;
        private RelayCommand _addTeamCommand;
        private RelayCommand _removeTeamCommand;
        private RelayCommand _createPrizeCommand;
        private RelayCommand _deletePrizeCommand;
        private RelayCommand _createTournamentCommand;

        public NewTournamentViewModel(NavigationStore navigationStore, DashBoardViewModel dashBoardViewModel)
        {
            _navigationStore = navigationStore;
            _dashBoardViewModel = dashBoardViewModel;
            TeamList = new ObservableCollection<TeamModel>(GlobalConfig.connection.GetTeam_All());

            CreateNewTeamCommand = new RelayCommand(CreateNewTeam);
            AddTeamCommand = new RelayCommand(AddTeam, CanAddTeam);
            RemoveTeamCommand = new RelayCommand(RemoveTeam, CanRemoveTeam);
            CreatePrizeCommand = new RelayCommand(CreatePrize, CanCreatePrize);
            DeletePrizeCommand = new RelayCommand(DeletePrize, CanDeletePrize);
            CreateTournamentCommand = new RelayCommand(CreateTournament, CanCreateTournament);

            EnteredTeam.CollectionChanged += EnteredTeam_CollectionChanged;
        }

        private void EnteredTeam_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _createTournamentCommand.OnCanExecuteChanged(this, EventArgs.Empty);
        }

        public string TournamentName
        {
            get => _tournamentName;
            set
            {
                _tournamentName = value;
                _createTournamentCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string EntreeFee
        {
            get => _entreefee;
            set
            {
                _entreefee = value;
                _createTournamentCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<TeamModel> EnteredTeam
        {
            get => _enteredTeam;
            set => _enteredTeam = value;
        }
        public TeamModel TeamToRemove
        {
            get => _teamToRemove;
            set
            {
                _teamToRemove = value;
                _removeTeamCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<TeamModel> TeamList
        {
            get => _teamList;
            set => _teamList = value;
        }
        public TeamModel TeamToAdd
        {
            get => _teamToAdd;
            set
            {
                _teamToAdd = value;
                _addTeamCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<PrizeModel> PrizeList
        {
            get => _prizelist;
            set => _prizelist = value;
        }


        public string PrizeName
        {
            get => _prizeName;
            set
            {
                _prizeName = value;
                OnPropertyChanged(nameof(PrizeName));
                _createPrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string PrizeNumber
        {
            get => _prizeNumber;
            set
            {
                _prizeNumber = value;
                OnPropertyChanged(nameof(PrizeNumber));
                _createPrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string PrizeAmount
        {
            get => _prizeAmount;
            set
            {
                _prizeAmount = value;
                OnPropertyChanged(nameof(PrizeAmount));
                _createPrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string PrizePercentage
        {
            get => _prizePercentage;
            set
            {
                _prizePercentage = value;
                OnPropertyChanged(nameof(PrizePercentage));
                _createPrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public bool UsePrizeAmount
        {
            get => _usePrizeAmount;
            set
            {
                _usePrizeAmount = value;
                if (_usePrizeAmount)
                {
                    PrizeAmount = "0";
                    PrizePercentage = null;
                    OnPropertyChanged(nameof(PrizePercentage));
                }
                else
                {
                    PrizeAmount = null;
                    PrizePercentage = "0";
                    OnPropertyChanged(nameof(PrizeAmount));
                }

                OnPropertyChanged(nameof(UsePrizeAmount));
            }
        }
        public PrizeModel PrizeToDelete
        {
            get => _prizeToDelete;
            set
            {
                _prizeToDelete = value;
                _deletePrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }


        public RelayCommand CreateNewTeamCommand { get; set; }
        public RelayCommand AddTeamCommand
        {
            get => _addTeamCommand;
            set => _addTeamCommand = value;
        }
        public RelayCommand RemoveTeamCommand
        {
            get => _removeTeamCommand;
            set => _removeTeamCommand = value;
        }
        public RelayCommand CreatePrizeCommand
        {
            get => _createPrizeCommand;
            set { _createPrizeCommand = value; }
        }
        public RelayCommand DeletePrizeCommand
        {
            get => _deletePrizeCommand;
            set => _deletePrizeCommand = value;
        }
        public RelayCommand CreateTournamentCommand
        {
            get => _createTournamentCommand;
            set => _createTournamentCommand = value;
        }



        private void CreateNewTeam(object parameter)
        {
            _navigationStore.CurrentViewModel = new NewTeamViewModel(_navigationStore, this);
        }

        private bool CanAddTeam(object parameter) => TeamToAdd != null;

        private void AddTeam(object parameter)
        {
            EnteredTeam.Add(TeamToAdd);
            TeamList.Remove(TeamToAdd);
        }

        private bool CanRemoveTeam(object parameter) => TeamToRemove != null;

        private void RemoveTeam(object parameter)
        {
            TeamList.Add(TeamToRemove);
            EnteredTeam.Remove(TeamToRemove);
        }

        private bool CanCreatePrize(object parameter)
        {
            if (string.IsNullOrWhiteSpace(PrizeName))
            {
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(PrizeNumber))
            {
                return false;
            }

            if (UsePrizeAmount)
            {
                return decimal.TryParse(PrizeAmount, out decimal result) &&
                    result > 0;
            }
            else
            {
                return decimal.TryParse(PrizePercentage, out decimal result) &&
                    result > 0 && result <= 100;
            }
        }

        private void CreatePrize(object parameter)
        {
            PrizeModel prize;

            if (UsePrizeAmount)
            {
                bool isValidPrize = decimal.TryParse(PrizeAmount, out decimal result) && result > 0;
                if (!isValidPrize)
                {
                    //TODO Display Error
                    return;
                }
            }
            else
            {
                bool isValidPrize = decimal.TryParse(PrizePercentage, out decimal result) && result > 0 && result <= 100;
                if (!isValidPrize)
                {
                    //TODO Display Error
                    return;
                }
            }

            if (UsePrizeAmount)
            {
                prize = new PrizeModel(PrizeName, PrizeNumber, UsePrizeAmount, PrizeAmount);
            }
            else
            {
                prize = new PrizeModel(PrizeName, PrizeNumber, UsePrizeAmount, PrizePercentage);
            }

            PrizeList.Add(prize);

            PrizeName = null;
            PrizeNumber = null;
            UsePrizeAmount = true;
        }

        private bool CanDeletePrize(object parameter) => PrizeToDelete != null;

        private void DeletePrize(object parameter)
        {
            PrizeList.Remove(PrizeToDelete);
        }

        private bool CanCreateTournament(object arg)
        {
            if (string.IsNullOrWhiteSpace(TournamentName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(EntreeFee))
            {
                EntreeFee = "0";
            }

            if (EnteredTeam.Count < 2)
            {
                return false;
            }

            return true;
        }

        private void CreateTournament(object obj)
        {
            if (!decimal.TryParse(EntreeFee, out decimal entreeFee) || entreeFee < 0)
            {
                //TODO display Error
                return;
            }

            if (PrizeList.Count > 0)
            {
                decimal totalIncome = entreeFee * EnteredTeam.Count;

                decimal totalPrize = 0;
                foreach (PrizeModel prize in PrizeList)
                {
                    totalPrize += prize.CalculatePrize(totalIncome);
                }

                if (totalPrize > totalIncome)
                {
                    //TODO display error
                    return;
                }
            }

            TournamentModel tournament = new TournamentModel()
            {
                TournamentName = TournamentName,
                EntreeFee = entreeFee,
                Prizes = PrizeList.ToList(),
                TeamList = EnteredTeam.ToList()
            };

            TournamentLogic.CreateNewTournament(tournament);

            _dashBoardViewModel.TournamentList.Add(tournament);
            _navigationStore.CurrentViewModel = _dashBoardViewModel;
        }
    }
}
