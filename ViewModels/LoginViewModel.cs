using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;

namespace DnDManager.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly LoginProvider _loginProvider;

        private string? _userName;
        public string? UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public LoginCommand LoginCommand { get; }
        public NavigateCommand<RegistrationViewModel> GoToRegistrationCommand { get; }

        public LoginViewModel(DatabaseProvider DBProvider, UserStore userStore, NavigationService<RegistrationViewModel> registrationViewModelNS, 
            NavigationService<MainPlayerViewModel> mainPlayerViewModelNS)
        {
            _loginProvider = new LoginProvider(DBProvider);
            LoginCommand = new LoginCommand(this, userStore, DBProvider, mainPlayerViewModelNS);
            GoToRegistrationCommand = new NavigateCommand<RegistrationViewModel>(registrationViewModelNS);
        }

        public override void Dispose()
        {
            base.Dispose();
        }




    }
}
