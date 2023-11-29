using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DnDManager.ViewModels
{
    class RegistrationViewModel : ViewModelBase
    {
		private string? _firstName;
		public string? FirstName
        {
			get
			{
				return _firstName;
			}
			set
			{
				_firstName = value;
				OnPropertyChanged(nameof(FirstName));
			}
		}

		private string? _lastName;
		public string? LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				_lastName = value;
				OnPropertyChanged(nameof(LastName));
			}
		}

		private string? _username;
		public string? UserName
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
				OnPropertyChanged(nameof(UserName));
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

        public ICommand RegisterCommand { get; }
        public NavigateCommand<LoginViewModel> GoToLoginCommand { get; }

        public RegistrationViewModel(DatabaseProvider databaseProvider,
			NavigationService<LoginViewModel> loginViewNavigationService)
        {
			RegisterCommand = new RegisterCommand(this, databaseProvider);
			GoToLoginCommand = new NavigateCommand<LoginViewModel>(loginViewNavigationService);
        }
    }
}
