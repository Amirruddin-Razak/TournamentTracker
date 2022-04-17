using System;
using System.Collections.Generic;
using System.Text;
using TrackerWPFUI.ViewModels.Base;

namespace TrackerWPFUI.Stores
{
    public class ModalNavigationStore
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        public bool IsOpen => CurrentViewModel != null;

        public event Action CurrentViewModelChanged;
    }
}
