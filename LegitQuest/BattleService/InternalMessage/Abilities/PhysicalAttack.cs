using BattleServiceLibrary.Actors.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class PhysicalAttack : TargetedMessage
    {
        public int attack { get; set; }
        public int accuracy { get; set; }
        public int crit { get; set; }
        public int abilityStrength { get; set; }

        public PhysicalAttack()
        {

        }

        public PhysicalAttack(Guid target, Character source, Guid conversationId, int abilityStrength)
        {
            this.conversationId = conversationId;
            this.target = target;
            this.abilityStrength = abilityStrength;
            this.attack = source.strength;
            this.source = source.id;
            this.accuracy = source.accuracy;
            this.crit = source.critical;
            this.executeTime = source.castTimeComplete;
        }
    }
}
