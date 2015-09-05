using BattleServiceLibrary.InternalMessage.Abilities;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters.Classes
{
    public class Warrior : PlayerCharacter
    {
        public Warrior(string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical, List<string> abilities) :
			base(name, maxHP, strength, dexterity, vitality, magic, mind, resistance, accuracy, dodge, critical, abilities)
        {

        }

        protected override void useCommand(CommandIssued commandIssued)
        {
            //For now we are just assuming it's an attack
            PhysicalAttack physicalAttack = new PhysicalAttack();
            physicalAttack.abilityStrength = 15;
            physicalAttack.attack = this.strength;
            physicalAttack.target = commandIssued.target;
            physicalAttack.source = this.id;
            setCastTime(4000); //4s cast time
            physicalAttack.executeTime = this.castTimeComplete;
            physicalAttack.conversationId = commandIssued.conversationId;
            addOutgoingMessage(physicalAttack);

            AbilityUsed abilityUsed = new AbilityUsed();
            abilityUsed.conversationId = commandIssued.conversationId;
            abilityUsed.message = "An attack has been used!";
            addOutgoingMessage(abilityUsed);

            this.commandSent = false;
        }
    }
}
