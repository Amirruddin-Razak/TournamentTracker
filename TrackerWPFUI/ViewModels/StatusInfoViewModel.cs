using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrackerWPFUI.Commands;
using TrackerWPFUI.ViewModels.Base;

namespace TrackerWPFUI.ViewModels
{
    public class StatusInfoViewModel : ViewModelBase
    {
        public StatusInfoViewModel()
        {
            CloseCommand = new RelayCommand(Close);
        }

        public string Header { get; set; }
        public string Message { get; set; }

        public RelayCommand CloseCommand { get; }

        private void Close(object parameter)
        {
            Window window = (Window)parameter;
            window.Close();
        }
    }
}
