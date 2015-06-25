using BattleService.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.DataRequests
{
    public class WhoHasLowestHealth : InternalMessage
    {
        public Targeted inquirer { get; set; }
        public bool isAlly { get; set; }
    }
}
