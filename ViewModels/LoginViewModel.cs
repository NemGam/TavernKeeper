using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;
using System.Windows;

namespace DnDManager.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private bool _failedAuthentication = false;
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
        public bool FailedAuthentication
        {
            get => _failedAuthentication;
            set
            {
                _failedAuthentication = value;
                OnPropertyChanged(nameof(FailedAuthentication));
            }

        }

        

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
            LoginCommand = new LoginCommand(this, userStore, DBProvider, mainPlayerViewModelNS);
            GoToRegistrationCommand = new NavigateCommand<RegistrationViewModel>(registrationViewModelNS);
        }

        public void SetFailedAuthentication()
        {
            IsBusy = false;
            FailedAuthentication = true;
        }




    }
}
