using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class TournamentViewerViewModel : ViewModelBase
    {
        private const string _bye = "--- byes ---";
        private const string _notYetSet = "Not Yet Determined";

        private readonly NavigationStore _navigationStore;
        private readonly DashBoardViewModel _dashBoardViewModel;
        private readonly TournamentModel _tournament;
        private int _selectedRoundNumber = 1;
        private bool _upcomingMatchOnly = true;
        private MatchupDisplayModel _selectedMatchup;
        private string _firstTeamScore;
        private string _secondTeamScore;
        private string _firstTeamName;
        private string _secondTeamName;

        public TournamentViewerViewModel(NavigationStore navigationStore, DashBoardViewModel dashBoardViewModel, TournamentModel tournament)
        {
            _navigationStore = navigationStore;
            _dashBoardViewModel = dashBoardViewModel;
            _tournament = tournament;

            LoadRound();

            if (CurrentRound.Count == 0 && SelectedRoundNumber < RoundList.Last())
            {
                SelectedRoundNumber += 1;
            }

            _tournament.OnTournamentComplete += Tournament_OnTournamentComplete;

            CloseCommand = new RelayCommand(Close);
            SaveScoreCommand = new RelayCommand(SaveScore, CanSaveScore);
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            string winner = _tournament.Rounds.Find(x => x.First().MatchupRound == RoundList.Last()).First().Winner.TeamName;
            //TODO Display tournament complete message
            //MessageBox.Show($"Tournament has ended, The winner is { winner }");

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
            else if (FirstTeamScore == SecondTeamScore)
            {
                //TODO display error tie game not handled
                return false;
            }

            return true;
        }

        private void SaveScore(object parameter)
        {
            if (!double.TryParse(FirstTeamScore, out double firstTeamScore) || !double.TryParse(SecondTeamScore, out double secondTeamScore))
            {
                //TODO Display error invalid input value
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
    }
}
