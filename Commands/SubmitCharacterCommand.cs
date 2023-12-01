using DnDManager.Models;
using DnDManager.Services;
using DnDManager.Stores;
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
        private readonly CharacterModificationViewModel _characterModificationViewModel;
        private readonly DatabaseProvider _databaseProvider;
        private readonly NavigationService<MainPlayerViewModel> _MainPlayerViewModelNS;
        private readonly Character _character;
        private readonly UserStore _userStore;

        public SubmitCharacterCommand(UserStore userStore, CharacterModificationViewModel characterModificationViewModel,
            NavigationService<MainPlayerViewModel> MainPlayerViewModelNS, DatabaseProvider databaseProvider, Character character) 
        {
            _userStore = userStore;
            _MainPlayerViewModelNS = MainPlayerViewModelNS;
            _characterModificationViewModel = characterModificationViewModel;
            _databaseProvider = databaseProvider;
            _character = character;
        }

        public override void Execute(object? parameter)
        {
            ExecuteAsync();
        }

        private async Task ExecuteAsync()
        {
            try
            {
                if (_character.Id == Character.DEFAULTID)
                {

                    string sql = "INSERT INTO characters (character_name, character_class, level, background, alignment, strength_score, dexterity_score, constitution_score, " +
                        "intelligence_score, wisdom_score, charisma_score, saving_throws, skills, race, owner_username, speed, armor_class, max_hp, current_hp, temp_hp, hit_dice, " +
                        "inspiration, attacks_spells, equipment, flaws, ideals, bonds, personality_traits, features_traits, proficiencies_languages, experience_points) " +
                        "VALUES (@character_name, @character_class, @level, @background, @alignment, @strength_score, @dexterity_score, @constitution_score, @intelligence_score, " +
                        "@wisdom_score, @charisma_score, @saving_throws, @skills, @race, @owner_username, @speed, @armor_class, @max_hp, @current_hp, @temp_hp, @hit_dice, @inspiration, " +
                        "@attacks_spells, @equipment, @flaws, @ideals, @bonds, @personality_traits, @features_traits, @proficiencies_languages, @experience_points);";
                    var parameters = new
                    {
                        character_name = _characterModificationViewModel.CharacterName,
                        character_class = _characterModificationViewModel.CharacterClass,
                        level = _characterModificationViewModel.Level,
                        background = _characterModificationViewModel.Background,
                        alignment = _characterModificationViewModel.ChosenAlignment,
                        strength_score = _characterModificationViewModel.Abilities.Strength.Value,
                        dexterity_score = _characterModificationViewModel.Abilities.Dexterity.Value,
                        constitution_score = _characterModificationViewModel.Abilities.Constitution.Value,
                        intelligence_score = _characterModificationViewModel.Abilities.Intelligence.Value,
                        wisdom_score = _characterModificationViewModel.Abilities.Wisdom.Value,
                        charisma_score = _characterModificationViewModel.Abilities.Charisma.Value,
                        saving_throws = _characterModificationViewModel.ProficientSavingThrows,
                        skills = _characterModificationViewModel.ProficientSkills,
                        race = _characterModificationViewModel.Race,
                        owner_username = _userStore.CurrentUser.UserName,
                        speed = _characterModificationViewModel.Speed,
                        armor_class = _characterModificationViewModel.ArmorClass,
                        max_hp = _characterModificationViewModel.MaxHP,
                        current_hp = _characterModificationViewModel.CurrHP,
                        temp_hp = _characterModificationViewModel.TempHP,
                        hit_dice = _characterModificationViewModel.HitDice,
                        inspiration = _characterModificationViewModel.Inspiration,
                        attacks_spells = _characterModificationViewModel.AtkSplSummary,
                        equipment = _characterModificationViewModel.Equipment,
                        flaws = _characterModificationViewModel.Flaws,
                        ideals = _characterModificationViewModel.Ideals,
                        bonds = _characterModificationViewModel.Bonds,
                        personality_traits = _characterModificationViewModel.PersonalityTraits,
                        features_traits = _characterModificationViewModel.FeaturesTraits,
                        proficiencies_languages = _characterModificationViewModel.ProfAndLang,
                        experience_points = _characterModificationViewModel.EXP
                    };
                    await _databaseProvider.PostAsync(sql, parameters);
                    _MainPlayerViewModelNS.Navigate();
                }
                else
                {
                    Debug.WriteLine(_characterModificationViewModel.GetCharacterInfo());
                    //EDIT
                    _MainPlayerViewModelNS.Navigate();
                    //_databaseProvider.Send();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
        }
    }
}
