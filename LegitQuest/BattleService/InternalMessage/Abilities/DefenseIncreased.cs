using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class DefenseIncreased : TargetedMessage
    {
        public int duration { get; set; }
        public int defenseBonus { get; set; }
    }
}
