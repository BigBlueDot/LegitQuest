using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.Abilities
{
    public class Taunt : InternalMessage
    {
        public Targeted target { get; set; }
        public Targeted switchTarget { get; set; }
        public int switchPosition { get; set; }
        public Status status { get; set; }

        public enum Status //Status moves through phases as different actors process it
        {
            Initialized, //It has been cast
            PassedTarget, //The target has failed to stop it
            PassedSwitchTarget, //The switch target has failed to stop it (Only applies if there is a switch target)
            Failed, //One of the targets resisted it
            Complete //It worked, carry on
        }
    }
}
