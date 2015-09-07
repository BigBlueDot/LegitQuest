using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class AttackIncreased : InternalMessage
    {
        public Guid target { get; set; }
        public Guid source { get; set; }
        public int duration { get; set; }
        public int attackIncrease { get; set; }
    }
}
