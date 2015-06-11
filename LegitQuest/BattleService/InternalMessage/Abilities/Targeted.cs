using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.Abilities
{
    public abstract class Targeted : InternalMessage
    {
        public Guid id { get; set; }
    }
}
