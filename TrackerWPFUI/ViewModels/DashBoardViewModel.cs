using System;
using System.Collections.Generic;
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
    public class DashBoardViewModel : ViewModelBase
    {
        private TournamentModel _selectedTournament;
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public DashBoardViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore)
        {
            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;
            TournamentList = new ObservableCollection<TournamentModel>(GlobalConfig.connection.GetTournament_All().FindAll(x => x.Active));

            CreateTournamentCommand = new RelayCommand(CreateTournament);
            ViewTournamentCommand = new RelayCommand(ViewTournament, CanViewTournament);
        }

        public ObservableCollection<TournamentModel> TournamentList { get; set; } = new ObservableCollection<TournamentModel>();
        public TournamentModel SelectedTournament
        {
            get => _selectedTournament;
            set
            {
                _selectedTournament = value;
                ViewTournamentCommand.OnCanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public RelayCommand CreateTournamentCommand { get; set; }
        public RelayCommand ViewTournamentCommand { get; set; }


        public void CreateTournament(object parameter)
        {
            _navigationStore.CurrentViewModel = new NewTournamentViewModel(_navigationStore, this);
        }

        public bool CanViewTournament(object parameter) => SelectedTournament != null;

        public void ViewTournament(object parameter)
        {
            _navigationStore.CurrentViewModel = new TournamentViewerViewModel(_navigationStore, _modalNavigationStore, this, SelectedTournament);
        }
    }
}
