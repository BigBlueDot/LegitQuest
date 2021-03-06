﻿using BattleServiceLibrary.InternalMessage;
using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Statuses
{
    public class DefenseDecreasedStatus : Actor
    {
        private long endTime { get; set; }
        private int defenseDecreaseMod { get; set; }
        private Guid target { get; set; }

        public DefenseDecreasedStatus(long currentTime, int duration, int defenseDecreaseMod, Guid target)
        {
            this.endTime = currentTime + duration;
            this.defenseDecreaseMod = defenseDecreaseMod;
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
                DefenseIncreased defenseDecreased = new DefenseIncreased();
                defenseDecreased.defenseBonus = defenseDecreaseMod;
                defenseDecreased.duration = -1;
                defenseDecreased.target = this.target;
                this.addOutgoingMessage(defenseDecreased);

                Defeated defeated = new Defeated();
                defeated.id = this.id;
                this.addOutgoingMessage(defeated);
            }
        }
    }
}
