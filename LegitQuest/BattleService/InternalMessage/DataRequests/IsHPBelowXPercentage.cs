using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataRequests
{
    public class IsHPBelowXPercentage : InternalMessage
    {
        public Guid inquirer { get; set; }
        public Guid answerer { get; set; }
        public int percentage { get; set; }
    }
}
