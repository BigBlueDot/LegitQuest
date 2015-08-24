using BattleServiceLibrary.InternalMessage.Abilities;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters
{
    public class PlayerCharacter : Character
    {
        private bool canUseCommand()
        {
            return (this.castTimeComplete <= this.currentTime + 1000); //Can cast 1 second ahead of time
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
                    setCastTime(4000); //4s cast time
                    physicalAttack.executeTime = this.castTimeComplete;
                    addOutgoingMessage(physicalAttack);
                }
            }

            base.processMessage(message);
        }

        private void setCastTime(long ms)
        {
            this.castTimeComplete += ms;
        }
    }
}
