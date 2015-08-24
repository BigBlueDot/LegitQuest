using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters
{
    public abstract class NonPlayerCharacter : Character
    {
        protected abstract void doAction();

        public override void process(long time)
        {
            //Determine if they should do an attack
            if (castTimeComplete <= time)
            {
                doAction();
            }

            base.process(time);
        }

        protected override void processMessage(MessageDataStructures.Message message)
        {
            base.processMessage(message);
        }
    }
}
