using BattleServiceLibrary.InternalMessage;
using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Statuses
{
    public class AttackDecreasedStatus : Actor
    {
        private long endTime { get; set; }
        private int attackDecreasedMod { get; set; }
        private Guid target { get; set; }

        public AttackDecreasedStatus(long currentTime, int duration, int attackDecreasedMod, Guid target)
        {
            this.endTime = currentTime + duration;
            this.attackDecreasedMod = attackDecreasedMod;
            this.target = target;
            this.id = Guid.NewGuid();
        }

        public override void removeMessagesAfterDefeat()
        {
            //Never need to remove anything
        }

        public override void process(long time)
        {
            if (time >= endTime)
            {
                AttackIncreased attackIncreased = new AttackIncreased();
                attackIncreased.attackIncrease = attackDecreasedMod;
                attackIncreased.duration = -1;
                attackIncreased.target = this.target;
                this.addOutgoingMessage(attackIncreased);

                Defeated defeated = new Defeated();
                defeated.id = this.id;
                this.addOutgoingMessage(defeated);
            }
        }
    }
}
