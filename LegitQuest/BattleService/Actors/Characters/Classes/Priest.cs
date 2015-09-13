using BattleServiceLibrary.Actors.Statuses;
using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.Abilities.Priest;
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
    public class Priest : PlayerCharacter
    {
        public Priest(int level, string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical, List<Ability> abilities) :
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

                if (this.abilities[commandIssued.commandNumber].name == "Heal")
                {
                    AddStatus addStatus = new AddStatus();
                    addStatus.conversationId = commandIssued.conversationId;
                    setCastTime(castTime);
                    addStatus.executeTime = this.castTimeComplete;
                    addStatus.status = new HealOverTimeStatus(this.castTimeComplete, 3000, 4, this.mind, this.id, commandIssued.target, commandIssued.conversationId);
                    this.addOutgoingMessage(addStatus);

                    AbilityUsed abilityUsed = new AbilityUsed();
                    abilityUsed.conversationId = commandIssued.conversationId;
                    abilityUsed.message = "A healing aura emanates from " + this.name + "!";
                    this.addOutgoingMessage(abilityUsed);

                    UseMana useMana = new UseMana();
                    useMana.conversationId = commandIssued.conversationId;
                    useMana.mana = manaCost;
                    addOutgoingMessage(useMana);

                    this.commandSent = false;
                }
                else if (this.abilities[commandIssued.commandNumber].name == "Prayer")
                {
                    setCastTime(castTime);
                    Prayer prayer = new Prayer();
                    prayer.conversationId = commandIssued.conversationId;
                    prayer.executeTime = this.castTimeComplete;
                    prayer.healValue = (int)Math.Floor(this.mind * 1.5);
                    prayer.source = this.id;
                    this.addOutgoingMessage(prayer);

                    AbilityUsed abilityUsed = new AbilityUsed();
                    abilityUsed.conversationId = commandIssued.conversationId;
                    abilityUsed.message = "A burst of healing energy is released by " + this.name + "!";
                    this.addOutgoingMessage(abilityUsed);

                    UseMana useMana = new UseMana();
                    useMana.conversationId = commandIssued.conversationId;
                    useMana.mana = manaCost;
                    addOutgoingMessage(useMana);

                    this.commandSent = false;
                }
                else if (this.abilities[commandIssued.commandNumber].name == "Smite")
                {
                    MagicalAttack magicalAttack = new MagicalAttack();
                    magicalAttack.abilityStrength = 8;
                    magicalAttack.magicAttack = this.magic;
                    magicalAttack.target = commandIssued.target;
                    magicalAttack.source = this.id;
                    magicalAttack.accuracy = this.accuracy;
                    magicalAttack.crit = this.critical;
                    setCastTime(castTime); //4s cast time
                    magicalAttack.executeTime = this.castTimeComplete;
                    magicalAttack.conversationId = commandIssued.conversationId;
                    addOutgoingMessage(magicalAttack);

                    AbilityUsed abilityUsed = new AbilityUsed();
                    abilityUsed.conversationId = commandIssued.conversationId;
                    abilityUsed.message = this.name + " calls upon holy fire!";
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
