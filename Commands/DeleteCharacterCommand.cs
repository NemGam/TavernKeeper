using DnDManager.ViewModels;
using System;
using System.ComponentModel;

namespace DnDManager.Commands
{
    class DeleteCharacterCommand : CommandBase
    {
        private CharacterBrowserViewModel _characterBrowserVM;

        public DeleteCharacterCommand(CharacterBrowserViewModel characterBrowserVM)
        {
            _characterBrowserVM = characterBrowserVM;
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
            _characterBrowserVM.RemoveSelectedCharacter(_characterBrowserVM.SelectedCharacter!.ID);
        }

        public override bool CanExecute(object? parameter)
        {
            return _characterBrowserVM.SelectedCharacter is not null
                && base.CanExecute(parameter);
        }

    }
}