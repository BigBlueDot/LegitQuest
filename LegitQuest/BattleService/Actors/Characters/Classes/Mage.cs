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
            int manaCost = this.abilities[commandIssued.commandNumber].manaCost;
            int castTime = this.abilities[commandIssued.commandNumber].castTime;
            int cooldown = this.abilities[commandIssued.commandNumber].cooldown;

            if (this.hasMana(manaCost))
            {
                this.useMana(manaCost);
                this.setCooldown(cooldown, commandIssued.commandNumber);

                if (this.abilities[commandIssued.commandNumber].name == "Arcane Bullet")
                {
                    MagicalAttack magicalAttack = new MagicalAttack();
                    magicalAttack.abilityStrength = 8;
                    magicalAttack.magicAttack = this.magic;
                    magicalAttack.target = commandIssued.target;
                    magicalAttack.source = this.id;
                    magicalAttack.accuracy = this.accuracy;
                    magicalAttack.crit = this.critical;
                    setCastTime(castTime);
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
                else if (this.abilities[commandIssued.commandNumber].name == "Flurry")
                {
                    Flurry flurry = new Flurry();
                    flurry.conversationId = commandIssued.conversationId;
                    flurry.potency = 8;
                    flurry.magicAttack = this.magic;
                    flurry.source = this.id;
                    flurry.accuracy = this.accuracy;
                    flurry.crit = this.critical;
                    setCastTime(castTime);
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
                else if (abilities[commandIssued.commandNumber].name == "Enfeeble")
                {
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
                    setCastTime(castTime);
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
        }
    }
}