using DnDManager.Helpers;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;
using DnDManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DnDManager.Commands
{
    internal class CreateGameCommand : CommandBase
    {
        private readonly CreateGameViewModel _createGameViewModel;
        private readonly DatabaseProvider _databaseProvider;
        private readonly UserStore _userStore;
        private readonly NavigationService<GameViewModel> _gameViewModelNS;
        private readonly NavigationService<MainPlayerViewModel> _mainPlayerViewModelNS;



        public CreateGameCommand(CreateGameViewModel createGameViewModel, UserStore userStore, DatabaseProvider databaseProvider,
            NavigationService<GameViewModel> gameViewModelNS, NavigationService<MainPlayerViewModel> mainPlayerViewModelNS)
        {
            _mainPlayerViewModelNS = mainPlayerViewModelNS;
            _createGameViewModel = createGameViewModel;
            _databaseProvider = databaseProvider;
            _gameViewModelNS = gameViewModelNS;
            _userStore = userStore;
            createGameViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == nameof(CreateGameViewModel.CampaignName)
                || e.PropertyName == nameof(CreateGameViewModel.IsBusy))
                {
                    OnCanExecuteChanged(null);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        public override void Execute(object? parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(((PasswordBox)parameter!).Password))
                {
                    return;
                }
                _createGameViewModel.IsBusy = true;
                var box = (PasswordBox)parameter;
                Task.Run(async () =>
                {
                    try
                    {
                        string sql = "INSERT INTO campaigns (created_by, created_at, campaign_name, password, pass_salt) VALUES (@created_by, CURRENT_TIMESTAMP, @campaign_name, @password, @pass_salt);";

                        string hashedPass = AuthenticationHelper.GenerateNewHashedString(box.Password, out string salt);
                        await _databaseProvider!.PostAsync(sql,
                            new
                            {
                                created_by = _userStore.CurrentUser.UserName,
                                password = hashedPass,
                                pass_salt = salt,
                                campaign_name = _createGameViewModel.CampaignName
                            });
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                    finally
                    {
                        _createGameViewModel.PropertyChanged -= OnViewModelPropertyChanged;
                    }
                    _mainPlayerViewModelNS.Navigate();
                    

                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_createGameViewModel.CampaignName)
                && !_createGameViewModel.IsBusy
                && base.CanExecute(parameter);
        }
    }
}

