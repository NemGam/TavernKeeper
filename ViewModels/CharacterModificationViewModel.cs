using DnDManager.Commands;
using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static DnDManager.Models.Character;

namespace DnDManager.ViewModels
{
    class CharacterModificationViewModel : ViewModelBase
    {
        private readonly Models.Character _character;
        private readonly UserStore _userStore;
        public int Level
		{
			get => _character.Level;
			set
			{
                _character.Level = value;
				OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(ProficiencyBonus));
                NotifyAllSavingThrows();
                NotifyAllSkills();
            }
		}

        public int Strength
        {
            get => _character.Strength.Value;
            set
            {
                _character.Strength.Value = value;
                OnPropertyChanged(nameof(Strength));
                OnPropertyChanged(nameof(StrengthMod));
                OnPropertyChanged(nameof(StrengthSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public int Dexterity
        {
            get => _character.Dexterity.Value;
            set
            {
                _character.Dexterity.Value = value;
                OnPropertyChanged(nameof(Dexterity));
                OnPropertyChanged(nameof(DexterityMod));
                OnPropertyChanged(nameof(DexteritySavingThrowValue));
                OnPropertyChanged(nameof(Initiative));
                NotifyAllSkills();
            }
        }

        public int Constitution
        {
            get => _character.Constitution.Value;
            set
            {
                _character.Constitution.Value = value;
                OnPropertyChanged(nameof(Constitution));
                OnPropertyChanged(nameof(ConstitutionMod));
                OnPropertyChanged(nameof(ConstitutionSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public int Intelligence
        {
            get => _character.Intelligence.Value;
            set
            {
                _character.Intelligence.Value = value;
                OnPropertyChanged(nameof(Intelligence));
                OnPropertyChanged(nameof(IntelligenceMod));
                OnPropertyChanged(nameof(IntelligenceSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public int Wisdom
        {
            get => _character.Wisdom.Value;
            set
            {
                _character.Wisdom.Value = value;
                OnPropertyChanged(nameof(Wisdom));
                OnPropertyChanged(nameof(WisdomMod));
                OnPropertyChanged(nameof(WisdomSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public int Charisma
        {
            get => _character.Charisma.Value;
            set
            {
                _character.Charisma.Value = value;
                OnPropertyChanged(nameof(Charisma));
                OnPropertyChanged(nameof(CharismaMod));
                OnPropertyChanged(nameof(CharismaSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public SavingThrows ProficientSavingThrows
        {
            get => _character.ProficientSavingThrows;
            set
            {
                //Players allowed to have only 2 or less proficient throws
                if (CountEnabledFlags<SavingThrows>(value) > 2) return;
                _character.ProficientSavingThrows = value;
                OnPropertyChanged(nameof(ProficientSavingThrows));
                NotifyAllSavingThrows();
            }
        }

        public Skills ProficientSkills
        {
            get => _character.ProficientSkills;
            set
            {
                _character.ProficientSkills = value;
                OnPropertyChanged(nameof(ProficientSkills));
                NotifyAllSkills();
            }
        }

        private void NotifyAllSavingThrows()
        {
            OnPropertyChanged(nameof(StrengthSavingThrowValue));
            OnPropertyChanged(nameof(DexteritySavingThrowValue));
            OnPropertyChanged(nameof(ConstitutionSavingThrowValue));
            OnPropertyChanged(nameof(IntelligenceSavingThrowValue));
            OnPropertyChanged(nameof(WisdomSavingThrowValue));
            OnPropertyChanged(nameof(CharismaSavingThrowValue));
        }

        private void NotifyAllSkills()
        {
            var skills = Enum.GetValues(typeof(Skills));
            if (skills is null) return;

            //Notifiying all the skills about the change
            //TODO: notify only changed skill
            //Skipping first as it is None
            for (int i = 1; i < skills.Length; i++)
            {
                OnPropertyChanged(skills.GetValue(i)!.ToString()!);
            }
            OnPropertyChanged(nameof(PassiveWisdom));
        }

        public string CharacterName
        {
            get => _character.CharacterName;
            set => _character.CharacterName = value;
        }

        public string CharacterClass
        {
            get => _character.CharacterClass;
            set => _character.CharacterClass = value;
        }

        public string Background
        {
            get => _character.Background;
            set => _character.Background = value;
        }

        public string Race
        {
            get => _character.Race;
            set => _character.Race = value;
        }

        public Alignment ChosenAlignment
        {
            get => _character.ChosenAlignment;
            set => _character.ChosenAlignment = value;
        }

        public string EXP
        {
            get => _character.EXP;
            set => _character.EXP = value;
        }

        public string ArmorClass
        {
            get => _character.ArmorClass;
            set => _character.ArmorClass = value;
        }

        public Alignment[] PossibleAlignments => new Alignment[] {
            Alignment.LawfulGood,
            Alignment.NeutralGood,
            Alignment.ChaoticGood,
            Alignment.LawfulNeutral,
            Alignment.Neutral,
            Alignment.ChaoticNeutral,
            Alignment.LawfulEvil,
            Alignment.NeutralEvil,
            Alignment.ChaoticEvil
        };

        public string PlayerName => _userStore.CurrentUser.FirstName is not null? _userStore.CurrentUser.FirstName : "ERROR ERROR";

        #region NumbersProperties

        public int ProficiencyBonus => _character.ProficiencyBonus;
        public int Initiative => DexterityMod;

        //Abilities modifiers
        public int StrengthMod => _character.Strength.Modifier;
        public int DexterityMod => _character.Dexterity.Modifier;
        public int ConstitutionMod => _character.Constitution.Modifier;
        public int IntelligenceMod => _character.Intelligence.Modifier;
        public int WisdomMod => _character.Wisdom.Modifier;
        public int CharismaMod => _character.Charisma.Modifier;

        //Saving Throws
        public int StrengthSavingThrowValue => _character.CalculateSavingThrowValue(Ability.Type.Strength);
        public int DexteritySavingThrowValue => _character.CalculateSavingThrowValue(Ability.Type.Dexterity);
        public int ConstitutionSavingThrowValue => _character.CalculateSavingThrowValue(Ability.Type.Constitution);
        public int IntelligenceSavingThrowValue => _character.CalculateSavingThrowValue(Ability.Type.Intelligence);
        public int WisdomSavingThrowValue => _character.CalculateSavingThrowValue(Ability.Type.Wisdom);
        public int CharismaSavingThrowValue => _character.CalculateSavingThrowValue(Ability.Type.Charisma);

        //Skills
        public int Acrobatics => _character.CalculateSkillValue(Skills.Acrobatics);
        public int AnimalHandling => _character.CalculateSkillValue(Skills.AnimalHandling);
        public int Arcana => _character.CalculateSkillValue(Skills.Arcana);
        public int Athletics => _character.CalculateSkillValue(Skills.Athletics);
        public int Deception => _character.CalculateSkillValue(Skills.Deception);
        public int History => _character.CalculateSkillValue(Skills.History);
        public int Insight => _character.CalculateSkillValue(Skills.Insight);
        public int Intimidation => _character.CalculateSkillValue(Skills.Intimidation);
        public int Investigation => _character.CalculateSkillValue(Skills.Investigation);
        public int Medicine => _character.CalculateSkillValue(Skills.Medicine);
        public int Nature => _character.CalculateSkillValue(Skills.Nature);
        public int Perception => _character.CalculateSkillValue(Skills.Perception);
        public int Performance => _character.CalculateSkillValue(Skills.Performance);
        public int Persuasion => _character.CalculateSkillValue(Skills.Persuasion);
        public int Religion => _character.CalculateSkillValue(Skills.Religion);
        public int SleightOfHand => _character.CalculateSkillValue(Skills.SleightOfHand);
        public int Stealth => _character.CalculateSkillValue(Skills.Stealth);
        public int Survival => _character.CalculateSkillValue(Skills.Survival);
        public int PassiveWisdom => 10 + Perception;

        #endregion

        public string GetCharacterInfo()
        {
            return _character.ToString();
        }

        public SubmitCharacterCommand SubmitCharacterCommand { get; }

        public CharacterModificationViewModel(UserStore userStore, Models.Character character, 
            DatabaseProvider databaseProvider, NavigationService<MainPlayerViewModel> MainPlayerViewModelNS)
        {
            _userStore = userStore;
            _character = character;
            SubmitCharacterCommand = new SubmitCharacterCommand(this, MainPlayerViewModelNS, databaseProvider);
        }

    }
}
