using DnDManager.Helpers;
using DnDManager.Models;
using DnDManager.Services;
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
        private readonly NavigationService<GamesBrowserViewModel> mpns;

        public FindGameCommand(FindGameViewModel findGameViewModel, 
            DatabaseProvider databaseProvider, 
            UserStore userStore,
           NavigationService<GamesBrowserViewModel> mpnss)
        {
            mpns = mpnss;
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
            RegisterInGame((PasswordBox)parameter);
        }

        private async void RegisterInGame(PasswordBox box)
        {
            //Get salt for the current user from the database
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

            //Check if the password is correct for the given campaign
            string hashedPass = AuthenticationHelper.HashString(box.Password, salt);
            sql = "SELECT check_campaign(@id, @pass)";
            var s1 = await _databaseProvider!.GetAsync<bool>(sql,
                    new { id = _findGameViewModel.CampaignID, pass = hashedPass });
            if (s1 == null || s1.Count == 0) authenticated = false;
            else authenticated = s1[0];
            
            //If authenticated add user's character into the campaign's pool
            //Else set error message
            if (authenticated)
            {

                _findGameViewModel.PropertyChanged -= OnViewModelPropertyChanged;
                sql = "INSERT INTO campaign_characters (campaign_id, character_id, campaign_role) VALUES (@cid, @ccid, @nyi);";
                await _databaseProvider!.PostAsync(sql, new
                {
                    cid = _findGameViewModel.CampaignID,
                    ccid = _findGameViewModel.SelectedCharacter.ID,
                    nyi = "NYI"
                });
                mpns.Navigate();
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
