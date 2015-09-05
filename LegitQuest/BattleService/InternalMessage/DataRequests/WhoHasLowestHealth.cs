using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataRequests
{
    public class WhoHasLowestHealth : InternalMessage
    {
        public Guid inquirer { get; set; }
        public bool isAlly { get; set; }
    }
}
