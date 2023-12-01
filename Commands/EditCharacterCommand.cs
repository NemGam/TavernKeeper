using DnDManager.Models;
using DnDManager.Services;
using DnDManager.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DnDManager.Commands
{
    class EditCharacterCommand : CommandBase
    {
        private CharacterBrowserViewModel _characterBrowserVM;
        private DatabaseProvider _databaseProvider;
        private ParameterNavigationService<Character, CharacterModificationViewModel> _characterModPNS;
        private bool finalizing = false; //Will block button just before the transition

        public EditCharacterCommand(CharacterBrowserViewModel characterBrowserVM, DatabaseProvider databaseProvider,
            ParameterNavigationService<Character, CharacterModificationViewModel> characterModPNS)
        {
            _characterBrowserVM = characterBrowserVM;
            _databaseProvider = databaseProvider;
            _characterModPNS = characterModPNS;
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
            _characterBrowserVM.PropertyChanged -= OnViewModelPropertyChanged;
            finalizing = true;
            Task.Run(async () => {
                Debug.WriteLine(_characterBrowserVM.SelectedCharacter!.ID);
                var character = await _databaseProvider.GetAsync<Character>(
                "SELECT * FROM characters WHERE characters.id = @id", new { id = _characterBrowserVM.SelectedCharacter!.ID});
                if (character is null) 
                {
                    Debug.WriteLine("No characters were found");
                    return;
                }
                Debug.WriteLine(character.Count);
                _characterModPNS.Navigate(character[0]);
            });
        }

        public override bool CanExecute(object? parameter)
        {
            return _characterBrowserVM.SelectedCharacter is not null 
                && !finalizing
                && base.CanExecute(parameter);
        }
    }
}
