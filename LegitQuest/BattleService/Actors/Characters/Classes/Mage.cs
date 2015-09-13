using BattleServiceLibrary.Actors.Statuses;
using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.Abilities.Mage;
using BattleServiceLibrary.Utility;
using MessageDataStructures;
using MessageDataStructures.Battle;
using MessageDataStructures.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters.Classes
{
    public class Mage : PlayerCharacter
    {
        public Mage(int level, string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical, List<Ability> abilities) :
            base(level, name, maxHP, strength, dexterity, vitality, magic, mind, resistance, accuracy, dodge, critical, abilities)
        {

        }

        protected override void useCommand(CommandIssued commandIssued)
        {
            if (this.abilities[commandIssued.commandNumber].name == "Arcane Bullet")
            {
                int manaCost = this.abilities[commandIssued.commandNumber].manaCost;

                if (this.hasMana(manaCost))
                {
                    this.useMana(manaCost);

                    MagicalAttack magicalAttack = new MagicalAttack();
                    magicalAttack.abilityStrength = 8;
                    magicalAttack.magicAttack = this.magic;
                    magicalAttack.target = commandIssued.target;
                    magicalAttack.source = this.id;
                    magicalAttack.accuracy = this.accuracy;
                    magicalAttack.crit = this.critical;
                    setCastTime(4000); //4s cast time
                    magicalAttack.executeTime = this.castTimeComplete;
                    magicalAttack.conversationId = commandIssued.conversationId;
                    addOutgoingMessage(magicalAttack);

                    AbilityUsed abilityUsed = new AbilityUsed();
                    abilityUsed.conversationId = commandIssued.conversationId;
                    abilityUsed.message = this.name + " fires an arcane bullet!";
                    addOutgoingMessage(abilityUsed);

                    UseMana useMana = new UseMana();
                    useMana.conversationId = commandIssued.conversationId;
                    useMana.mana = manaCost;
                    addOutgoingMessage(useMana);

                    this.commandSent = false;
                }
            }
            else if (this.abilities[commandIssued.commandNumber].name == "Flurry")
            {
                int manaCost = this.abilities[commandIssued.commandNumber].manaCost;

                if (this.hasMana(manaCost))
                {
                    this.useMana(manaCost);

                    Flurry flurry = new Flurry();
                    flurry.conversationId = commandIssued.conversationId;
                    flurry.potency = 8;
                    flurry.magicAttack = this.magic;
                    flurry.source = this.id;
                    flurry.accuracy = this.accuracy;
                    flurry.crit = this.critical;
                    setCastTime(8000);
                    flurry.executeTime = this.castTimeComplete;
                    addOutgoingMessage(flurry);

                    AbilityUsed abilityUsed = new AbilityUsed();
                    abilityUsed.conversationId = commandIssued.conversationId;
                    abilityUsed.message = this.name + " unleashes a burst of magic energy!";
                    addOutgoingMessage(abilityUsed);

                    UseMana useMana = new UseMana();
                    useMana.conversationId = commandIssued.conversationId;
                    useMana.mana = manaCost;
                    addOutgoingMessage(useMana);

                    this.commandSent = false;
                }
            }
            else if (abilities[commandIssued.commandNumber].name == "Enfeeble")
            {
                int manaCost = this.abilities[commandIssued.commandNumber].manaCost;

                if (this.hasMana(manaCost))
                {
                    this.useMana(manaCost);

                    AttackDecreased attackDecreased = new AttackDecreased();
                    attackDecreased.conversationId = commandIssued.conversationId;
                    attackDecreased.attackReduction = 5;
                    attackDecreased.duration = 12000;
                    attackDecreased.executeTime = this.castTimeComplete;
                    attackDecreased.source = this.id;
                    attackDecreased.target = commandIssued.target;
                    addOutgoingMessage(attackDecreased);

                    AddStatus addStatus = new AddStatus();
                    addStatus.conversationId = commandIssued.conversationId;
                    setCastTime(4000);
                    addStatus.executeTime = this.castTimeComplete;
                    addStatus.status = new AttackDecreasedStatus(this.castTimeComplete, attackDecreased.duration, attackDecreased.attackReduction, attackDecreased.target);
                    this.addOutgoingMessage(addStatus);

                    AbilityUsed abilityUsed = new AbilityUsed();
                    abilityUsed.conversationId = commandIssued.conversationId;
                    abilityUsed.message = this.name + " has case a blanket of enfeeblement!";
                    addOutgoingMessage(abilityUsed);

                    UseMana useMana = new UseMana();
                    useMana.conversationId = commandIssued.conversationId;
                    useMana.mana = manaCost;
                    addOutgoingMessage(useMana);

                    this.commandSent = false;
                }
            }
            else
            {
                //For now we are just assuming it's an attack
                PhysicalAttack physicalAttack = new PhysicalAttack();
                physicalAttack.abilityStrength = 0;
                physicalAttack.attack = this.strength;
                physicalAttack.target = commandIssued.target;
                physicalAttack.source = this.id;
                physicalAttack.accuracy = this.accuracy;
                physicalAttack.crit = this.critical;
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