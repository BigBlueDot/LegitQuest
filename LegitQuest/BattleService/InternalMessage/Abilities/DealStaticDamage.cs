using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class DealStaticDamage : Targeted
    {
        public int damage { get; set; }
    }
}
