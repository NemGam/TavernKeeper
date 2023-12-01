using Dapper;
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
            Task.Run(async () =>
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

                        await _databaseProvider.PostAsync(sql, CreateParameters());
                        _MainPlayerViewModelNS.Navigate();
                    }
                    else
                    {
                        string sql = "UPDATE characters SET character_name = @character_name, character_class = @character_class, level = @level, " +
                        "background = @background, alignment = @alignment, strength_score = @strength_score, dexterity_score = @dexterity_score, " +
                        "constitution_score = @constitution_score, intelligence_score = @intelligence_score, wisdom_score = @wisdom_score, " +
                        "charisma_score = @charisma_score, saving_throws = @saving_throws, skills = @skills, race = @race, owner_username = @owner_username, " +
                        "speed = @speed, armor_class = @armor_class, max_hp = @max_hp, current_hp = @current_hp, temp_hp = @temp_hp, hit_dice = @hit_dice, " +
                        "inspiration = @inspiration, attacks_spells = @attacks_spells, equipment = @equipment, flaws = @flaws, ideals = @ideals, bonds = @bonds, " +
                        "personality_traits = @personality_traits, features_traits = @features_traits, proficiencies_languages = @proficiencies_languages, experience_points = @experience_points " +
                        "WHERE id = @id;";
                        var param = CreateParameters();
                        param.Add("id", _character.Id, System.Data.DbType.Int64);

                        await _databaseProvider.PostAsync(sql, param);
                        _MainPlayerViewModelNS.Navigate();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });

        }
        private DynamicParameters CreateParameters()
        {
            var parameters = new DynamicParameters();
            parameters.Add("character_name", _characterModificationViewModel.CharacterName, System.Data.DbType.String);
            parameters.Add("character_class", _characterModificationViewModel.CharacterClass, System.Data.DbType.String);
            parameters.Add("level", _characterModificationViewModel.Level, System.Data.DbType.Int16);
            parameters.Add("background", _characterModificationViewModel.Background, System.Data.DbType.String);
            parameters.Add("alignment", _characterModificationViewModel.ChosenAlignment, System.Data.DbType.Int32);
            parameters.Add("strength_score", _characterModificationViewModel.Abilities.Strength.Value, System.Data.DbType.Int32);
            parameters.Add("dexterity_score", _characterModificationViewModel.Abilities.Dexterity.Value, System.Data.DbType.Int32);
            parameters.Add("constitution_score", _characterModificationViewModel.Abilities.Constitution.Value, System.Data.DbType.Int32);
            parameters.Add("intelligence_score", _characterModificationViewModel.Abilities.Intelligence.Value, System.Data.DbType.Int32);
            parameters.Add("wisdom_score", _characterModificationViewModel.Abilities.Wisdom.Value, System.Data.DbType.Int32);
            parameters.Add("charisma_score", _characterModificationViewModel.Abilities.Charisma.Value, System.Data.DbType.Int32);
            parameters.Add("saving_throws", _characterModificationViewModel.ProficientSavingThrows, System.Data.DbType.Int32);
            parameters.Add("skills", _characterModificationViewModel.ProficientSkills, System.Data.DbType.Int32);
            parameters.Add("race", _characterModificationViewModel.Race, System.Data.DbType.String);
            parameters.Add("owner_username", _userStore.CurrentUser.UserName, System.Data.DbType.String);
            parameters.Add("speed", _characterModificationViewModel.Speed, System.Data.DbType.Int32);
            parameters.Add("armor_class", _characterModificationViewModel.ArmorClass, System.Data.DbType.Int32);
            parameters.Add("max_hp", _characterModificationViewModel.MaxHP, System.Data.DbType.Int32);
            parameters.Add("current_hp", _characterModificationViewModel.CurrHP, System.Data.DbType.Int32);
            parameters.Add("temp_hp", _characterModificationViewModel.TempHP, System.Data.DbType.Int32);
            parameters.Add("hit_dice", _characterModificationViewModel.HitDice, System.Data.DbType.String);
            parameters.Add("inspiration", _characterModificationViewModel.Inspiration, System.Data.DbType.Int32);
            parameters.Add("attacks_spells", _characterModificationViewModel.AtkSplSummary, System.Data.DbType.String);
            parameters.Add("equipment", _characterModificationViewModel.Equipment, System.Data.DbType.String);
            parameters.Add("flaws", _characterModificationViewModel.Flaws, System.Data.DbType.String);
            parameters.Add("ideals", _characterModificationViewModel.Ideals, System.Data.DbType.String);
            parameters.Add("bonds", _characterModificationViewModel.Bonds, System.Data.DbType.String);
            parameters.Add("personality_traits", _characterModificationViewModel.PersonalityTraits, System.Data.DbType.String);
            parameters.Add("features_traits", _characterModificationViewModel.FeaturesTraits, System.Data.DbType.String);
            parameters.Add("proficiencies_languages", _characterModificationViewModel.ProfAndLang, System.Data.DbType.String);
            parameters.Add("experience_points", _characterModificationViewModel.EXP, System.Data.DbType.Int32);
            return parameters;
        }
    }
}
