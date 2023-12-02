using DnDManager.Helpers;
using DnDManager.Models;
using DnDManager.Stores;
using DnDManager.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DnDManager.Commands
{
    internal class FindGameCommand : CommandBase
    {
        private readonly FindGameViewModel _findGameViewModel;
        private readonly DatabaseProvider _databaseProvider;
        private readonly UserStore _userStore;

        public FindGameCommand(FindGameViewModel findGameViewModel, DatabaseProvider databaseProvider, UserStore userStore)
        {
            this._findGameViewModel = findGameViewModel;
            this._databaseProvider = databaseProvider;
            this._userStore = userStore;
            findGameViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FindGameViewModel.CampaignID)
                || e.PropertyName == nameof(FindGameViewModel.IsBusy)
                || e.PropertyName == nameof(FindGameViewModel.SelectedCharacter))
            {
                OnCanExecuteChanged(null);
            }
        }

        public override void Execute(object? parameter)
        {
            if (string.IsNullOrEmpty(((PasswordBox)parameter!).Password))
            {
                return;
            }
            Login((PasswordBox)parameter);
        }

        private async void Login(PasswordBox box)
        {
            _findGameViewModel.IsBusy = true;
            bool authenticated = true;
            byte[] salt;
            string sql = "SELECT pass_salt FROM campaigns WHERE campaign_id = @id";
            var s = await _databaseProvider!.GetAsync<string>(sql,
                    new { id = _findGameViewModel.CampaignID });
            if (s == null || s.Count == 0)
            {
                _findGameViewModel.SetFailedGameFind();
                _findGameViewModel.IsBusy = false;
                return;
            }
            salt = Convert.FromHexString(s[0]);
            string hashedPass = AuthenticationHelper.HashString(box.Password, salt);
            sql = "SELECT check_campaign(@id, @pass)";
            var s1 = await _databaseProvider!.GetAsync<bool>(sql,
                    new { id = _findGameViewModel.CampaignID, pass = hashedPass });
            if (s1 == null || s1.Count == 0) authenticated = false;
            else authenticated = s1[0];
            

            if (authenticated)
            {
                /*
                _findGameViewModel.PropertyChanged -= OnViewModelPropertyChanged;

                sql = "SELECT first_name FROM users WHERE username = @username";
                var s2 = await _databaseProvider!.GetAsync<string>(sql,
                    new { username = _loginViewModel.UserName! });
                string fname = s2[0];
                _userStore.CurrentUser = new User(_loginViewModel.UserName!, fname);
                _mainPlayerViewNS.Navigate();
                */
            }
            else
            {
                _findGameViewModel.SetFailedGameFind();
                _findGameViewModel.IsBusy = false;
            }

        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_findGameViewModel.CampaignID.ToString())
                && !_findGameViewModel.IsBusy
                && _findGameViewModel.SelectedCharacter is not null
                && base.CanExecute(parameter);
        }
    }
}
