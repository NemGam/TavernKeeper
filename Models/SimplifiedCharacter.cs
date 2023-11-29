using DnDManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager.Models
{
    /// <summary>
    /// Class for simplified view of all characters
    /// </summary>
    internal class SimplifiedCharacter : IDbReturnable
    {
        private int _id;
        
        public int ID => _id;
        public string Name { get; }
        public int Level { get; }
        public string Class { get; }
        public string Background { get; }
        public string Race { get; }
        public Character.Alignment Alignment { get; }


        public SimplifiedCharacter(int id, string name, int level, string @class, string background, string race, Character.Alignment alignment)
        {
            _id = id;
            Name = name;
            Level = level;
            Class = @class;
            Background = background;
            Race = race;
            Alignment = alignment;
        }
    }
}
