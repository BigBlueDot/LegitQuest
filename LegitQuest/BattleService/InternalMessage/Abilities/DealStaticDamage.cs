using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class DealStaticDamage : InternalMessage
    {
        public Guid target { get; set; }
        public Guid source { get; set; }
        public int damage { get; set; }
    }
}
