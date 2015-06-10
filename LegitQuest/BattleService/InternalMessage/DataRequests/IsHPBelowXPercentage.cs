using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.DataRequests
{
    public class IsHPBelowXPercentage : InternalMessage
    {
        public bool isAlly { get; set; }
        public int percentage { get; set; }
    }
}
