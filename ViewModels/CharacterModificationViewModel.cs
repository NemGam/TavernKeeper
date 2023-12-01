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
        private readonly Models.Character? _character;
        private Character _initialCharacter;
        private readonly UserStore _userStore;

        private Abilities _abilities;

        private int _level;
        private SavingThrows _proficientSavingThrows;
        private Skills _proficientSkills;
        private int _currHP;
        private int _inspiration;
        private int _exp;
        private int _maxHP;
        private int _speed;
        private int _armorClass;

        public int Level
		{
			get => _level;
			set
			{
                _level = Math.Clamp(value, 1, 20);
				OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(ProficiencyBonus));
                NotifyAllSavingThrows();
                NotifyAllSkills();
            }
		}

        public Abilities Abilities
        {
            get => _abilities;
            set => _abilities = value;
        }
        public int Strength
        {
            get => _abilities.Strength.Value;
            set
            {
                _abilities.Strength.Value = value;
                OnPropertyChanged(nameof(Strength));
                OnPropertyChanged(nameof(StrengthMod));
                OnPropertyChanged(nameof(StrengthSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public int Dexterity
        {
            get => _abilities.Dexterity.Value;
            set
            {
                _abilities.Dexterity.Value = value;
                OnPropertyChanged(nameof(Dexterity));
                OnPropertyChanged(nameof(DexterityMod));
                OnPropertyChanged(nameof(DexteritySavingThrowValue));
                OnPropertyChanged(nameof(Initiative));
                NotifyAllSkills();
            }
        }

        public int Constitution
        {
            get => _abilities.Constitution.Value;
            set
            {
                _abilities.Constitution.Value = value;
                OnPropertyChanged(nameof(Constitution));
                OnPropertyChanged(nameof(ConstitutionMod));
                OnPropertyChanged(nameof(ConstitutionSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public int Intelligence
        {
            get => _abilities.Intelligence.Value;
            set
            {
                _abilities.Intelligence.Value = value;
                OnPropertyChanged(nameof(Intelligence));
                OnPropertyChanged(nameof(IntelligenceMod));
                OnPropertyChanged(nameof(IntelligenceSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public int Wisdom
        {
            get => _abilities.Wisdom.Value;
            set
            {
                _abilities.Wisdom.Value = value;
                OnPropertyChanged(nameof(Wisdom));
                OnPropertyChanged(nameof(WisdomMod));
                OnPropertyChanged(nameof(WisdomSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public int Charisma
        {
            get => _abilities.Charisma.Value;
            set
            {
                _abilities.Charisma.Value = value;
                OnPropertyChanged(nameof(Charisma));
                OnPropertyChanged(nameof(CharismaMod));
                OnPropertyChanged(nameof(CharismaSavingThrowValue));
                NotifyAllSkills();
            }
        }

        public SavingThrows ProficientSavingThrows
        {
            get => _proficientSavingThrows;
            set
            {
                //Players are allowed to have only 2 or less proficient throws
                if (CountEnabledFlags(value) > 2) return;
                _proficientSavingThrows = value;
                Debug.WriteLine(value);
                OnPropertyChanged(nameof(ProficientSavingThrows));
                NotifyAllSavingThrows();
            }
        }

        public Skills ProficientSkills
        {
            get => _proficientSkills;
            set
            {
                _proficientSkills = value;
                Debug.WriteLine(value);
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

            //Notifying all the skills about the change
            //TODO: notify only changed skill
            //Skipping first as it is None
            for (int i = 1; i < skills.Length; i++)
            {
                OnPropertyChanged(skills.GetValue(i)!.ToString()!);
            }
            OnPropertyChanged(nameof(PassiveWisdom));
        }

        public string CharacterName { get; set; }

        public string CharacterClass { get; set; }

        public string Background { get; set; }

        public string Race { get;set; }

        public Alignment ChosenAlignment { get;set; }

        public int EXP
        {
            get => _exp;
            set => _exp = Math.Max(0, value);
        }

        public int ArmorClass
        {
            get => _armorClass;
            set => _armorClass = Math.Max(0, value);
        }

        public int Speed
        {
            get => _speed;
            set => _speed = Math.Max(0, value);
        }

        public int MaxHP
        {
            get => _maxHP;
            set => _maxHP = Math.Max(0, value);
        }

        public int CurrHP
        {
            get => _currHP;
            set => _currHP = Math.Clamp(value, 0, MaxHP);
        }

        public int TempHP { get; set; }

        public int Inspiration
        {
            get => _inspiration;
            set => _inspiration = value;
        }

        //Attack and spells fields
        public string AtkSpl1 { get; set; }
        public string AtkSpl2 { get; set; }
        public string AtkSpl3 { get; set; }
        public string AtkSplOverall { get; set; }
        
        //DB final string
        public string AtkSplSummary => $"{AtkSpl1}~ {AtkSpl2}~ {AtkSpl3}~ {AtkSplOverall}~";

        //Equipment fields

        public string CP { get; set; }
        public string SP { get; set; }
        public string EP { get; set; }
        public string GP { get; set; }
        public string PP { get; set; }
        public string EquipmentBody { get; set; }

        //DB final string
        public string Equipment => $"{CP}~ {SP}~ {EP}~ {GP}~ {PP}~ {EquipmentBody}~";
        public string Flaws { get; set; }
        public string Ideals { get; set; }
        public string Bonds { get; set; }
        public string PersonalityTraits { get; set; }

        public string FeaturesTraits { get; set; }



        public string HitDice { get; set; }

        public string ProfAndLang { get; set; }

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


        #region Getters

        public string PlayerName => _userStore.CurrentUser.FirstName is not null? _userStore.CurrentUser.FirstName : "ERROR ERROR";
        public int ProficiencyBonus => Character.CalculateProficiencyBonus(Level);
        public int Initiative => DexterityMod;

        //Abilities modifiers
        public int StrengthMod => _abilities.Strength.Modifier;
        public int DexterityMod => _abilities.Dexterity.Modifier;
        public int ConstitutionMod => _abilities.Constitution.Modifier;
        public int IntelligenceMod => _abilities.Intelligence.Modifier;
        public int WisdomMod => _abilities.Wisdom.Modifier;
        public int CharismaMod => _abilities.Charisma.Modifier;

        //Saving Throws
        public int StrengthSavingThrowValue => Character.CalculateSavingThrowValue(Ability.Type.Strength, ProficientSavingThrows,
            _abilities, ProficiencyBonus);
        public int DexteritySavingThrowValue => Character.CalculateSavingThrowValue(Ability.Type.Dexterity, ProficientSavingThrows,
            _abilities, ProficiencyBonus);
        public int ConstitutionSavingThrowValue => Character.CalculateSavingThrowValue(Ability.Type.Constitution, ProficientSavingThrows,
            _abilities, ProficiencyBonus);
        public int IntelligenceSavingThrowValue => Character.CalculateSavingThrowValue(Ability.Type.Intelligence, ProficientSavingThrows,
            _abilities, ProficiencyBonus);
        public int WisdomSavingThrowValue => Character.CalculateSavingThrowValue(Ability.Type.Wisdom, ProficientSavingThrows,
            _abilities, ProficiencyBonus);
        public int CharismaSavingThrowValue => Character.CalculateSavingThrowValue(Ability.Type.Charisma, ProficientSavingThrows,
            _abilities, ProficiencyBonus);

        //Skills
        public int Acrobatics => Character.CalculateSkillValue(Skills.Acrobatics, ProficientSkills, _abilities, ProficiencyBonus);
        public int AnimalHandling => Character.CalculateSkillValue(Skills.AnimalHandling, ProficientSkills, _abilities, ProficiencyBonus);
        public int Arcana => Character.CalculateSkillValue(Skills.Arcana, ProficientSkills, _abilities, ProficiencyBonus);
        public int Athletics => Character.CalculateSkillValue(Skills.Athletics, ProficientSkills, _abilities, ProficiencyBonus);
        public int Deception => Character.CalculateSkillValue(Skills.Deception, ProficientSkills, _abilities, ProficiencyBonus);
        public int History => Character.CalculateSkillValue(Skills.History, ProficientSkills, _abilities, ProficiencyBonus);
        public int Insight => Character.CalculateSkillValue(Skills.Insight, ProficientSkills, _abilities, ProficiencyBonus);
        public int Intimidation => Character.CalculateSkillValue(Skills.Intimidation, ProficientSkills, _abilities, ProficiencyBonus);
        public int Investigation => Character.CalculateSkillValue(Skills.Investigation, ProficientSkills, _abilities, ProficiencyBonus);
        public int Medicine => Character.CalculateSkillValue(Skills.Medicine, ProficientSkills, _abilities, ProficiencyBonus);
        public int Nature => Character.CalculateSkillValue(Skills.Nature, ProficientSkills, _abilities, ProficiencyBonus);
        public int Perception => Character.CalculateSkillValue(Skills.Perception, ProficientSkills, _abilities, ProficiencyBonus);
        public int Performance => Character.CalculateSkillValue(Skills.Performance, ProficientSkills, _abilities, ProficiencyBonus);
        public int Persuasion => Character.CalculateSkillValue(Skills.Persuasion, ProficientSkills, _abilities, ProficiencyBonus);
        public int Religion => Character.CalculateSkillValue(Skills.Religion, ProficientSkills, _abilities, ProficiencyBonus);
        public int SleightOfHand => Character.CalculateSkillValue(Skills.SleightOfHand, ProficientSkills, _abilities, ProficiencyBonus);
        public int Stealth => Character.CalculateSkillValue(Skills.Stealth, ProficientSkills, _abilities, ProficiencyBonus);
        public int Survival => Character.CalculateSkillValue(Skills.Survival, ProficientSkills, _abilities, ProficiencyBonus);
        public int PassiveWisdom => 10 + Perception;

        #endregion

        public string GetCharacterInfo()
        {
            return _character.ToString();
        }

        public SubmitCharacterCommand SubmitCharacterCommand { get; }
        public NavigateCommand<MainPlayerViewModel> CancelCommand { get; }

        public CharacterModificationViewModel(UserStore userStore, Models.Character character, 
            DatabaseProvider databaseProvider, NavigationService<MainPlayerViewModel> MainPlayerViewModelNS)
        {
            _abilities = new Abilities();
            _userStore = userStore;
            _character = character;
            SubmitCharacterCommand = new SubmitCharacterCommand(_userStore, this, MainPlayerViewModelNS, databaseProvider, _character);
            CancelCommand = new NavigateCommand<MainPlayerViewModel>(MainPlayerViewModelNS);

            if (character is not null) LoadFromCharacter(character);
        }

        private void LoadFromCharacter(Character character)
        {
            
        }
    }
}
