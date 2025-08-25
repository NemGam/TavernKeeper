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
	/// <summary>
	/// ViewModel for registration form.
	/// </summary>
    class RegistrationViewModel : ViewModelBase
    {
		private bool _failedRegistration = false;
		private bool _isBusy = false;
        public bool IsBusy
		{
			get => _isBusy;
			set
			{
				_isBusy = value;
				OnPropertyChanged(nameof(IsBusy));
			}
		}
        public bool FailedRegistration
		{
			get => _failedRegistration;
			set 
			{
                _failedRegistration = value;
				OnPropertyChanged(nameof(FailedRegistration));
            }

        }


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

        public RegisterCommand RegisterCommand { get; }
        public NavigateCommand<LoginViewModel> GoToLoginCommand { get; }

        public RegistrationViewModel(DatabaseProvider databaseProvider,
			NavigationService<LoginViewModel> loginViewNavigationService)
        {
			RegisterCommand = new RegisterCommand(this, databaseProvider, loginViewNavigationService);
			GoToLoginCommand = new NavigateCommand<LoginViewModel>(loginViewNavigationService);
        }
		/// <summary>
		/// Show error message
		/// </summary>
        public void SetFailedRegistration()
        {
            FailedRegistration = true;
        }
    }
}
