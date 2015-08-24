using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataResults
{
    public class TargetList : InternalMessage
    {
        public List<Abilities.Targeted> targets { get; set; }
    }
}
