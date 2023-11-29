using DnDManager.Commands;
using DnDManager.Services;
using DnDManager.Stores;

namespace DnDManager.ViewModels
{
    internal class MainPlayerViewModel : ViewModelBase
    {
		private UserStore _userStore;

		//Welcome message for the Main Player Screen
		public string WelcomeMessage
		{
			get
			{
				return _userStore.CurrentUser is not null? 
					$"Hello, {_userStore.CurrentUser.FirstName}!" : "ERROR: CURRENT USER IS NULL";
			}
			set
			{ 
				OnPropertyChanged(nameof(WelcomeMessage));
			}
		}

		public NavigateCommand<CharacterModificationViewModel> CreateCharacterCommand { get; }
		public NavigateCommand<CharacterBrowserViewModel> BrowseCharacterCommand { get; }
        public NavigateCommand<ViewModelBase> ContinueGameCommand { get; }
        public NavigateCommand<ViewModelBase> BrowseGameCommand { get; }

        public MainPlayerViewModel(UserStore userStore, 
			NavigationService<CharacterModificationViewModel> characterModificationNS,
			NavigationService<CharacterBrowserViewModel> characterBrowserNS
			)
        {
			_userStore = userStore;
			CreateCharacterCommand = new NavigateCommand<CharacterModificationViewModel>(characterModificationNS);
            BrowseCharacterCommand = new NavigateCommand<CharacterBrowserViewModel>(characterBrowserNS);
        }



    }
}
