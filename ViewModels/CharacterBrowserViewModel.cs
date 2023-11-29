using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.ViewModels
{
    internal class CharacterBrowserViewModel : ViewModelBase
    {
        private DatabaseProvider _databaseProvider;

        public ObservableCollection<SimplifiedCharacter> SimplifiedCharacterList { get; }

        private SimplifiedCharacter? _selectedCharacter;
        public SimplifiedCharacter? SelectedCharacter 
        {
            get => _selectedCharacter;
            set
            {
                _selectedCharacter = value;
                OnPropertyChanged(nameof(SelectedCharacter));
            }
        }

        public void RemoveSelectedCharacter(int id)
        {
            if (_selectedCharacter is null) return;
            SimplifiedCharacterList.Remove(_selectedCharacter);
            //_databaseProvider.DeleteCharacter(id);
            SelectedCharacter = null;
        }

        public NavigateCommand<MainPlayerViewModel> GoBackCommand { get; }
        public EditCharacterCommand EditCharacterCommand { get; }
        public DeleteCharacterCommand DeleteCharacterCommand { get; }
        public CharacterBrowserViewModel(DatabaseProvider databaseProvider, NavigationService<MainPlayerViewModel> mainPlayerViewModelNS)
        {
            _databaseProvider = databaseProvider;
            SimplifiedCharacterList = new ObservableCollection<SimplifiedCharacter>();
            GoBackCommand = new NavigateCommand<MainPlayerViewModel>(mainPlayerViewModelNS);
            EditCharacterCommand = new EditCharacterCommand(this, databaseProvider);
            DeleteCharacterCommand = new DeleteCharacterCommand(this);
            SimplifiedCharacterList = new ObservableCollection<SimplifiedCharacter>();
            //Get characters from simplified view
            SimplifiedCharacterList.Add(new SimplifiedCharacter(23, "Mor", 20, "Wigle", "Beach", "Genie", Character.Alignment.LawfulGood));
            SimplifiedCharacterList.Add(new SimplifiedCharacter(26, "FGF", 5, "NotWigle", "Forest Child", "Genie", Character.Alignment.LawfulGood));
            SimplifiedCharacterList.Add(new SimplifiedCharacter(21, "Tester", 11, "Tester", "Tester", "Tester", Character.Alignment.LawfulGood));
            SimplifiedCharacterList.Add(new SimplifiedCharacter(28, "SS", 1, "Wigle", "Beach", "Dwarf", Character.Alignment.LawfulGood));
        }
    }
}
