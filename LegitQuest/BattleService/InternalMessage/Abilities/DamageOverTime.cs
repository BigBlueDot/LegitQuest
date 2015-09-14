using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class DamageOverTime : TargetedMessage
    {
        public int damage { get; set; }
        public int durationMS { get; set; }
        public int tickDurationMS { get; set; }
        public int timeStarted { get; set; }
    }
}
