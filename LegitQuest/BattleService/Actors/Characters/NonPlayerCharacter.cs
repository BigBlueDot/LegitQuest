using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.Actors.Characters
{
    public abstract class NonPlayerCharacter : Character
    {
        protected abstract void doAction();

        public override void processDeltaTime(int time)
        {
            //Determine if they should do an attack
            if (castTimeComplete <= time)
            {
                doAction();
            }

            base.processDeltaTime(time);
        }

        protected override void processMessage(MessageDataStructures.Message message)
        {
            base.processMessage(message);
        }

        public override void processFinal()
        {
            base.processFinal();
        }
    }
}
