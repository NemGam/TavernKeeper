using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDManager
{
    [Flags]
    public enum Skills
    {
        None = 0,
        Acrobatics = 1,
        AnimalHandling = 2,
        Arcana = 4,
        Athletics = 8,
        Deception = 16,
        History = 32,
        Insight = 64,
        Intimidation = 128,
        Investigation = 256,
        Medicine = 512,
        Nature = 1024,
        Perception = 2048,
        Performance = 4096,
        Persuasion = 8192,
        Religion = 16384,
        SleightOfHand = 32768,
        Stealth = 65536,
        Survival = 131072
    }
}
