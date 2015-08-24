﻿using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.DataRequests;
using BattleServiceLibrary.InternalMessage.DataResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters
{
    public class RandomNonPlayerCharacter : NonPlayerCharacter
    {
        private bool waitingForTarget { get; set; }

        protected override void doAction()
        {
            //Get the correct target
            WhoIsEngagedWithMe dataRequest = new WhoIsEngagedWithMe();
            waitingForTarget = true;
            dataRequest.source.id = this.id;
            addOutgoingMessage(dataRequest);
        }

        protected override void processMessage(MessageDataStructures.Message message)
        {
            if (waitingForTarget && message is Target)
            {
                //Now that it has the target, get the attack
                waitingForTarget = false;
                PhysicalAttack physicalAttack = new PhysicalAttack();
                physicalAttack.abilityStrength = 10;
                physicalAttack.attack = this.strength;
                physicalAttack.id = ((Target)message).id;

                setCastTime(4000); //4s cast time

                physicalAttack.executeTime = this.castTimeComplete;
                addOutgoingMessage(physicalAttack);

                return;
            }
            base.processMessage(message);
        }

        private void setCastTime(int castTimeMs)
        {
            this.castTimeComplete += castTimeMs; //Will wait castTimeMs before using another ability
        }


    }
}
