using DnDManager.Models;
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

        public CharacterBrowserViewModel()
        {
            SimplifiedCharacterList = new ObservableCollection<SimplifiedCharacter>();
            SimplifiedCharacterList.Add(new SimplifiedCharacter(23, "Mor", 20, "Wigle", "Beach", "Genie", Character.Alignment.LawfulGood));
            SimplifiedCharacterList.Add(new SimplifiedCharacter(26, "FGF", 5, "NotWigle", "Forest Child", "Genie", Character.Alignment.LawfulGood));
            SimplifiedCharacterList.Add(new SimplifiedCharacter(21, "Tester", 11, "Tester", "Tester", "Tester", Character.Alignment.LawfulGood));
            SimplifiedCharacterList.Add(new SimplifiedCharacter(28, "SS", 1, "Wigle", "Beach", "Dwarf", Character.Alignment.LawfulGood));
        }
        public CharacterBrowserViewModel(DatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            SimplifiedCharacterList = new ObservableCollection<SimplifiedCharacter>();
        }
    }
}
