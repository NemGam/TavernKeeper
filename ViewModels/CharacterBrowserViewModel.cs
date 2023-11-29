using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.ViewModels
{
    internal class CharacterBrowserViewModel : ViewModelBase
    {
        private readonly DatabaseProvider _databaseProvider;
        private readonly UserStore _userStore;

        public ObservableCollection<SimplifiedCharacter> SimplifiedCharacterList { get; private set; }

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

        public void RemoveSelectedCharacter(int id)
        {
            if (_selectedCharacter is null) return;
            SimplifiedCharacterList.Remove(_selectedCharacter);
            //_databaseProvider.DeleteCharacter(id);
            SelectedCharacter = null;
        }

        public NavigateCommand<MainPlayerViewModel> GoBackCommand { get; }
        public EditCharacterCommand EditCharacterCommand { get; }
        public DeleteCharacterCommand DeleteCharacterCommand { get; }
        public CharacterBrowserViewModel(UserStore userStore, DatabaseProvider databaseProvider, 
            NavigationService<MainPlayerViewModel> mainPlayerViewModelNS,
            ParameterNavigationService<Character, CharacterModificationViewModel> characterModPNS)
        {
            _userStore = userStore;
            _databaseProvider = databaseProvider;
            FillCharactersList();
            
            GoBackCommand = new NavigateCommand<MainPlayerViewModel>(mainPlayerViewModelNS);
            EditCharacterCommand = new EditCharacterCommand(this, databaseProvider, characterModPNS);
            DeleteCharacterCommand = new DeleteCharacterCommand(this);
        }

        private async void FillCharactersList()
        {
            string sql = "SELECT * FROM simplified_chracters_view WHERE owner_username = @username";
            SimplifiedCharacterList = 
                 new ObservableCollection<SimplifiedCharacter>(await _databaseProvider.GetAsync<SimplifiedCharacter>(sql, new {username = _userStore.CurrentUser.UserName}));
        }
    }
}
