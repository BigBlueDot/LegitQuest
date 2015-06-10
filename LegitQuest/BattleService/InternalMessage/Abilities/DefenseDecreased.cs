using BattleService.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.Abilities
{
    public class DefenseDecreased : Targeted
    {
        public int duration { get; set; }
        public bool defenseReduction { get; set; }
    }
}
