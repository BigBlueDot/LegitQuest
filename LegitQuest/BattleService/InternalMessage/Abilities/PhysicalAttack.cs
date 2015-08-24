using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class PhysicalAttack : Targeted
    {
        public int attack { get; set; }
        public int abilityStrength { get; set; }
    }
}
