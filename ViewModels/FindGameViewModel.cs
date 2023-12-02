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
    internal class FindGameViewModel : ViewModelBase
    {
        private readonly DatabaseProvider _databaseProvider;
        private readonly UserStore _userStore;
        private ObservableCollection<SimplifiedCharacter> _charactersList;

        private bool _failedGameFind;
        public bool FailedGameFind
        {
            get => _failedGameFind;
            set
            {
                _failedGameFind = value;
                OnPropertyChanged(nameof(FailedGameFind));
            }

        }

        public int CampaignID { get; set; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ObservableCollection<SimplifiedCharacter> CharactersList
        {
            get => _charactersList;
            set
            {
                _charactersList = value;
                OnPropertyChanged(nameof(CharactersList));
            }
        }
        private SimplifiedCharacter? _selectedCharacter;

        public SimplifiedCharacter? SelectedCharacter
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                OnPropertyChanged(nameof(SelectedCharacter));
            }
        }

        public NavigateCommand<GamesBrowserViewModel> GoBackCommand { get; }
        public FindGameCommand FindGameCommand { get; }

        public FindGameViewModel(UserStore userStore, DatabaseProvider databaseProvider,
            NavigationService<GamesBrowserViewModel> gameBrowserViewModelNS)
        {
            _userStore = userStore;
            _databaseProvider = databaseProvider;
            Task.Run(async () =>
            {
                string sql = "SELECT * FROM simplified_characters_view WHERE owner_username = @username";
                CharactersList =
                     new ObservableCollection<SimplifiedCharacter>(await _databaseProvider.GetAsync<SimplifiedCharacter>(sql,
                     new { username = _userStore.CurrentUser.UserName }));
                Debug.WriteLine(CharactersList[0].ID);
            });
            GoBackCommand = new NavigateCommand<GamesBrowserViewModel>(gameBrowserViewModelNS);
            FindGameCommand = new FindGameCommand(this, _databaseProvider, _userStore);
        }

        internal void SetFailedGameFind()
        {
            FailedGameFind = true;
        }
    }
}
