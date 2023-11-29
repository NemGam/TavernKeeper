using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Models
{
    class Character
    {
        private readonly int _id = -1000;

        private int _level;

        public int Id => _id;
        public string CharacterName { get; set; }
        public string CharacterClass { get; set; }
        public string Background { get; set; }
        public string Race { get; set; }
        public string EXP { get; set; }
        public string ArmorClass { get; set; }

        public Alignment ChosenAlignment { get; set; }
        public int Level
        {
            get => _level;
            set => _level = Math.Clamp(value, 1, 20);
        }

        private SavingThrows _proficientSavingThrows;
        private Skills _proficientSkills;

        public Skills ProficientSkills
        {
            get => _proficientSkills;
            set
            {
                _proficientSkills = value;
            }
        }

        public SavingThrows ProficientSavingThrows
        {
            get => _proficientSavingThrows;
            set => _proficientSavingThrows = value;
        }

        public enum Alignment
        {
            LawfulGood,
            NeutralGood,
            ChaoticGood,
            LawfulNeutral,
            Neutral,
            ChaoticNeutral,
            LawfulEvil,
            NeutralEvil,
            ChaoticEvil
        }

        public Ability Strength;
        public Ability Dexterity;
        public Ability Constitution;
        public Ability Intelligence;
        public Ability Wisdom;
        public Ability Charisma;

        
        public int ProficiencyBonus => (int)Math.Ceiling(Level / 4.0f) + 1;

        public int CalculateSavingThrowValue(Ability.Type type)
        {
            Ability ab = GetAbilityPropertyFromType(type);
            return _proficientSavingThrows.HasFlag((SavingThrows)ab.type)? ab.Modifier + ProficiencyBonus : ab.Modifier;
        }  

        public int CalculateSkillValue(Skills skill)
        {
            var mod = GetAbilityPropertyFromType(GetAbilityTypeFromSkill(skill)).Modifier;
            return _proficientSkills.HasFlag(skill) ? mod + ProficiencyBonus : mod;
        }

        /// <summary>
        /// Get related ability type based on the skill
        /// </summary>
        /// <param name="skill"></param>
        /// <returns>Returns related ability type</returns>
        private Ability.Type GetAbilityTypeFromSkill(Skills skill)
        {
            return skill switch
            {
                (Skills.Acrobatics) or (Skills.SleightOfHand) or (Skills.Stealth)
                => Ability.Type.Dexterity,

                (Skills.AnimalHandling) or (Skills.Insight) or (Skills.Medicine) or (Skills.Perception) or (Skills.Survival)
                => Ability.Type.Wisdom,

                (Skills.Arcana) or (Skills.History) or (Skills.Investigation) or (Skills.Nature) or (Skills.Religion)
                => Ability.Type.Intelligence,

                (Skills.Deception) or (Skills.Intimidation) or (Skills.Performance) or (Skills.Persuasion)
                => Ability.Type.Charisma,

                (Skills.Athletics) => Ability.Type.Strength,

                _ => throw new IndexOutOfRangeException()
            };
        }

        private Ability GetAbilityPropertyFromType(Ability.Type type)
        {
            return (type) switch
            {
                Ability.Type.Strength => Strength,
                Ability.Type.Dexterity => Dexterity,
                Ability.Type.Constitution => Constitution,
                Ability.Type.Intelligence => Intelligence,
                Ability.Type.Wisdom => Wisdom,
                Ability.Type.Charisma => Charisma,
                _ => throw new NotImplementedException(),
            };
        }

        public static int CountEnabledFlags<T>(T collectionToCount) 
            where T : IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("collectionToCount must be enum type");
            return new BitArray(new[] { (int)(object)collectionToCount }).OfType<bool>().Count(x => x);
        }

        public Character()
        {
            Strength = new Ability(Ability.Type.Strength);
            Dexterity = new Ability(Ability.Type.Dexterity);
            Constitution = new Ability(Ability.Type.Constitution);
            Intelligence = new Ability(Ability.Type.Intelligence);
            Wisdom = new Ability(Ability.Type.Wisdom);
            Charisma = new Ability(Ability.Type.Charisma);
            Level = 1;
        }

        public Character(int id, string characterName, string characterClass)
        {
            _id = id;
            CharacterName = characterName;
            CharacterClass = characterClass;
            Strength = new Ability(Ability.Type.Strength);
            Dexterity = new Ability(Ability.Type.Dexterity);
            Constitution = new Ability(Ability.Type.Constitution);
            Intelligence = new Ability(Ability.Type.Intelligence);
            Wisdom = new Ability(Ability.Type.Wisdom);
            Charisma = new Ability(Ability.Type.Charisma);
            Level = 1;
        }

        public override string ToString()
        {
            return $"Character name: {CharacterName}, Class: {CharacterClass}";
        }

    }
}
