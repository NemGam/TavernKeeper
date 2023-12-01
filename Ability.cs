using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager
{
    public class Ability
    {
        public enum Type
        {
            Strength = 1,
            Dexterity = 2,
            Constitution = 4,
            Intelligence = 8,
            Wisdom = 16,
            Charisma = 32
        }

        public Type type;

        public Ability(Type type)
        {
            this.type = type;
            this.Value = 0;
        }

        public Ability(Type type, int value)
        {
            this.type = type;
            this.Value = value;
        }

        const int MinLevel = 1;
        const int MaxLevel = 30;


        private int _value;
        public int Value 
        {
            get => _value;
            set
            {
                _value = Math.Clamp(value, MinLevel, MaxLevel);
            }
        }

        public int Modifier => CalculateModifier(_value);


        /// <summary>
        /// Returns modifier value of this ability.
        /// </summary>
        /// <param name="abilityScore"></param>
        /// <returns></returns>
        private static int CalculateModifier(int abilityScore)
        {
            return (int)Math.Floor(abilityScore / 2.0f) - 5;
        }
    }
}
