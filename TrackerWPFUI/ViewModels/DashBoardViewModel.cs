using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class DashBoardViewModel : ViewModelBase
    {
        private TournamentModel _selectedTournament;
        private readonly INotificationService _notificationService;

        public DashBoardViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
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
            _notificationService.Subscribe<CreateTournamentCompletedEvent>(this, HandleCreateTournamentCompletedEvent);
            _notificationService.Notify(new CreateTournamentEvent());
        }

        private void HandleCreateTournamentCompletedEvent(object parameter)
        {
            CreateTournamentCompletedEvent message = (CreateTournamentCompletedEvent)parameter;

            if (message.Tournament != null)
            {
                TournamentList.Add(message.Tournament);
            }

            _notificationService.Unsubscribe<CreateTournamentCompletedEvent>(this);
        }

        public bool CanViewTournament(object parameter) => SelectedTournament != null;

        public void ViewTournament(object parameter)
        {
            _notificationService.Subscribe<ViewTournamentEndedEvent>(this, HandleViewTournamentEndedEvent);
            _notificationService.Notify(new ViewTournamentEvent());
        }

        private void HandleViewTournamentEndedEvent(object parameter)
        {
            ViewTournamentEndedEvent message = (ViewTournamentEndedEvent)parameter;

            if (message.EndedTournament != null)
            {
                TournamentList.Remove(message.EndedTournament);
                SelectedTournament = null;
            }

            _notificationService.Unsubscribe<ViewTournamentEndedEvent>(this);
        }
    }
}
