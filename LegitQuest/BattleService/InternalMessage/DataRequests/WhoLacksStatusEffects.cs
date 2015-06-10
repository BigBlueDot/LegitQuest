using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.DataRequests
{
    public class WhoLacksStatusEffects
    {
        public bool isAlly { get; set; }
        public string statusEffect { get; set; } //might use a different identifier later on
    }
}
