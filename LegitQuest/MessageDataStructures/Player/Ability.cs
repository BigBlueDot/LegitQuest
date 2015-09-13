using MessageDataStructures.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Player
{
    public class Ability
    {
        public string name { get; set; }
        public int manaCost { get; set; }
        public bool enabled { get; set; }
        public int castTime { get; set; }
        public int cooldown { get; set; }
        public ManaAffinity affinity { get; set; }
    }
}
