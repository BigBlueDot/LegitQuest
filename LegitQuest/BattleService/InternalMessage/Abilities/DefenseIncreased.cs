using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class DefenseIncreased : InternalMessage
    {
        public Guid target { get; set; }
        public Guid source { get; set; }
        public int duration { get; set; }
        public int defenseBonus { get; set; }
    }
}
