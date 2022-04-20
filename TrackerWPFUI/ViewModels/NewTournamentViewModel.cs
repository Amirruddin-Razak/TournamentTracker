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
using TrackerWPFUI.Events;
using TrackerWPFUI.Services;
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
        private readonly ViewModelValidation _viewModelValidation;
        private readonly INotificationService _notificationService;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public NewTournamentViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
            _viewModelValidation = new ViewModelValidation();

            TeamList = new ObservableCollection<TeamModel>(GlobalConfig.connection.GetTeam_All());

            CreateNewTeamCommand = new RelayCommand(CreateNewTeam);
            AddTeamCommand = new RelayCommand(AddTeam, CanAddTeam);
            RemoveTeamCommand = new RelayCommand(RemoveTeam, CanRemoveTeam);
            CreatePrizeCommand = new RelayCommand(CreatePrize, CanCreatePrize);
            DeletePrizeCommand = new RelayCommand(DeletePrize, CanDeletePrize);
            CreateTournamentCommand = new RelayCommand(CreateTournament, CanCreateTournament);
            CancelCommand = new RelayCommand(Cancel);

            EnteredTeam.CollectionChanged += EnteredTeam_CollectionChanged;
            _viewModelValidation.ErrorsChanged += ViewModelValidation_ErrorsChanged;
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
        public bool IsPrizeListErrorVisible => _viewModelValidation.HasSpecificError(nameof(PrizeList));
        public string PrizeListErrorMessage
        {
            get
            {
                OnPropertyChanged(nameof(IsPrizeListErrorVisible));

                return GetErrors(nameof(PrizeList))?.Cast<string>().First();
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

        public bool HasErrors => _viewModelValidation.HasErrors;

        private void CreateNewTeam(object parameter)
        {
            _notificationService.Subscribe<CreateTeamCompletedEvent>(this, HandleCreateTeamCompletedEvent);
            _notificationService.Notify(new CreateTeamEvent());
        }

        private void HandleCreateTeamCompletedEvent(object parameter)
        {
            CreateTeamCompletedEvent message = (CreateTeamCompletedEvent)parameter;

            if (message.Team != null)
            {
                EnteredTeam.Add(message.Team);
            }

            _notificationService.Unsubscribe<CreateTeamCompletedEvent>(this);
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
            _viewModelValidation.ClearError(nameof(PrizeName));
            _viewModelValidation.ClearError(nameof(PrizeNumber));
            _viewModelValidation.ClearError(nameof(PrizeAmount));
            _viewModelValidation.ClearError(nameof(PrizePercentage));

            if (PrizeName.Length > 20)
            {
                _viewModelValidation.AddError(nameof(PrizeName), "Prize Name must not exceed 20 character");
            }

            if (!int.TryParse(PrizeNumber, out placeNumber))
            {
                _viewModelValidation.AddError(nameof(PrizeNumber), "Please enter a valid integer number");
            }

            prizeAmount = 0;
            prizePercentage = 0;
            if (UsePrizeAmount)
            {
                bool isValidPrize = decimal.TryParse(PrizeAmount, out prizeAmount) && prizeAmount > 0;
                if (!isValidPrize)
                {
                    _viewModelValidation.AddError(nameof(PrizeAmount), "Prize Amount must be greater than 0");
                }
            }
            else
            {
                bool isValidPrize = double.TryParse(PrizePercentage, out prizePercentage) && prizePercentage > 0 && prizePercentage <= 100;
                if (!isValidPrize)
                {
                    _viewModelValidation.AddError(nameof(PrizePercentage), "Prize Percentage must be between 0 to 100");
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

            _notificationService.Notify(new CreateTournamentCompletedEvent(typeof(NewTournamentViewModel), tournament));
        }
        private void ValidateTournament(out decimal entreeFee)
        {
            _viewModelValidation.ClearError(nameof(TournamentName));
            _viewModelValidation.ClearError(nameof(EntreeFee));
            _viewModelValidation.ClearError(nameof(PrizeList));
            OnPropertyChanged(nameof(PrizeListErrorMessage));

            if (TournamentName.Length > 50)
            {
                _viewModelValidation.AddError(nameof(TournamentName), "Tournament Name cannot be more than 50 character long");
            }

            if (!decimal.TryParse(EntreeFee, out entreeFee) || entreeFee < 0)
            {
                _viewModelValidation.AddError(nameof(EntreeFee), "Please enter a valid positive number");
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
                    _viewModelValidation.AddError(nameof(PrizeList), "Total Prize Amount must not exceed Total Collected Fees");
                    OnPropertyChanged(nameof(PrizeListErrorMessage));
                }
            }
        }

        private void Cancel(object parameter)
        {
            _notificationService.Notify(new CreateTournamentCompletedEvent(typeof(NewTournamentViewModel), null));
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
