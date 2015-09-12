using BattleServiceLibrary.InternalMessage.Abilities;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters.Enemies
{
    public class RedSlime : Slime
    {
        public RedSlime(int level, string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical) :
            base(level, name, maxHP, strength, dexterity, vitality, magic, mind, resistance, accuracy, dodge, critical)
        {

        }

        protected override void useRandomAttack(InternalMessage.DataResults.Target target, Random random)
        {
            //Use Blue Merge
            PhysicalAttack physicalAttack = new PhysicalAttack();
            physicalAttack.abilityStrength = 3;
            physicalAttack.attack = this.strength;
            physicalAttack.target = target.target;
            physicalAttack.source = this.id;
            physicalAttack.accuracy = this.accuracy;
            physicalAttack.crit = this.critical;

            setCastTime(4000);

            physicalAttack.executeTime = this.castTimeComplete;
            addOutgoingMessage(physicalAttack);

            AbilityUsed abilityUsed = new AbilityUsed();
            abilityUsed.conversationId = target.conversationId;
            abilityUsed.message = "The Red Slime attacks!";
            addOutgoingMessage(abilityUsed);
        }

    }
}
