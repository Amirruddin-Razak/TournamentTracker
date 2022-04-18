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
using TrackerWPF.Models;
using TrackerWPFUI.Commands;
using TrackerWPFUI.Stores;
using TrackerWPFUI.ViewModels.Base;

namespace TrackerWPFUI.ViewModels
{
    public class TournamentViewerViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private const string _bye = "--- byes ---";
        private const string _notYetSet = "Not Yet Determined";

        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly DashBoardViewModel _dashBoardViewModel;
        private readonly TournamentModel _tournament;
        private int _selectedRoundNumber = 1;
        private bool _upcomingMatchOnly = true;
        private MatchupDisplayModel _selectedMatchup;
        private string _firstTeamScore;
        private string _secondTeamScore;
        private string _firstTeamName;
        private string _secondTeamName;
        private readonly ViewModelValidation _viewModelValidation;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public TournamentViewerViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore, DashBoardViewModel dashBoardViewModel,
            TournamentModel tournament)
        {
            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;
            _dashBoardViewModel = dashBoardViewModel;
            _tournament = tournament;
            _viewModelValidation = new ViewModelValidation();

            LoadRound();

            if (CurrentRound.Count == 0 && SelectedRoundNumber < RoundList.Last())
            {
                SelectedRoundNumber += 1;
            }

            CloseCommand = new RelayCommand(Close);
            SaveScoreCommand = new RelayCommand(SaveScore, CanSaveScore);

            _tournament.OnTournamentComplete += Tournament_OnTournamentComplete;
            _viewModelValidation.ErrorsChanged += ViewModelValidation_ErrorsChanged;
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            string winner = _tournament.Rounds.Find(x => x.First().MatchupRound == RoundList.Last()).First().Winner.TeamName;

            string header = "Tournament Ended";
            string message = $"Team { winner } has won the tournament";
            _modalNavigationStore.CurrentViewModel = new StatusInfoViewModel(header, message, _modalNavigationStore);

            _dashBoardViewModel.TournamentList.Remove(_tournament);
            Close(null);
        }


        public string TournamentName => _tournament.TournamentName;
        public List<int> RoundList => Enumerable.Range(1, _tournament.Rounds.Count).ToList();
        public int SelectedRoundNumber
        {
            get => _selectedRoundNumber;
            set
            {
                _selectedRoundNumber = value;
                LoadRound();
            }
        }
        public bool UpcomingMatchOnly
        {
            get => _upcomingMatchOnly;
            set
            {
                _upcomingMatchOnly = value;
                LoadRound();
            }
        }

        public ObservableCollection<MatchupDisplayModel> CurrentRound { get; set; }
        public MatchupDisplayModel SelectedMatchup
        {
            get => _selectedMatchup;
            set
            {
                _selectedMatchup = value;
                FirstTeamName = _selectedMatchup?.FirstTeamName;
                FirstTeamScore = _selectedMatchup?.FirstTeamScore.ToString();
                SecondTeamName = _selectedMatchup?.SecondTeamName;
                SecondTeamScore = _selectedMatchup?.SecondTeamScore.ToString();
            }
        }

        public string FirstTeamName
        {
            get => _firstTeamName; set
            {
                _firstTeamName = value;
                OnPropertyChanged(nameof(FirstTeamName));
                OnPropertyChanged(nameof(CanScoreFirstTeam));
            }
        }
        public string FirstTeamScore
        {
            get => _firstTeamScore;
            set
            {
                _firstTeamScore = value;
                OnPropertyChanged(nameof(FirstTeamScore));
                SaveScoreCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public string SecondTeamName
        {
            get => _secondTeamName; set
            {
                _secondTeamName = value;
                OnPropertyChanged(nameof(SecondTeamName));
                OnPropertyChanged(nameof(CanScoreSecondTeam));
            }
        }
        public string SecondTeamScore
        {
            get => _secondTeamScore;
            set
            {
                _secondTeamScore = value;
                OnPropertyChanged(nameof(SecondTeamScore));
                SaveScoreCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }
        public bool CanScoreFirstTeam => FirstTeamName != null && FirstTeamName != _bye && FirstTeamName != _notYetSet;
        public bool CanScoreSecondTeam => SecondTeamName != null && SecondTeamName != _bye && SecondTeamName != _notYetSet;

        public RelayCommand CloseCommand { get; set; }
        public RelayCommand SaveScoreCommand { get; set; }

        public bool HasErrors => _viewModelValidation.HasErrors;

        private void Close(object parameter)
        {
            _navigationStore.CurrentViewModel = _dashBoardViewModel;
        }

        private bool CanSaveScore(object parameter)
        {
            if (SelectedMatchup == null)
            {
                return false;
            }
            else if (CanScoreFirstTeam == false || CanScoreSecondTeam == false)
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(FirstTeamScore) || string.IsNullOrWhiteSpace(SecondTeamScore))
            {
                return false;
            }

            return true;
        }
        private void SaveScore(object parameter)
        {
            ValidateScore(out double firstTeamScore, out double secondTeamScore);

            if (HasErrors)
            {
                return;
            }

            MatchupModel currentMatchup = _tournament.Rounds[SelectedRoundNumber - 1].Find(x => x.Id == SelectedMatchup.MatchupId);

            currentMatchup.Entries[0].Score = SelectedMatchup.FirstTeamScore = firstTeamScore;
            currentMatchup.Entries[1].Score = SelectedMatchup.SecondTeamScore = secondTeamScore;

            TournamentLogic.UpdateTournamentResult(_tournament);

            LoadRound();

            if (CurrentRound.Count == 0 && SelectedRoundNumber < RoundList.Last())
            {
                SelectedRoundNumber += 1;
            }
        }
        private void ValidateScore(out double firstTeamScore, out double secondTeamScore)
        {
            _viewModelValidation.ClearError(nameof(FirstTeamScore));
            _viewModelValidation.ClearError(nameof(SecondTeamScore));

            if (!double.TryParse(FirstTeamScore, out firstTeamScore))
            {
                _viewModelValidation.AddError(nameof(FirstTeamScore), "Please Enter a valid score");
            }

            if (!double.TryParse(SecondTeamScore, out secondTeamScore))
            {
                _viewModelValidation.AddError(nameof(SecondTeamScore), "Please Enter a valid score");
            }

            if (firstTeamScore == secondTeamScore)
            {
                _viewModelValidation.AddError(nameof(FirstTeamScore), "Tie game are not allowed");
                _viewModelValidation.AddError(nameof(SecondTeamScore), "Tie game are not allowed");
            }
        }

        private void LoadRound()
        {
            ObservableCollection<MatchupDisplayModel> currentDisplayRound = new ObservableCollection<MatchupDisplayModel>();

            foreach (MatchupModel matchup in _tournament.Rounds[SelectedRoundNumber - 1])
            {
                MatchupDisplayModel matchupDisplay = new MatchupDisplayModel();
                matchupDisplay.MatchupId = matchup.Id;

                if (matchup.Entries.Count == 1)
                {
                    matchupDisplay.FirstTeamName = matchup.Entries[0].TeamCompeting.TeamName;
                    matchupDisplay.FirstTeamScore = matchup.Entries[0].Score;
                    matchupDisplay.SecondTeamName = _bye;
                    matchupDisplay.SecondTeamScore = 0;
                }
                else
                {
                    if (matchup.Entries[0].TeamCompeting != null)
                    {
                        matchupDisplay.FirstTeamName = matchup.Entries[0].TeamCompeting.TeamName;
                        matchupDisplay.FirstTeamScore = matchup.Entries[0].Score;
                    }
                    else
                    {
                        matchupDisplay.FirstTeamName = _notYetSet;
                        matchupDisplay.FirstTeamScore = 0;
                    }

                    if (matchup.Entries[1].TeamCompeting != null)
                    {
                        matchupDisplay.SecondTeamName = matchup.Entries[1].TeamCompeting.TeamName;
                        matchupDisplay.SecondTeamScore = matchup.Entries[1].Score;
                    }
                    else
                    {
                        matchupDisplay.SecondTeamName = _notYetSet;
                        matchupDisplay.SecondTeamScore = 0;
                    }
                }

                if (UpcomingMatchOnly)
                {
                    if (matchupDisplay.FirstTeamScore == matchupDisplay.SecondTeamScore &&
                        matchupDisplay.SecondTeamScore == 0 &&
                        matchupDisplay.SecondTeamName != _bye)
                    {
                        currentDisplayRound.Add(matchupDisplay);
                    }
                }
                else
                {
                    currentDisplayRound.Add(matchupDisplay);
                }
            }

            CurrentRound = currentDisplayRound;

            OnPropertyChanged(nameof(CurrentRound));
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
