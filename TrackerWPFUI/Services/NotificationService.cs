using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerWPFUI.ViewModels;
using TrackerWPFUI.Views;

namespace TrackerWPF.Services
{
    public class NotificationService
    {
        private readonly StatusInfoView _statusInfoView;
        private readonly StatusInfoViewModel _statusInfoViewModel;

        public NotificationService()
        {
            _statusInfoView = new StatusInfoView();
            _statusInfoViewModel = new StatusInfoViewModel();
        }

        public void NotifyUser(string header, string message)
        {
            _statusInfoViewModel.Header = header;
            _statusInfoViewModel.Message = message;
            _statusInfoView.ShowDialog();
        }
    }
}
