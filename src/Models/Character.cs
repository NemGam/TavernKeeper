using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace DnDManager.Models
{

    class Character
    {
        public const long DEFAULTID = -10000;
        public long Id => _id;
        public string CharacterName { get; private set; }
        public string CharacterClass { get; private set; }
        public string Background { get; private set; }
        public string Race { get; private set; }
        public int ArmorClass { get; private set; }
        public int Inspiration { get; private set; }
        public int Speed
        {
            get { return _speed; }
            private set => _speed = Math.Max(0, value);
        }

        public string HitDice { get; private set; }
        public string ProfAndLang { get; private set; }

        public string Flaws { get; private set; }
        public string Ideals { get; private set; }
        public string Bonds { get; private set; }
        public string PersonalityTraits { get; private set; }
        public string FeaturesTraits { get; private set; }
        public string AtkSplSummary { get; private set; }
        public string Equipment { get; private set; }
        public int EXP
        {
            get { return _exp; }
            private set => _exp = Math.Max(0, value);
        }

        public int MaxHP
        {
            get { return _maxHP; }
            private set => _maxHP = Math.Max(0, value);
        }

        public int CurrHP
        {
            get { return _currHP; }
            private set => _currHP = Math.Clamp(value, 0, MaxHP);
        }

        public int TempHP
        {
            get { return _tempHP; }
            private set => _tempHP = Math.Max(0, value);
        }

        public Alignment ChosenAlignment { get; private set; }
        public int Level
        {
            get => _level;
            private set => _level = Math.Clamp(value, 1, 20);
        }

        private SavingThrows _proficientSavingThrows;
        private Skills _proficientSkills;

        public Skills ProficientSkills
        {
            get => _proficientSkills;
            private set
            {
                _proficientSkills = value;
            }
        }

        public SavingThrows ProficientSavingThrows
        {
            get => _proficientSavingThrows;
            private set => _proficientSavingThrows = value;
        }



        private readonly long _id = DEFAULTID; //Default value
        public Abilities abilities;
        private int _maxHP;
        private int _level = 1;
        private int _speed;
        private int _currHP;
        private int _tempHP;
        private int _exp;

        public static int CalculateProficiencyBonus(int level) => (int)Math.Ceiling(level / 4.0f) + 1;

        public static int CalculateSavingThrowValue(Ability.Type type, SavingThrows proficientSavingThrows, Abilities abilities, int proficiencyBonus)
        {
            Ability ab = GetAbilityPropertyFromType(type, abilities);
            return proficientSavingThrows.HasFlag((SavingThrows)ab.type)? ab.Modifier + proficiencyBonus : ab.Modifier;
        }  

        public static int CalculateSkillValue(Skills skill, Skills proficientSkills, Abilities abilities, int proficiencyBonus)
        {
            var mod = GetAbilityPropertyFromType(GetAbilityTypeFromSkill(skill), abilities).Modifier;
            return proficientSkills.HasFlag(skill) ? mod + proficiencyBonus : mod;
        }

        /// <summary>
        /// Get related ability type based on the skill
        /// </summary>
        /// <param name="skill"></param>
        /// <returns>Returns related ability type</returns>
        public static Ability.Type GetAbilityTypeFromSkill(Skills skill)
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

        public static Ability GetAbilityPropertyFromType(Ability.Type type, Abilities abilities)
        {
            return (type) switch
            {
                Ability.Type.Strength => abilities.Strength,
                Ability.Type.Dexterity => abilities.Dexterity,
                Ability.Type.Constitution => abilities.Constitution,
                Ability.Type.Intelligence => abilities.Intelligence,
                Ability.Type.Wisdom => abilities.Wisdom,
                Ability.Type.Charisma => abilities.Charisma,
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
        
        public Character(long id, string character_name, string character_class, System.Int16 level, string background, 
            int strength_score, int dexterity_score, int constitution_score, int intelligence_score, int wisdom_score, int charisma_score,
            int saving_throws, int skills, string race, string owner_username, int speed, int armor_class, int max_hp, int current_hp, int temp_hp,
            string hit_dice, int inspiration, string attacks_spells, string equipment, string flaws, string ideals, string bonds, string personality_traits,
            string features_traits, string proficiencies_languages, int experience_points, int alignment)
        {
            _id = id;
            this.CharacterName = character_name;
            this.CharacterClass = character_class;
            this.Level = level;
            this.Background = background;
            abilities = new Abilities(strength_score, dexterity_score, constitution_score, intelligence_score, wisdom_score, charisma_score);
            ProficientSavingThrows = (SavingThrows)saving_throws;
            ProficientSkills = (Skills)skills;
            this.Race = race;
            this.Speed = speed;
            this.ArmorClass = armor_class;
            this.MaxHP = max_hp;
            this.CurrHP = current_hp;
            this.TempHP = temp_hp;
            this.HitDice = hit_dice;
            this.Inspiration = inspiration;
            this.AtkSplSummary = attacks_spells;
            this.Equipment = equipment;
            this.Flaws = flaws;
            this.Ideals = ideals;
            this.Bonds = bonds;
            this.PersonalityTraits = personality_traits;
            this.FeaturesTraits = features_traits;
            this.ProfAndLang = proficiencies_languages;
            this.EXP = experience_points;
            this.ChosenAlignment = (Alignment)alignment;
        }

        public Character(long id)
        {
            Abilities abilities = new();
            _id = id;
            Level = 1;
        }

        public override string ToString()
        {
            return $"Character name: {CharacterName}, Class: {CharacterClass}";
        }
    }
}
