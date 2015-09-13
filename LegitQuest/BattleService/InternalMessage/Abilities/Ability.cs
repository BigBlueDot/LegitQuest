using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class AbilityMessage : InternalMessage
    {
        public bool hasComplexProcessing { get; set; }
        public Func<AbilityMessage, List<Actors.Actor>, List<Guid>, List<Guid>, List<InternalMessage>> execute { get; set; }
    }
}
