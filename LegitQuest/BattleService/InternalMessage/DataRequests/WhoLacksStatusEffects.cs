using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataRequests
{
    public class WhoLacksStatusEffects : InternalMessage
    {
        public bool isAlly { get; set; }
        public string statusEffect { get; set; } //might use a different identifier later on
    }
}
