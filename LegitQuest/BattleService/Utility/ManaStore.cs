using MessageDataStructures.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Utility
{
    public class ManaStore
    {
        public int mana { get; set; }
        public ManaAffinity affinity { get; set; }
        public int affinityMana { get; set; }
    }
}
