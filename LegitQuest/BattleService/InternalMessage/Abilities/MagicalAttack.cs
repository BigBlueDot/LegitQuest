using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class MagicalAttack : TargetedMessage
    {
        public int magicAttack { get; set; }
        public int accuracy { get; set; }
        public int crit { get; set; }
        public int abilityStrength { get; set; }
    }
}
