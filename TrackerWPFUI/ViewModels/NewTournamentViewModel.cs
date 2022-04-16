using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class NewTournamentViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _tournamentName;
        private string _entreefee;
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
        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
            CancelCommand = new RelayCommand(Cancel);

            EnteredTeam.CollectionChanged += EnteredTeam_CollectionChanged;
        }


        private void EnteredTeam_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CreateTournamentCommand.OnCanExecuteChanged(this, EventArgs.Empty);
        }

        public string TournamentName
        {
            get => _tournamentName;
            set
            {
                _tournamentName = value;
                CreateTournamentCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string EntreeFee
        {
            get => _entreefee;
            set
            {
                _entreefee = value;
                CreateTournamentCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<TeamModel> EnteredTeam { get; set; } = new ObservableCollection<TeamModel>();
        public TeamModel TeamToRemove
        {
            get => _teamToRemove;
            set
            {
                _teamToRemove = value;
                RemoveTeamCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<TeamModel> TeamList { get; set; }
        public TeamModel TeamToAdd
        {
            get => _teamToAdd;
            set
            {
                _teamToAdd = value;
                AddTeamCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<PrizeModel> PrizeList { get; set; } = new ObservableCollection<PrizeModel>();
        public bool IsPrizeListErrorVisible => _propertyErrors.ContainsKey(nameof(PrizeList));
        public string PrizeListErrorMessage
        {
            get
            {
                OnPropertyChanged(nameof(IsPrizeListErrorVisible));

                return _propertyErrors.ContainsKey(nameof(PrizeList)) ? _propertyErrors[nameof(PrizeList)].FirstOrDefault() : null;
            }
        }


        public string PrizeName
        {
            get => _prizeName;
            set
            {
                _prizeName = value;
                OnPropertyChanged(nameof(PrizeName));
                CreatePrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string PrizeNumber
        {
            get => _prizeNumber;
            set
            {
                _prizeNumber = value;
                OnPropertyChanged(nameof(PrizeNumber));
                CreatePrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string PrizeAmount
        {
            get => _prizeAmount;
            set
            {
                _prizeAmount = value;
                OnPropertyChanged(nameof(PrizeAmount));
                CreatePrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string PrizePercentage
        {
            get => _prizePercentage;
            set
            {
                _prizePercentage = value;
                OnPropertyChanged(nameof(PrizePercentage));
                CreatePrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
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
                DeletePrizeCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }


        public RelayCommand CreateNewTeamCommand { get; set; }
        public RelayCommand AddTeamCommand { get; set; }
        public RelayCommand RemoveTeamCommand { get; set; }
        public RelayCommand CreatePrizeCommand { get; set; }
        public RelayCommand DeletePrizeCommand { get; set; }
        public RelayCommand CreateTournamentCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public bool HasErrors => _propertyErrors.Any();

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
                return !string.IsNullOrWhiteSpace(PrizeAmount);
            }
            else
            {
                return !string.IsNullOrWhiteSpace(PrizePercentage);
            }
        }

        private void CreatePrize(object parameter)
        {
            ValidatePrize(out int placeNumber, out decimal prizeAmount, out double prizePercentage);

            if (HasErrors)
            {
                return;
            }

            PrizeModel prize = new PrizeModel()
            {
                PrizeName = PrizeName,
                PlaceNumber = placeNumber,
                PrizeAmount = prizeAmount,
                PrizePercentage = prizePercentage
            };

            PrizeList.Add(prize);

            PrizeName = null;
            PrizeNumber = null;
            UsePrizeAmount = true;
        }

        private void ValidatePrize(out int placeNumber, out decimal prizeAmount, out double prizePercentage)
        {
            ClearError(nameof(PrizeName));
            ClearError(nameof(PrizeNumber));
            ClearError(nameof(PrizeAmount));
            ClearError(nameof(PrizePercentage));

            if (PrizeName.Length > 20)
            {
                AddError(nameof(PrizeName), "Prize Name must not exceed 20 character");
                OnErrorsChanged(nameof(PrizeName));
            }

            if (!int.TryParse(PrizeNumber, out placeNumber))
            {
                AddError(nameof(PrizeNumber), "Please enter a valid integer number");
                OnErrorsChanged(nameof(PrizeNumber));
            }

            prizeAmount = 0;
            prizePercentage = 0;
            if (UsePrizeAmount)
            {
                bool isValidPrize = decimal.TryParse(PrizeAmount, out prizeAmount) && prizeAmount > 0;
                if (!isValidPrize)
                {
                    AddError(nameof(PrizeAmount), "Prize Amount must be greater than 0");
                    OnErrorsChanged(nameof(PrizeAmount));
                }
            }
            else
            {
                bool isValidPrize = double.TryParse(PrizePercentage, out prizePercentage) && prizePercentage > 0 && prizePercentage <= 100;
                if (!isValidPrize)
                {
                    AddError(nameof(PrizePercentage), "Prize Percentage must be between 0 to 100");
                    OnErrorsChanged(nameof(PrizePercentage));
                }
            }
        }

        private bool CanDeletePrize(object parameter) => PrizeToDelete != null;

        private void DeletePrize(object parameter)
        {
            PrizeList.Remove(PrizeToDelete);
        }

        private bool CanCreateTournament(object parameter)
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

        private void CreateTournament(object parameter)
        {
            ValidateTournament(out decimal entreeFee);

            if (HasErrors)
            {
                return;
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

        private void ValidateTournament(out decimal entreeFee)
        {
            ClearError(nameof(TournamentName));
            ClearError(nameof(EntreeFee));
            ClearError(nameof(PrizeList));
            OnPropertyChanged(nameof(PrizeListErrorMessage));

            if (TournamentName.Length > 50)
            {
                AddError(nameof(TournamentName), "Tournament Name cannot be more than 50 character long");
                OnErrorsChanged(nameof(TournamentName));
            }

            if (!decimal.TryParse(EntreeFee, out entreeFee) || entreeFee < 0)
            {
                AddError(nameof(EntreeFee), "Please enter a valid positive number");
                OnErrorsChanged(nameof(EntreeFee));
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
                    AddError(nameof(PrizeList), "Total Prize Amount must not exceed Total Collected Fees");
                    OnErrorsChanged(nameof(PrizeList));
                    OnPropertyChanged(nameof(PrizeListErrorMessage));
                }
            }
        }

        private void Cancel(object parameter)
        {
            _navigationStore.CurrentViewModel = _dashBoardViewModel;
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
    }
}
