using DnDManager.Models;
using DnDManager.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DnDManager.Commands
{
    /// <summary>
    /// Command to delete character from the character list and DB
    /// </summary>
    class DeleteCharacterCommand : CommandBase
    {
        private readonly CharacterBrowserViewModel _characterBrowserVM;
        private readonly DatabaseProvider _databaseProvider;

        public DeleteCharacterCommand(CharacterBrowserViewModel characterBrowserVM, DatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            _characterBrowserVM = characterBrowserVM;
            characterBrowserVM.PropertyChanged += OnViewModelPropertyChanged;
        }

        ~DeleteCharacterCommand() 
        {
            _characterBrowserVM.PropertyChanged -= OnViewModelPropertyChanged;
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
            try
            {
                //Try to delete the character with the given ID from the database.
                Task.Run(async () =>
                {
                    string sql = "DELETE FROM characters WHERE id = @id;";
                    await _databaseProvider.PostAsync(sql, new { id = _characterBrowserVM.SelectedCharacter!.ID });                   
                });
                _characterBrowserVM.RemoveSelectedCharacter(_characterBrowserVM.SelectedCharacter!.ID);
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex);
            }
            
        }

        public override bool CanExecute(object? parameter)
        {
            return _characterBrowserVM.SelectedCharacter is not null
                && base.CanExecute(parameter);
        }

    }
}