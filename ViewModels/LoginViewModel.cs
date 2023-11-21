using DnDManager.Commands;
using DnDManager.Models;
using System.Windows.Input;

namespace DnDManager.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly LoginProvider _loginProvider;

        private string? _login;
        public string? Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }


        private string? _password;
        public string? Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _loginProvider = new LoginProvider();
            LoginCommand = new LoginCommand(this);
        }




    }
}
