using MessageDataStructures.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities.TriAttack
{
    public class TriAttackContribution : TargetedMessage
    {
        public List<TriAttackInfo> contributions { get; set; }
    }
}
