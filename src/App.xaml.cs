// Ignore Spelling: App

using DnDManager.ViewModels;
using DnDManager.Services;
using System;
using System.Windows;
using DnDManager.Stores;
using System.Reflection.Metadata;
using DnDManager.Models;

namespace DnDManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly NavigationStore _navigationStore;
        private readonly UserStore _userStore;
        private readonly DatabaseProvider _dbProvider;

        public App()
        {
            _navigationStore = new NavigationStore();
            _userStore = new UserStore();
            _dbProvider = DatabaseProvider.Create();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = CreateLoginViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        private GamesBrowserViewModel CreateGameBrowserViewModel()
        {
            return new GamesBrowserViewModel(_userStore, _dbProvider,
                new NavigationService<MainPlayerViewModel>(_navigationStore, CreateMainPlayerViewModel),
                new NavigationService<FindGameViewModel>(_navigationStore, CreateFindGameViewModel));
        }

        private FindGameViewModel CreateFindGameViewModel()
        {
            return new FindGameViewModel(_userStore, _dbProvider,
                new NavigationService<GamesBrowserViewModel>(_navigationStore, CreateGameBrowserViewModel));
        }

        private LoginViewModel CreateLoginViewModel()
        {
            return new LoginViewModel(_dbProvider, _userStore,
                new NavigationService<RegistrationViewModel>(_navigationStore, CreateRegistrationViewModel), 
                new NavigationService<MainPlayerViewModel>(_navigationStore, CreateMainPlayerViewModel));
        }

        private MainPlayerViewModel CreateMainPlayerViewModel()
        {
            return new MainPlayerViewModel(_userStore, 
                new ParameterNavigationService<Character, CharacterModificationViewModel>(_navigationStore, 
                (character) => CreateCharacterModificationViewModel(character)),
                new NavigationService<CharacterBrowserViewModel>(_navigationStore, CreateCharacterBrowserViewModel),
                new NavigationService<CreateGameViewModel>(_navigationStore, CreateCreateGameViewModel),
                new NavigationService<GamesBrowserViewModel>(_navigationStore, CreateGameBrowserViewModel));
        }

        private CreateGameViewModel CreateCreateGameViewModel()
        {
            return new CreateGameViewModel(_dbProvider, _userStore,
                new NavigationService<MainPlayerViewModel>(_navigationStore, CreateMainPlayerViewModel));
        }

        private CharacterModificationViewModel CreateCharacterModificationViewModel(Character character)
        {
            return new CharacterModificationViewModel(_userStore, character, _dbProvider, 
                new NavigationService<MainPlayerViewModel>(_navigationStore, CreateMainPlayerViewModel));
        }

        private CharacterBrowserViewModel CreateCharacterBrowserViewModel()
        {
            return new CharacterBrowserViewModel(_userStore, _dbProvider,
                new NavigationService<MainPlayerViewModel>(_navigationStore, CreateMainPlayerViewModel),
                new ParameterNavigationService<Character, CharacterModificationViewModel>(_navigationStore,
                (character) => CreateCharacterModificationViewModel(character)));
        }

        private RegistrationViewModel CreateRegistrationViewModel()
        {
            return new RegistrationViewModel(_dbProvider,
                new NavigationService<LoginViewModel>(_navigationStore, CreateLoginViewModel));
        }
    }
}
