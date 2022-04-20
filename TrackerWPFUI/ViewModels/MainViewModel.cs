using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerWPFUI.ViewModels.Base;
using TrackerWPFUI.Services;
using TrackerWPFUI.Events;

namespace TrackerWPFUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Dictionary<Type, ViewModelBase> _activeViewModels = new Dictionary<Type, ViewModelBase>();
        private readonly IServiceProvider _serviceProvider;
        private readonly INotificationService _notificationService;
        private ViewModelBase _currentModalViewModel;
        private ViewModelBase _currentViewModel;

        public MainViewModel(IServiceProvider serviceProvider, INotificationService notificationService)
        {
            _serviceProvider = serviceProvider;
            _notificationService = notificationService;

            NavigateToNewViewModel(typeof(DashBoardViewModel));

            ConfigureViewModelNavigation();
        }

        private void ConfigureViewModelNavigation()
        {
            _notificationService.Subscribe<CloseModalEvent>(this,
                (object parameter) => CurrentModalViewModel = null);

            _notificationService.Subscribe<CreateTeamEvent>(this,
                (object parameter) => NavigateToNewViewModel(typeof(NewTeamViewModel)));

            _notificationService.Subscribe<CreateTournamentEvent>(this,
                (object parameter) => NavigateToNewViewModel(typeof(NewTournamentViewModel)));

            _notificationService.Subscribe<ViewTournamentEvent>(this,
                (object parameter) => NavigateToNewViewModel(typeof(TournamentViewerViewModel)));

            _notificationService.Subscribe<CreateTeamCompletedEvent>(this,
                (object parameter) => NavigateToPreviousViewModel(typeof(NewTournamentViewModel), ((CreateTeamCompletedEvent)parameter).CallerType));

            _notificationService.Subscribe<CreateTournamentCompletedEvent>(this,
                (object parameter) => NavigateToPreviousViewModel(typeof(DashBoardViewModel), ((CreateTournamentCompletedEvent)parameter).CallerType));

            _notificationService.Subscribe<ViewTournamentEndedEvent>(this, HandleViewTournamentEndedEvent);
        }

        private void HandleViewTournamentEndedEvent(object parameter)
        {
            ViewTournamentEndedEvent message = (ViewTournamentEndedEvent)parameter;

            if (message.StatusInfo != null)
            {
                CurrentModalViewModel = message.StatusInfo;
            }

            NavigateToPreviousViewModel(typeof(DashBoardViewModel), message.CallerType);
        }

        private void NavigateToNewViewModel(Type viewModelType)
        {
            _activeViewModels.Add(viewModelType, (ViewModelBase)_serviceProvider.GetService(viewModelType));

            CurrentViewModel = _activeViewModels[viewModelType];
        }

        private void NavigateToPreviousViewModel(Type viewModelType, Type callerType)
        {
            _activeViewModels.Remove(callerType);

            CurrentViewModel = _activeViewModels[viewModelType];
        }


        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ViewModelBase CurrentModalViewModel
        {
            get
            {
                return _currentModalViewModel;
            }
            set
            {
                _currentModalViewModel = value;
                OnPropertyChanged(nameof(CurrentModalViewModel));
                OnPropertyChanged(nameof(IsModalOpen));
            }
        }

        public bool IsModalOpen => CurrentModalViewModel != null;
    }
}
