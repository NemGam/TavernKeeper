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
    internal class GamesBrowserViewModel : ViewModelBase
    {
        private readonly DatabaseProvider _databaseProvider;
        private readonly UserStore _userStore;
        private ObservableCollection<GameModel> _simplifiedCharacterList;
        public ObservableCollection<GameModel> SimplifiedCharacterList
        {
            get => _simplifiedCharacterList;
            set
            {
                _simplifiedCharacterList = value;
                OnPropertyChanged(nameof(SimplifiedCharacterList));
            }
        }
        private SimplifiedCharacter? _selectedGame;
        public SimplifiedCharacter? SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
            }
        }

        public NavigateCommand<MainPlayerViewModel> GoBackCommand { get; }

        public GamesBrowserViewModel(UserStore userStore, DatabaseProvider databaseProvider,
            NavigationService<MainPlayerViewModel> mainPlayerViewModelNS) 
        {
            _userStore = userStore;
            _databaseProvider = databaseProvider;
            Task.Run(async () =>
            {
                string sql = "SELECT DISTINCT c.campaign_name FROM campaigns c JOIN campaign_characters cc ON c.campaign_id = cc.campaign_id JOIN users u ON cc.character_id = @username WHERE cc.character_id = @username OR c.created_by = @username;";
                SimplifiedCharacterList =
                     new ObservableCollection<GameModel>(await _databaseProvider.GetAsync<GameModel>(sql,
                     new { username = _userStore.CurrentUser.UserName}));
                Debug.WriteLine(SimplifiedCharacterList[0].created_by);
            });
            GoBackCommand = new NavigateCommand<MainPlayerViewModel>(mainPlayerViewModelNS);
        }
    }
}
