using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrackerWPFUI.Services;
using TrackerWPFUI.Commands;
using TrackerWPFUI.ViewModels.Base;
using TrackerWPFUI.Events;

namespace TrackerWPFUI.ViewModels
{
    public class StatusInfoViewModel : ViewModelBase
    {
        private readonly INotificationService _notificationService;
        private string _message;
        private string _header;

        public StatusInfoViewModel(string header, string message, INotificationService notificationService)
        {
            Header = header;
            Message = message;
            _notificationService = notificationService;
            CloseCommand = new RelayCommand(Close);
        }

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public RelayCommand CloseCommand { get; }

        private void Close(object parameter)
        {
            _notificationService.Notify(new CloseModalEvent());
        }
    }
}
