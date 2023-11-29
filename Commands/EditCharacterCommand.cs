using DnDManager.Models;
using DnDManager.ViewModels;
using System;
using System.ComponentModel;

namespace DnDManager.Commands
{
    class EditCharacterCommand : CommandBase
    {
        private CharacterBrowserViewModel _characterBrowserVM;
        private DatabaseProvider _databaseProvider;

        public EditCharacterCommand(CharacterBrowserViewModel characterBrowserVM, DatabaseProvider databaseProvider)
        {
            _characterBrowserVM = characterBrowserVM;
            _databaseProvider = databaseProvider;
            characterBrowserVM.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CharacterBrowserViewModel.SelectedCharacter))
            {
                OnCanExecuteChanged(null);
            }
        }

        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public override bool CanExecute(object? parameter)
        {
            return _characterBrowserVM.SelectedCharacter is not null 
                && base.CanExecute(parameter);
        }
    }
}
