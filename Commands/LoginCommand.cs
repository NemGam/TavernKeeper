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

namespace DnDManager.Commands
{
    /*
    public class Usr
    {
        string username;
        string firstname;
        string lastname;
        string pass;

        public Usr(string username, string first_name, string last_name, string hashed_pwd)
        {
            this.username = username;
            this.firstname = first_name;
            this.lastname = last_name;
            this.pass = hashed_pwd;
        }

        public override string ToString()
        {
            return $"{username}, {firstname}, {lastname}, {pass}";
        }
    }
    */

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
            /*
            List<Usr> users = Task.Run(() => _databaseProvider.GetAsync<Usr>("SELECT * FROM Users")).Result;
            foreach(var user in users){
                Debug.WriteLine(user);
            }
            */
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
            Debug.WriteLine("Disposed");
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
