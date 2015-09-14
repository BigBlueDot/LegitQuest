using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class AttackDecreased : TargetedMessage
    {
        public int duration { get; set; }
        public int attackReduction { get; set; }
    }
}
