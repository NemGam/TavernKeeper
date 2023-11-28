using DnDManager.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using DnDManager.Services;
using DnDManager.Views;
using DnDManager.Stores;
using DnDManager.Models;

namespace DnDManager.Commands
{
    internal class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly UserStore _userStore;
        private readonly NavigationService<MainPlayerViewModel> _mainPlayerViewNS;

        public LoginCommand(LoginViewModel loginViewModel, UserStore userStore,
            NavigationService<MainPlayerViewModel> mainPlayerViewNS)
        {
            _loginViewModel = loginViewModel;
            _userStore = userStore;
            _mainPlayerViewNS = mainPlayerViewNS;
            loginViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.UserName) || e.PropertyName == nameof(LoginViewModel.Password)) 
            {
                OnCanExecuteChanged(null);
            }
        }

        ~LoginCommand()
        {
            _loginViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            Debug.WriteLine($"{_loginViewModel.UserName}, {_loginViewModel.Password}");
            Login();
        }

        private void Login()
        {
            if (true)
            {
                //if type == Player: Go there, else go to Master
                //Get stuff from DB
                _userStore.CurrentUser = new User(_loginViewModel.UserName, "Test");
                _mainPlayerViewNS.Navigate();
            }
            else
            {

            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_loginViewModel.UserName) 
                && !string.IsNullOrEmpty(_loginViewModel.Password)
                && base.CanExecute(parameter);
        }
    }
}
