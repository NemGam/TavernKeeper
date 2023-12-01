using DnDManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private long _id;
        
        public long ID => _id;
        public string Name { get; }
        public int Level { get; }
        public string Class { get; }
        public string Background { get; }
        public string Race { get; }
        public Alignment Alignment { get; private set; }

        /// <summary>
        /// The constructor that matches the Table in the DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="owner_username"></param>
        /// <param name="character_name"></param>
        /// <param name="level"></param>
        /// <param name="character_class"></param>
        /// <param name="background"></param>
        /// <param name="race"></param>
        /// <param name="alignment"></param>
        /// 

        public SimplifiedCharacter(long id, string character_name, string owner_username, Int16 level, string character_class,
            string background, string race, int alignment)
        {
            _id = id;
            Name = character_name;
            Level = level;
            Class = character_class;
            Background = background;
            Race = race;
            Alignment = (Alignment)alignment;
        }
    }
}
