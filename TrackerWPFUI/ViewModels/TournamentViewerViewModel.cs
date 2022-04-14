using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerWPFUI.Stores;
using TrackerWPFUI.ViewModels.Base;

namespace TrackerWPFUI.ViewModels
{
    public class TournamentViewerViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public TournamentViewerViewModel(NavigationStore navigationStore, TournamentModel tournament)
        {
            _navigationStore = navigationStore;
        }
    }
}
