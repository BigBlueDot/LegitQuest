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
    public class Slime : RandomNonPlayerCharacter
    {
        public Slime(int level, string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical) :
            base(level, name, maxHP, strength, dexterity, vitality, magic, mind, resistance, accuracy, dodge, critical)
        {

        }

        protected override void processMessage(MessageDataStructures.Message message)
        {
            if (message is BlueMerge)
            {
                BlueMerge blueMerge = (BlueMerge)message;
                if (blueMerge.fullPotency)
                {
                    MaxHPChange maxHPChange = new MaxHPChange();
                    maxHPChange.conversationId = message.conversationId;
                    maxHPChange.target = this.id;
                    maxHPChange.maxHPMod = this.maxHp;
                    addOutgoingMessage(maxHPChange);

                    this.maxHp = this.maxHp * 2;
                }

                HealingDone healingDone = new HealingDone();
                healingDone.conversationId = blueMerge.conversationId;
                healingDone.healValue = this.maxHp - this.hp;
                healingDone.source = blueMerge.source;
                healingDone.target = this.id;
                addOutgoingMessage(healingDone);

                this.hp = this.maxHp;
                return;
            }

            base.processMessage(message);
        }
    }
}
