using BattleServiceLibrary.Actors.Statuses;
using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.Abilities.Warrior;
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
            if (this.abilities[commandIssued.commandNumber] == "Sword and Board")
            {
                PhysicalAttack physicalAttack = new PhysicalAttack();
                physicalAttack.abilityStrength = 5;
                physicalAttack.attack = this.strength;
                physicalAttack.target = commandIssued.target;
                physicalAttack.source = this.id;
                physicalAttack.accuracy = this.accuracy;
                physicalAttack.crit = this.critical;
                setCastTime(4000); //4s cast time
                physicalAttack.executeTime = this.castTimeComplete;
                physicalAttack.conversationId = commandIssued.conversationId;
                addOutgoingMessage(physicalAttack);

                SwordAndBoard swordAndBoard = new SwordAndBoard();
                swordAndBoard.conversationId = commandIssued.conversationId;
                swordAndBoard.executeTime = this.castTimeComplete;
                swordAndBoard.potency = 5;
                swordAndBoard.source = this.id;
                swordAndBoard.target = commandIssued.target;
                swordAndBoard.time = 8000;
                addOutgoingMessage(swordAndBoard);

                AbilityUsed abilityUsed = new AbilityUsed();
                abilityUsed.conversationId = commandIssued.conversationId;
                abilityUsed.message = this.name + " attacks while raising their shield!";
                addOutgoingMessage(abilityUsed);

                this.commandSent = false;
            }
            else if (this.abilities[commandIssued.commandNumber] == "Stagger")
            {
                AbilityUsed abilityUsed = new AbilityUsed();
                abilityUsed.conversationId = commandIssued.conversationId;
				abilityUsed.message = this.name + " knocks the enemy off balance!";
                addOutgoingMessage(abilityUsed);

                PhysicalAttack physicalAttack = new PhysicalAttack();
                physicalAttack.abilityStrength = 7;
                physicalAttack.attack = this.strength;
                physicalAttack.target = commandIssued.target;
                physicalAttack.source = this.id;
                physicalAttack.accuracy = this.accuracy;
                physicalAttack.crit = this.critical;
                setCastTime(4000); //4s cast time
                physicalAttack.executeTime = this.castTimeComplete;
                physicalAttack.conversationId = commandIssued.conversationId;
                addOutgoingMessage(physicalAttack);

                DefenseDecreased defenseDecreased = new DefenseDecreased();
                defenseDecreased.conversationId = commandIssued.conversationId;
                defenseDecreased.defenseReduction = 5;
                defenseDecreased.duration = 6000;
                defenseDecreased.executeTime = this.castTimeComplete;
                defenseDecreased.source = this.id;
                defenseDecreased.target = commandIssued.target;
                addOutgoingMessage(defenseDecreased);

                AddStatus addStatus = new AddStatus();
                addStatus.conversationId = commandIssued.conversationId;
                addStatus.executeTime = this.castTimeComplete;
                addStatus.status = new DefenseDecreasedStatus(this.castTimeComplete,defenseDecreased.duration,defenseDecreased.defenseReduction,defenseDecreased.target);
                this.addOutgoingMessage(addStatus);

                this.commandSent = false;
            }
            else if (abilities[commandIssued.commandNumber] == "Haymaker")
            {
                AbilityUsed abilityUsed = new AbilityUsed();
                abilityUsed.conversationId = commandIssued.conversationId;
                abilityUsed.message = this.name + " has struck a mighty blow!";
                addOutgoingMessage(abilityUsed);

                PhysicalAttack physicalAttack = new PhysicalAttack();
                physicalAttack.abilityStrength = 15;
                physicalAttack.attack = this.strength;
                physicalAttack.target = commandIssued.target;
                physicalAttack.source = this.id;
                physicalAttack.accuracy = this.accuracy;
                physicalAttack.crit = this.critical;
                setCastTime(4000); //4s cast time
                physicalAttack.executeTime = this.castTimeComplete;
                physicalAttack.conversationId = commandIssued.conversationId;
                addOutgoingMessage(physicalAttack);

                this.commandSent = false;
            }
            else
            {
                //For now we are just assuming it's an attack
                PhysicalAttack physicalAttack = new PhysicalAttack();
                physicalAttack.abilityStrength = 0;
                physicalAttack.attack = this.strength;
                physicalAttack.target = commandIssued.target;
                physicalAttack.source = this.id;
                physicalAttack.crit = this.critical;
                physicalAttack.accuracy = this.accuracy;
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
