using BattleServiceLibrary.InternalMessage.Abilities;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters.Classes
{
    public class Mage : PlayerCharacter
    {
        public Mage(string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical, List<string> abilities) :
            base(name, maxHP, strength, dexterity, vitality, magic, mind, resistance, accuracy, dodge, critical, abilities)
        {

        }

        protected override void useCommand(CommandIssued commandIssued)
        {
            if (this.abilities[commandIssued.commandNumber] == "Arcane Bullet")
            {
                MagicalAttack magicalAttack = new MagicalAttack();
                magicalAttack.abilityStrength = 8;
                magicalAttack.magicAttack = this.magic;
                magicalAttack.target = commandIssued.target;
                magicalAttack.source = this.id;
                setCastTime(4000); //4s cast time
                magicalAttack.executeTime = this.castTimeComplete;
                magicalAttack.conversationId = commandIssued.conversationId;
                addOutgoingMessage(magicalAttack);

                AbilityUsed abilityUsed = new AbilityUsed();
                abilityUsed.conversationId = commandIssued.conversationId;
                abilityUsed.message = this.name + " fires an arcane bullet!";
                addOutgoingMessage(abilityUsed);

                this.commandSent = false;
            }
            else
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
}