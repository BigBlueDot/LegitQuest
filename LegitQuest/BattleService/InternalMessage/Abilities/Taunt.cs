using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.Abilities
{
    public class Taunt : Targeted
    {
        public bool sourceIsAlly { get; set; }
        public int source { get; set; }
    }
}
