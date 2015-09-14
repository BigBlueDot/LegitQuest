using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class CustomAbility : AbilityMessage
    {
        public CustomAbility()
        {
            this.hasComplexProcessing = true;
        }
    }
}
