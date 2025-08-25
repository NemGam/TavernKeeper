﻿using DnDManager.Stores;
using DnDManager.ViewModels;
using System;

namespace DnDManager
{
    /// <summary>
    /// Main ViewModel that contains Current ViewModel 
    /// </summary>
    internal class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel { get => _navigationStore.CurrentViewModel; }

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
