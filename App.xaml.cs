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
            _dbProvider = new DatabaseProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //_navigationStore.CurrentViewModel = CreateLoginViewModel();
            _navigationStore.CurrentViewModel = new CharacterBrowserViewModel();
            
            //_navigationStore.CurrentViewModel = new CharacterModificationViewModel(new Models.Character("Vlad"), new Models.DatabaseProvider());
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
                new NavigationService<MainPlayerViewModel>(_navigationStore, 
                () => CreateMainPlayerViewModel(_userStore)));
        }

        private MainPlayerViewModel CreateMainPlayerViewModel(UserStore userStore)
        {
            return new MainPlayerViewModel(userStore, 
                new NavigationService<CharacterModificationViewModel>(_navigationStore, 
                () => CreateCharacterModificationViewModel(userStore)));
        }

        private CharacterModificationViewModel CreateCharacterModificationViewModel(UserStore userStore)
        {
            return new CharacterModificationViewModel(userStore, new Models.Character(), new Models.DatabaseProvider(), 
                new NavigationService<MainPlayerViewModel>(_navigationStore, () => CreateMainPlayerViewModel(_userStore)));
        }

        private RegistrationViewModel CreateRegistrationViewModel()
        {
            return new RegistrationViewModel(new NavigationService<LoginViewModel>(_navigationStore, CreateLoginViewModel));
        }
    }
}
