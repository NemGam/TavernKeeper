using DnDManager.Helpers;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DnDManager.Commands
{
    class RegisterCommand : CommandBase
    {
        private readonly RegistrationViewModel _registrationViewModel;
        private readonly DatabaseProvider _databaseProvider;
        private readonly NavigationService<LoginViewModel> _loginVMNS;

        

        public RegisterCommand(RegistrationViewModel registrationViewModel, DatabaseProvider databaseProvider, 
            NavigationService<LoginViewModel> loginVMNS)
        {
            _registrationViewModel = registrationViewModel;
            _databaseProvider = databaseProvider;
            registrationViewModel.PropertyChanged += OnViewModelPropertyChanged;
            _loginVMNS = loginVMNS;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == nameof(RegistrationViewModel.FirstName)
                || e.PropertyName == nameof(RegistrationViewModel.LastName)
                || e.PropertyName == nameof(RegistrationViewModel.UserName)
                || e.PropertyName == nameof(RegistrationViewModel.IsBusy))
                {
                    OnCanExecuteChanged(null);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
        }

        public override void Execute(object? parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(((PasswordBox)parameter!).Password))
                {
                    return;
                }
                _registrationViewModel.IsBusy = true;
                var box = (PasswordBox)parameter;
                Task.Run(async () =>
                {
                    bool registered = false;
                    string sql = "SELECT register(@username, @pass, @pass_salt, @fname, @lname);";

                    string hashedPass = AuthenticationHelper.GenerateNewHashedString(box.Password, out string salt);
                    var s = await _databaseProvider!.GetAsync<bool>(sql,
                        new
                        {
                            username = _registrationViewModel!.UserName!,
                            pass = hashedPass,
                            pass_salt = salt,
                            fname = _registrationViewModel.FirstName,
                            lname = _registrationViewModel.LastName,
                        });
                    if (s == null || s.Count == 0) registered = false;
                    else registered = s[0];
                    Debug.WriteLine(registered);

                    if (registered)
                    {
                        _registrationViewModel.PropertyChanged -= OnViewModelPropertyChanged;
                        _loginVMNS.Navigate();
                    }
                    else
                    {
                        _registrationViewModel.SetFailedRegistration();
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            _registrationViewModel.IsBusy = false;
                        });
                    }

                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_registrationViewModel.FirstName)
                && !string.IsNullOrEmpty(_registrationViewModel.LastName)
                && !string.IsNullOrEmpty(_registrationViewModel.UserName)
                && !_registrationViewModel.IsBusy
                && base.CanExecute(parameter);
        }
    }
}
