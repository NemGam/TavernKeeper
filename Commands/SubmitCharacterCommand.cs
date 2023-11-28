using DnDManager.Models;
using DnDManager.Services;
using DnDManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Commands
{
    class SubmitCharacterCommand : CommandBase
    {
        private CharacterModificationViewModel _characterModificationViewModel;
        private DatabaseProvider _databaseProvider;
        private readonly NavigationService<MainPlayerViewModel> _MainPlayerViewModelNS;

        public SubmitCharacterCommand(CharacterModificationViewModel characterModificationViewModel,
            NavigationService<MainPlayerViewModel> MainPlayerViewModelNS, DatabaseProvider databaseProvider) 
        {
            _MainPlayerViewModelNS = MainPlayerViewModelNS;
            _characterModificationViewModel = characterModificationViewModel;
            _databaseProvider = databaseProvider;
        }

        public override void Execute(object? parameter)
        {
            Debug.WriteLine(_characterModificationViewModel.GetCharacterInfo());
            //Check for ID and either edit or submit a new character
            _MainPlayerViewModelNS.Navigate();
            //_databaseProvider.Send();
        }
    }
}
