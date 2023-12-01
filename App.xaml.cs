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
            //TEMP
            const bool def = true;
            if (def)
            {
                _navigationStore.CurrentViewModel = CreateLoginViewModel();
            }
            else
            {
                _userStore.CurrentUser = new User("Vlad", "Test");
                _navigationStore.CurrentViewModel = new GamesBrowserViewModel(_userStore, _dbProvider,
                    new NavigationService<MainPlayerViewModel>(_navigationStore, CreateMainPlayerViewModel));
            
                //_navigationStore.CurrentViewModel = new CharacterModificationViewModel(new Models.Character("Vlad"), new Models.DatabaseProvider());
            }

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();
            base.OnStartup(e);
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
                new NavigationService<CharacterBrowserViewModel>(_navigationStore, CreateCharacterBrowserViewModel));
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
                new ParameterNavigationService<Character, CharacterModificationViewModel>(_navigationStore, CreateCharacterModificationViewModel));
        }

        private RegistrationViewModel CreateRegistrationViewModel()
        {
            return new RegistrationViewModel(_dbProvider,
                new NavigationService<LoginViewModel>(_navigationStore, CreateLoginViewModel));
        }
    }
}
