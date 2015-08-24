using BattleServiceLibrary.InternalMessage;
using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Statuses
{
    public class DamageOverTime : Actor
    {
        private Guid target { get; set; }
        private int lastTickTime { get; set; }
        private int endTime { get; set; }
        private int tickTime { get; set; }
        private int damage { get; set; }

        public DamageOverTime(Guid target, int timeStarted, int tickTime, int duration, int damage)
        {
            this.target = target;
            this.lastTickTime = 0;
            this.tickTime = tickTime;
            this.lastTickTime = timeStarted;
            this.endTime = timeStarted + duration;
            this.damage = damage;
        }

        public override void process(long time)
        {
            while (lastTickTime + tickTime <= time && lastTickTime + tickTime <= endTime)
            {
                lastTickTime += tickTime;

                DealStaticDamage dealStaticDamage = new DealStaticDamage();
                dealStaticDamage.damage = this.damage;
                dealStaticDamage.id = this.target;
                addOutgoingMessage(dealStaticDamage);
            }

            if (lastTickTime >= endTime) //This status is done
            {
                Defeated defeated = new Defeated();
                defeated.id = this.id;
                addOutgoingMessage(defeated);
            }
        }
    }
}
