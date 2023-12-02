using DnDManager.Models;
using DnDManager.Stores;
using DnDManager.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DnDManager.Commands
{
    class DeleteGameCommand : CommandBase
    {
        private readonly GamesBrowserViewModel _gamesBrowserVM;
        private readonly DatabaseProvider _databaseProvider;
        private readonly UserStore _userStore;
        public DeleteGameCommand(GamesBrowserViewModel gamesBrowserVM, DatabaseProvider databaseProvider, UserStore userStore)
        {
            _databaseProvider = databaseProvider;
            _gamesBrowserVM = gamesBrowserVM;
            _gamesBrowserVM.PropertyChanged += OnViewModelPropertyChanged;
            _userStore = userStore;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GamesBrowserViewModel.SelectedGame))
            {
                OnCanExecuteChanged(null);
            }
        }
        public override void Execute(object? parameter)
        {
            try
            {
                Task.Run(async () =>
                {
                    Debug.WriteLine($"{_gamesBrowserVM.SelectedGame!.campaign_id}, {_userStore.CurrentUser.UserName}");
                    string sql = "DELETE FROM campaigns WHERE campaign_id = @id AND created_by = @username;";
                    await _databaseProvider.PostAsync(sql, new { id = _gamesBrowserVM.SelectedGame!.campaign_id, username = _userStore.CurrentUser.UserName});                   
                });
                _gamesBrowserVM.RemoveSelectedGame();
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
            }
            
        }

        public override bool CanExecute(object? parameter)
        {
            return _gamesBrowserVM.SelectedGame is not null
                && base.CanExecute(parameter);
        }

    }
}