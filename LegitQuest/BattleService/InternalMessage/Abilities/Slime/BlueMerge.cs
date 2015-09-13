using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities.Slime
{
    public class BlueMerge : AbilityMessage
    {
        public Guid source { get; set; }
        public Guid target { get; set; }
        public bool fullPotency { get; set; }
    }
}
