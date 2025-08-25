using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.ViewModels
{
    /// <summary>
    /// Games browser logic (note: games and campaigns are the same)
    /// </summary>
    internal class GamesBrowserViewModel : ViewModelBase
    {
        private readonly DatabaseProvider _databaseProvider;
        private readonly UserStore _userStore;
        private ObservableCollection<GameModel> _gamesList;
        public ObservableCollection<GameModel> GamesList
        {
            get => _gamesList;
            set
            {
                _gamesList = value;
                OnPropertyChanged(nameof(GamesList));
            }
        }
        private GameModel? _selectedGame;
        public GameModel? SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
            }
        }

        public NavigateCommand<MainPlayerViewModel> GoBackCommand { get; }
        public DeleteGameCommand DeleteGameCommand { get; }
        public NavigateCommand<FindGameViewModel> GoToFindGameCommand { get; }

        public GamesBrowserViewModel(UserStore userStore, DatabaseProvider databaseProvider,
            NavigationService<MainPlayerViewModel> mainPlayerViewModelNS,
            NavigationService<FindGameViewModel> findGameViewModelNS) 
        {
            _userStore = userStore;
            _databaseProvider = databaseProvider;
            Task.Run(async () =>
            {
                string sql = "SELECT DISTINCT c.campaign_id, c.campaign_name, c.created_by, c.created_at, ch.character_name FROM campaigns c LEFT JOIN campaign_characters cc ON c.campaign_id = cc.campaign_id LEFT JOIN users u ON c.created_by = u.username LEFT JOIN characters ch ON cc.character_id = ch.id WHERE u.username = @username OR ch.owner_username = @username;";
                _gamesList = new ObservableCollection<GameModel>(await _databaseProvider.GetAsync<GameModel>(sql,
                     new { username = _userStore.CurrentUser.UserName}));
                OnPropertyChanged(nameof(GamesList));
            });
            GoBackCommand = new NavigateCommand<MainPlayerViewModel>(mainPlayerViewModelNS);
            DeleteGameCommand = new DeleteGameCommand(this, _databaseProvider, _userStore);
            GoToFindGameCommand = new NavigateCommand<FindGameViewModel>(findGameViewModelNS);
        }

        /// <summary>
        /// Remove selected game from the local list (not DB)
        /// </summary>
        internal void RemoveSelectedGame()
        {
            if (_selectedGame is null) return;
            if (SelectedGame.created_by != _userStore.CurrentUser.UserName) return;
            try
            {
                _gamesList.Remove(_selectedGame);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            SelectedGame = null;
        }
    }
}
