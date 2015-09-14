using BattleServiceLibrary.Actors.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class MagicalAttack : TargetedMessage
    {
        public int magicAttack { get; set; }
        public int accuracy { get; set; }
        public int crit { get; set; }
        public int abilityStrength { get; set; }

        public MagicalAttack()
        {

        }

        public MagicalAttack(Guid target, Character source, Guid conversationId, int abilityStrength)
        {
            this.conversationId = conversationId;
            this.target = target;
            this.abilityStrength = abilityStrength;
            this.magicAttack = source.magic;
            this.source = source.id;
            this.accuracy = source.accuracy;
            this.crit = source.critical;
            this.executeTime = source.castTimeComplete;
        }
    }
}
