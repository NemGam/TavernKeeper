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
				return $"Hello, {_userStore.CurrentUser.FirstName}!";
			}
			set
			{ 
				OnPropertyChanged(nameof(WelcomeMessage));
			}
		}

		public NavigateCommand<CharacterModificationViewModel> CreateCharacterCommand { get; }
		public NavigateCommand<ViewModelBase> BrowseCharacterCommand { get; }
        public NavigateCommand<ViewModelBase> ContinueGameCommand { get; }
        public NavigateCommand<ViewModelBase> BrowseGameCommand { get; }

        public MainPlayerViewModel(UserStore userStore, 
			NavigationService<CharacterModificationViewModel> characterModificationNS)
        {
			_userStore = userStore;
			CreateCharacterCommand = new NavigateCommand<CharacterModificationViewModel>(characterModificationNS);
        }



    }
}
