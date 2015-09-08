using BattleServiceLibrary.InternalMessage;
using BattleServiceLibrary.InternalMessage.Abilities;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Statuses
{
    public class HealOverTimeStatus : Actor
    {
        private long nextTick { get; set; }
        private long tick { get; set; }
        private int tickCount { get; set; }
        private int currentTickCount { get; set; }
        private int heal { get; set; }
        private Guid target { get; set; }
        private Guid conversationId { get; set; }
        private Guid source { get; set; }

        public HealOverTimeStatus(long nextTick, long tick, int tickCount, int heal, Guid source, Guid target, Guid conversationId)
        {
            this.nextTick = nextTick;
            this.tick = tick;
            this.tickCount = tickCount;
            this.heal = heal;
            this.source = source;
            this.target = target;
            this.currentTickCount = 0;
            this.conversationId = conversationId;
        }

        public override void removeMessagesAfterDefeat()
        {
            //Never need to remove anything
        }

        public override void process(long time)
        {
            if (time >= nextTick)
            {
                Heal heal = new Heal();
                heal.conversationId = this.conversationId;
                heal.executeTime = nextTick;
                heal.healValue = this.heal;
                heal.source = this.source;
                heal.target = this.target;
                this.addOutgoingMessage(heal);

                AbilityUsed abilityUsed = new AbilityUsed();
                abilityUsed.conversationId = this.conversationId;
                abilityUsed.message = "Heal has caused a rejuvenation!";
                this.addOutgoingMessage(abilityUsed);

                this.nextTick = this.nextTick + this.tick;
                this.currentTickCount++;

                if (tickCount == currentTickCount)
                {
                    Defeated defeated = new Defeated();
                    defeated.id = this.id;
                    this.addOutgoingMessage(defeated);
                }
            }
        }
    }
}
