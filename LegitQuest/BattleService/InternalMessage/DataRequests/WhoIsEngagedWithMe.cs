using BattleService.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.DataRequests
{
    public class WhoIsEngagedWithMe : InternalMessage
    {
        public Targeted source { get; set; }
    }
}
