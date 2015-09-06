﻿using BattleServiceLibrary.InternalMessage.Abilities;
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
                physicalAttack.abilityStrength = 15;
                physicalAttack.attack = this.strength;
                physicalAttack.target = commandIssued.target;
                physicalAttack.source = this.id;
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
