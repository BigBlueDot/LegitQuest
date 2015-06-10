using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.Abilities
{
    public class DefenseIncreased : Targeted
    {
        public int duration { get; set; }
        public bool defenseBonus { get; set; }
    }
}
