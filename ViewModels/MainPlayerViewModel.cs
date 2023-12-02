using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;

namespace DnDManager.ViewModels
{
	/// <summary>
	/// ViewModel for Main Menu
	/// </summary>
    internal class MainPlayerViewModel : ViewModelBase
    {
		private UserStore _userStore;

		//Welcome message for the Main Player Screen
		public string WelcomeMessage
		{
			get	=> _userStore.CurrentUser is not null? 
					$"Hello, {_userStore.CurrentUser.FirstName}!" : "ERROR: CURRENT USER IS NULL";
			set => OnPropertyChanged(nameof(WelcomeMessage));
		}

		public ParameterNavigateCommand<Character, CharacterModificationViewModel> CreateCharacterCommand { get; }
		public NavigateCommand<CharacterBrowserViewModel> BrowseCharacterCommand { get; }
        public NavigateCommand<CreateGameViewModel> CreateGameCommand { get; }
        public NavigateCommand<GamesBrowserViewModel> BrowseGameCommand { get; }

        public MainPlayerViewModel(UserStore userStore, 
			ParameterNavigationService<Character, CharacterModificationViewModel> characterModificationNS,
			NavigationService<CharacterBrowserViewModel> characterBrowserNS,
			NavigationService<CreateGameViewModel> createGameNS,
            NavigationService<GamesBrowserViewModel> gameBrowserNS)
        {
			_userStore = userStore;
			CreateCharacterCommand = new ParameterNavigateCommand<Character, CharacterModificationViewModel>
				(characterModificationNS, null);
            BrowseCharacterCommand = new NavigateCommand<CharacterBrowserViewModel>(characterBrowserNS);
			BrowseGameCommand = new NavigateCommand<GamesBrowserViewModel>(gameBrowserNS);
			CreateGameCommand = new NavigateCommand<CreateGameViewModel>(createGameNS);
        }



    }
}
