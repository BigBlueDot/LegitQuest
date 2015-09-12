using BattleServiceLibrary.Actors.Statuses;
using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.Abilities.Goblin;
using BattleServiceLibrary.InternalMessage.DataResults;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters.Enemies
{
    public class Goblin : RandomNonPlayerCharacter
    {
        public Goblin(string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical) :
            base(name, maxHP, strength, dexterity, vitality, magic, mind, resistance, accuracy, dodge, critical)
        {
        }

        protected override void useRandomAttack(InternalMessage.DataResults.Target target, Random random)
        {
            double rnd = random.NextDouble();

            //Now that it has the target, get the attack
            if (rnd <= .75)
            {
                //Use normal attack
                PhysicalAttack physicalAttack = new PhysicalAttack();
                physicalAttack.abilityStrength = 2;
                physicalAttack.attack = this.strength;
                physicalAttack.target = target.target;
                physicalAttack.source = this.id;
                physicalAttack.accuracy = this.accuracy;
                physicalAttack.crit = this.critical;

                setCastTime(4000); //4s cast time

                physicalAttack.executeTime = this.castTimeComplete;
                addOutgoingMessage(physicalAttack);

                AbilityUsed abilityUsed = new AbilityUsed();
                abilityUsed.conversationId = target.conversationId;
                abilityUsed.message = "The Goblin attacks!";
                addOutgoingMessage(abilityUsed);
            }
            else
            {
                //Use Goblin Punch
                GoblinPunch goblinPunch = new GoblinPunch();
                setCastTime(4000); //4s cast time
                goblinPunch.executeTime = this.castTimeComplete;
                goblinPunch.physicalAttack = this.strength;
                goblinPunch.potency = 4;
                goblinPunch.source = this.id;
                goblinPunch.target = target.target;
                goblinPunch.conversationId = target.conversationId;
                goblinPunch.accuracy = this.accuracy;
                goblinPunch.crit = this.critical;
                addOutgoingMessage(goblinPunch);

                DefenseDecreased defenseDecreased = new DefenseDecreased();
                defenseDecreased.conversationId = target.conversationId;
                defenseDecreased.defenseReduction = 2;
                defenseDecreased.duration = 9000;
                defenseDecreased.executeTime = this.castTimeComplete;
                defenseDecreased.source = this.id;
                defenseDecreased.target = this.id;
                addOutgoingMessage(defenseDecreased);

                AddStatus addStatus = new AddStatus();
                addStatus.conversationId = target.conversationId;
                addStatus.executeTime = this.castTimeComplete;
                addStatus.status = new DefenseDecreasedStatus(this.castTimeComplete, 9000, defenseDecreased.defenseReduction, defenseDecreased.target);

                AbilityUsed abilityUsed = new AbilityUsed();
                abilityUsed.conversationId = target.conversationId;
                abilityUsed.message = "The Goblin attacks with a mighty Goblin Punch!";
                addOutgoingMessage(abilityUsed);

            }
        }
    }
}
