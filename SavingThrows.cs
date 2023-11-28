using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager
{
    [Flags]
    public enum SavingThrows
    {
        None = 0,
        Strength = 1,
        Dexterity = 2,
        Constitution = 4,
        Intelligence = 8,
        Wisdom = 16,
        Charisma = 32
    }
}
