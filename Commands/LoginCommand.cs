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
using System.Net;
using DnDManager.Helpers;

namespace DnDManager.Commands
{

    internal class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly UserStore _userStore;
        private readonly NavigationService<MainPlayerViewModel> _mainPlayerViewNS;
        private readonly DatabaseProvider _databaseProvider;
        

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
            if (e.PropertyName == nameof(LoginViewModel.UserName) 
                || e.PropertyName == nameof(LoginViewModel.Password)
                || e.PropertyName == nameof(LoginViewModel.IsBusy))
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
            _loginViewModel.IsBusy = true;
            bool authenticated = true;
            byte[] salt;
            string sql = "SELECT pass_salt FROM users WHERE username = @username";
            var s = await _databaseProvider!.GetAsync<string>(sql,
                    new { username = _loginViewModel!.UserName!});
            if (s == null || s.Count == 0) {
                _loginViewModel.SetFailedAuthentication();
                _loginViewModel.IsBusy = false;
                return;
            }
            salt = Convert.FromHexString(s[0]);
            string hashedPass = AuthenticationHelper.HashString(box.Password, salt);
            sql = "SELECT check_password(@username, @pass)";
            var s1 = await _databaseProvider!.GetAsync<bool>(sql,
                    new { username = _loginViewModel!.UserName!, pass = hashedPass });
            if (s1 == null || s1.Count == 0) authenticated = false;
            else authenticated = s1[0];
            if (authenticated)
            {
                
                _loginViewModel.PropertyChanged -= OnViewModelPropertyChanged;
                sql = "SELECT first_name FROM users WHERE username = @username";
                var s2 = await _databaseProvider!.GetAsync<string>(sql,
                    new { username = _loginViewModel.UserName!});
                string fname = s2[0];
                _userStore.CurrentUser = new User(_loginViewModel.UserName!, fname);
                _mainPlayerViewNS.Navigate();
            }
            else
            {
                _loginViewModel.SetFailedAuthentication();
                _loginViewModel.IsBusy = false;
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_loginViewModel.UserName)
                && !_loginViewModel.IsBusy
                && base.CanExecute(parameter);
        }
    }
}
