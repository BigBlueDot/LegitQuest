using BattleService.InternalMessage;
using BattleService.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.Actors.Statuses
{
    public class DefenseIncreased : Actor
    {
        private int endTime { get; set; }
        private int defenseMod { get; set; }
        private Guid target { get; set; }

        public DefenseIncreased(int currentTime, int duration, int defenseMod, Guid target)
        {
            this.endTime = currentTime + duration;
            this.defenseMod = defenseMod;
            this.target = target;
        }

        public override void process(int time)
        {
            if (time >= endTime)
            {
                DefenseDecreased defenseDecreased = new DefenseDecreased();
                defenseDecreased.defenseReduction = defenseMod;
                defenseDecreased.duration = -1;
                defenseDecreased.id = this.target;
                this.addOutgoingMessage(defenseDecreased);

                Defeated defeated = new Defeated();
                defeated.id = this.id;
                this.addOutgoingMessage(defeated);
            }
        }
    }
}
