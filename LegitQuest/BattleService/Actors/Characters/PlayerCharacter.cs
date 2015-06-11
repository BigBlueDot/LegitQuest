using BattleService.InternalMessage.Abilities;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.Actors.Characters
{
    public class PlayerCharacter : Character
    {
        private bool canUseCommand()
        {
            return (this.castTimeComplete <= this.currentTime);
        }

        private bool AoEAttackAllowed(CommandIssued command)
        {
            return false; //In the future, will check the ability api or something
        }

        protected override void processMessage(Message message)
        {
            if (message is CommandIssued)
            {
                if (canUseCommand())
                {
                    CommandIssued specificMessage = (CommandIssued)message;
                    
                    //For now we are just assuming it's an attack
                    PhysicalAttack physicalAttack = new PhysicalAttack();
                    physicalAttack.abilityStrength = 15;
                    physicalAttack.attack = this.strength;
                    physicalAttack.id = specificMessage.target;
                    addOutgoingMessage(physicalAttack);
                }
            }

            base.processMessage(message);
        }
    }
}
