using BattleServiceLibrary.InternalMessage.Abilities;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters.Enemies
{
    public class BlueSlime : RandomNonPlayerCharacter
    {
        public BlueSlime(string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical) :
            base(name, maxHP, strength, dexterity, vitality, magic, mind, resistance, accuracy, dodge, critical)
        {

        }

        protected override void useRandomAttack(InternalMessage.DataResults.Target target, Random random)
        {
            //Use normal attack
            PhysicalAttack physicalAttack = new PhysicalAttack();
            physicalAttack.abilityStrength = 2;
            physicalAttack.attack = this.strength;
            physicalAttack.target = target.target;
            physicalAttack.source = this.id;

            setCastTime(4000); //4s cast time

            physicalAttack.executeTime = this.castTimeComplete;
            addOutgoingMessage(physicalAttack);

            AbilityUsed abilityUsed = new AbilityUsed();
            abilityUsed.conversationId = target.conversationId;
            abilityUsed.message = "The Blue Slime attacks!";
            addOutgoingMessage(abilityUsed);
        }

    }
}
