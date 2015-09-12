using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.Abilities.Slime;
using MessageDataStructures;
using MessageDataStructures.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters.Enemies
{
    public class BlueSlime : Slime
    {
        private InternalMessage.DataResults.Target lastTarget { get; set; }
        private bool merging { get; set; }

        public BlueSlime(int level, string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical) :
            base(level, name, maxHP, strength, dexterity, vitality, magic, mind, resistance, accuracy, dodge, critical)
        {

        }

        public override void removeMessagesAfterDefeat()
        {
            //Remove any messages that shouldn't be sent out after a defeat
            List<Message> toRemove = new List<Message>();
            foreach (Message message in this.outgoingMessages)
            {
                if (message is DamageDealt || (message is BlueMerge && merging))
                {

                }
                else
                {
                    toRemove.Add(message);
                }
            }

            foreach (Message message in toRemove)
            {
                this.outgoingMessages.Remove(message);
            }
        }

        protected override void processMessage(Message message)
        {
            if (message is BlueMergeResult)
            {
                BlueMergeResult blueMergeResult = (BlueMergeResult)message;
                if (blueMergeResult.should)
                {
                    //Cast BlueMerge on the appropriate target
                    BlueMerge blueMerge = new BlueMerge();
                    blueMerge.conversationId = blueMergeResult.conversationId;
                    blueMerge.fullPotency = (this.hp >= this.maxHp / 2);
                    blueMerge.source = this.id;
                    blueMerge.target = blueMergeResult.target;
                    this.addOutgoingMessage(blueMerge);

                    //Trigger ability message
                    AbilityUsed abilityUsed = new AbilityUsed();
                    abilityUsed.conversationId = blueMergeResult.conversationId;
                    abilityUsed.message = "The blue slime has merged with another";
                    this.addOutgoingMessage(abilityUsed);

                    //Defeat this Blue Slime
                    DamageDealt damageDealt = new DamageDealt();
                    damageDealt.conversationId = blueMergeResult.conversationId;
                    damageDealt.damage = this.hp;
                    damageDealt.source = this.id;
                    damageDealt.target = this.id;
                    this.addOutgoingMessage(damageDealt);

                    this.merging = true;
                    this.hp = 0;
                }
                else
                {
                    //Shouldn't use merge, use a regular attack instead
                    addNormalAttack(lastTarget);
                }

                return;
            }

            base.processMessage(message);
        }

        protected override void useRandomAttack(InternalMessage.DataResults.Target target, Random random)
        {
            //Use Blue Merge
            BlueMergeRequest blueMerge = new BlueMergeRequest();
            blueMerge.conversationId = target.conversationId;
            setCastTime(4000);
            blueMerge.executeTime = this.castTimeComplete;
            blueMerge.source = this.id;
            addOutgoingMessage(blueMerge);
            lastTarget = target;
        }

        private void addNormalAttack(InternalMessage.DataResults.Target target)
        {
            //Use normal attack
            PhysicalAttack physicalAttack = new PhysicalAttack();
            physicalAttack.abilityStrength = 2;
            physicalAttack.attack = this.strength;
            physicalAttack.target = target.target;
            physicalAttack.source = this.id;
            physicalAttack.accuracy = this.accuracy;
            physicalAttack.crit = this.critical;

            physicalAttack.executeTime = this.castTimeComplete;
            addOutgoingMessage(physicalAttack);

            AbilityUsed abilityUsed = new AbilityUsed();
            abilityUsed.conversationId = target.conversationId;
            abilityUsed.message = "The Blue Slime attacks!";
            addOutgoingMessage(abilityUsed);
        }
    }
}
