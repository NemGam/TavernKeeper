using DnDManager.Models;
using DnDManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Commands
{
    class RegisterCommand : CommandBase
    {
        private readonly RegistrationViewModel _registrationViewModel;
        private readonly DatabaseProvider _databaseProvider;

        public RegisterCommand(RegistrationViewModel registrationViewModel, DatabaseProvider databaseProvider)
        {
            _registrationViewModel = registrationViewModel;
            _databaseProvider = databaseProvider;
            registrationViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RegistrationViewModel.FirstName)
                || e.PropertyName == nameof(RegistrationViewModel.LastName)
                || e.PropertyName == nameof(RegistrationViewModel.UserName)
                || e.PropertyName == nameof(RegistrationViewModel.Password))
            {
                OnCanExecuteChanged(null);
            }
        }

        ~RegisterCommand() 
        {
            _registrationViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        public override void Execute(object? parameter)
        {
            //Add user to the database
            throw new NotImplementedException();
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_registrationViewModel.FirstName)
                && !string.IsNullOrEmpty(_registrationViewModel.LastName)
                && !string.IsNullOrEmpty(_registrationViewModel.UserName)
                && !string.IsNullOrEmpty(_registrationViewModel.Password)
                && base.CanExecute(parameter);
        }
    }
}
