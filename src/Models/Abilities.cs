using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Models
{
    internal class Abilities
    {
        public Ability Strength;
        public Ability Dexterity;
        public Ability Constitution;
        public Ability Intelligence;
        public Ability Wisdom;
        public Ability Charisma;
        public Abilities() 
        {
            Strength = new Ability(Ability.Type.Strength);
            Dexterity = new Ability(Ability.Type.Dexterity);
            Constitution = new Ability(Ability.Type.Constitution);
            Intelligence = new Ability(Ability.Type.Intelligence);
            Wisdom = new Ability(Ability.Type.Wisdom);
            Charisma = new Ability(Ability.Type.Charisma);
        }

        public Abilities(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
        {
            Strength = new Ability(Ability.Type.Strength, strength);
            Dexterity = new Ability(Ability.Type.Dexterity, dexterity);
            Constitution = new Ability(Ability.Type.Constitution, constitution);
            Intelligence = new Ability(Ability.Type.Intelligence, intelligence);
            Wisdom = new Ability(Ability.Type.Wisdom, wisdom);
            Charisma = new Ability(Ability.Type.Charisma, charisma);
        }
    }
}
