using System;
using System.Collections.Generic;
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
        private List<TeamModel> _enteredTeam = new List<TeamModel>();
        private List<TeamModel> _teamList;
        private List<PrizeModel> _prizelist;

        public NewTournamentViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            CreateNewTeamCommand = new RelayCommand(CreateNewTeam);

            TeamList = GlobalConfig.connection.GetTeam_All();
        }

        private void CreateNewTeam(object parameter)
        {
            _navigationStore.CurrentViewModel = new NewTeamViewModel(_navigationStore, this);
        }

        public TournamentModel Tournament { get; set; } = new TournamentModel();

        public List<TeamModel> EnteredTeam
        {
            get { return _enteredTeam; }
            set { _enteredTeam = value; }
        }

        public List<TeamModel> TeamList
        {
            get { return _teamList; }
            set { _teamList = value; }
        }

        public List<PrizeModel> PrizeList
        {
            get { return _prizelist; }
            set { _prizelist = value; }
        }

        private TeamModel _selectedTeam;

        public TeamModel SelectedTeam
        {
            get { return _selectedTeam; }
            set { _selectedTeam = value; }
        }

        private string _tournamentName;

        public string TournamentName
        {
            get { return _tournamentName; }
            set { _tournamentName = value; }
        }

        private decimal _entreefee;
        private readonly NavigationStore _navigationStore;

        public decimal EntreeFee
        {
            get { return _entreefee; }
            set { _entreefee = value; }
        }

        public RelayCommand CreateNewTeamCommand { get; set; }
    }
}
