using DnDManager.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DnDManager.Commands
{
    internal class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginVM;

        public LoginCommand(LoginViewModel loginVM)
        {
            this._loginVM = loginVM;
            loginVM.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.Login) || e.PropertyName == nameof(LoginViewModel.Password)) 
            {
                OnCanExecuteChanged(null);
            }
        }

        public override void Execute(object? parameter)
        {
            Debug.WriteLine($"{_loginVM.Login}, {_loginVM.Password}");
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_loginVM.Login) 
                && !string.IsNullOrEmpty(_loginVM.Password)
                && base.CanExecute(parameter);
        }
    }
}
