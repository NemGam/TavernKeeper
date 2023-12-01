using DnDManager.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using DnDManager.Services;
using DnDManager.Views;
using DnDManager.Stores;
using DnDManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DnDManager.Commands
{

    internal class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly UserStore _userStore;
        private readonly NavigationService<MainPlayerViewModel> _mainPlayerViewNS;
        private readonly DatabaseProvider _databaseProvider;
        private bool isLoading = false;
        

        public LoginCommand(LoginViewModel loginViewModel, UserStore userStore, DatabaseProvider databaseProvider,
            NavigationService<MainPlayerViewModel> mainPlayerViewNS)
        {
            _loginViewModel = loginViewModel;
            _userStore = userStore;
            _mainPlayerViewNS = mainPlayerViewNS;
            _databaseProvider = databaseProvider;
            loginViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }


        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.UserName) || e.PropertyName == nameof(LoginViewModel.Password)) 
            {
                OnCanExecuteChanged(null);
            }
        }

        public override void Execute(object? parameter)
        {;
            if (string.IsNullOrEmpty(((PasswordBox)parameter!).Password)){
                return;
            }
            Login((PasswordBox) parameter);
        }

        private async void Login(PasswordBox box)
        {
            bool authenticated = true;
            isLoading = true;
            string sql = "SELECT check_password(@username, @pass)";
            var s = await _databaseProvider!.GetAsync<bool>(sql,
                    new { username = _loginViewModel!.UserName!, pass = box.Password });
            if (s == null || s.Count == 0) authenticated = false;
            else authenticated = s[0];
            if (authenticated)
            {
                //Get first name from the users table
                _userStore.CurrentUser = new User(_loginViewModel.UserName!, "Test");
                _loginViewModel.PropertyChanged -= OnViewModelPropertyChanged;
                _mainPlayerViewNS.Navigate();
            }
            else
            {
                _loginViewModel.SetFailedAuthentication();
                MessageBox.Show("Good try, Krish, I've added security, use tester as password");
                isLoading = false;
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_loginViewModel.UserName)
                && !isLoading
                && base.CanExecute(parameter);
        }
    }
}
