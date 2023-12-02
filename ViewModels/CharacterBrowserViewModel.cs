using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DnDManager.ViewModels
{
    internal class CharacterBrowserViewModel : ViewModelBase
    {
        private readonly DatabaseProvider _databaseProvider;
        private readonly UserStore _userStore;
        private ObservableCollection<SimplifiedCharacter> _simplifiedCharacterList;
        public ObservableCollection<SimplifiedCharacter> SimplifiedCharacterList
        {
            get => _simplifiedCharacterList;
            set
            {
                _simplifiedCharacterList = value;
                OnPropertyChanged(nameof(SimplifiedCharacterList));
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

        public void RemoveSelectedCharacter(Int64 id)
        {
            if (_selectedCharacter is null) return;
            try
            {
                _simplifiedCharacterList.Remove(_selectedCharacter);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
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
            Task.Run(async () =>
            {
                string sql = "SELECT * FROM simplified_characters_view WHERE owner_username = @username";
                SimplifiedCharacterList =
                     new ObservableCollection<SimplifiedCharacter>(await _databaseProvider.GetAsync<SimplifiedCharacter>(sql, 
                     new { username = _userStore.CurrentUser.UserName }));
            });
            GoBackCommand = new NavigateCommand<MainPlayerViewModel>(mainPlayerViewModelNS);
            DeleteCharacterCommand = new DeleteCharacterCommand(this, _databaseProvider);
            EditCharacterCommand = new EditCharacterCommand(this, databaseProvider, characterModPNS);
        }
    }
}
