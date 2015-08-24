using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class MagicalAttack : Targeted
    {
        public int magicAttack { get; set; }
        public int abilityStrength { get; set; }
    }
}
