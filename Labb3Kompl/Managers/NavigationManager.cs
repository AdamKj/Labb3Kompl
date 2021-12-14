using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace Labb3Kompl.Managers
{
    class NavigationManager
    {
        private ObservableObject _currentView;
        public ObservableObject CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        public event Action CurrentViewModelChanged;
    }
}
