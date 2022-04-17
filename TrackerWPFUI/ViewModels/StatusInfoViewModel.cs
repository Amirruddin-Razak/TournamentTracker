using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrackerWPF.Services;
using TrackerWPFUI.Commands;
using TrackerWPFUI.Stores;
using TrackerWPFUI.ViewModels.Base;

namespace TrackerWPFUI.ViewModels
{
    public class StatusInfoViewModel : ViewModelBase
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private string _message;
        private string _header;

        public StatusInfoViewModel(string header, string message, ModalNavigationStore modalNavigationStore)
        {
            Header = header;
            Message = message;
            _modalNavigationStore = modalNavigationStore;
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
            _modalNavigationStore.CurrentViewModel = null;
        }
    }
}
