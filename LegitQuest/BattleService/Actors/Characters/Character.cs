using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleService.InternalMessage.Abilities;
using BattleService.InternalMessage.DataRequests;
using BattleService.InternalMessage.DataResults;
using MessageDataStructures;
using BattleService.InternalMessage;

namespace BattleService.Actors.Characters
{
    public abstract class Character : Actor
    {
        public int hp { get; set; }
        public int maxHp { get; set; }
        public int strength { get; set; }
        public int defense { get; set; }
        public int magic { get; set; }
        public int resistance { get; set; }
        public int spirit { get; set; }

        public int currentTime { get; set; }
        public int castTimeComplete { get; set; }

        protected bool isDefeated()
        {
            return (hp <= 0);
        }

        protected virtual void processMessage(Message message)
        {
            if (message is PhysicalAttack)
            {
                PhysicalAttack specificMessage = (PhysicalAttack)message;

                int dmg = Convert.ToInt32(Math.Floor((Math.Pow((double)specificMessage.attack, 1.65f)) / ((double)this.defense) * specificMessage.abilityStrength));
                hp -= dmg;

                DamageDealt damageDealt = new DamageDealt();
                damageDealt.damage = dmg;
                damageDealt.target = this.id;
                addOutgoingMessage(damageDealt);

                messages.Dequeue();
            }
            else if (message is MagicalAttack)
            {
                MagicalAttack specificMessage = (MagicalAttack)message;

                int dmg = Convert.ToInt32(Math.Floor((Math.Pow((double)specificMessage.magicAttack, 1.65f)) / ((double)this.resistance) * specificMessage.abilityStrength));

                this.hp -= dmg;

                DamageDealt damageDealt = new DamageDealt();
                damageDealt.damage = dmg;
                damageDealt.target = this.id;
                addOutgoingMessage(damageDealt);

                messages.Dequeue();
            }
            else if (message is Heal)
            {
                Heal specificMessage = (Heal)message;

                this.hp += specificMessage.healValue;

                HealingDone healingDone = new HealingDone();
                healingDone.healValue = specificMessage.healValue;
                healingDone.target = this.id;
                addOutgoingMessage(healingDone);

                messages.Dequeue();
            }
            else if (message is DefenseIncreased)
            {
                DefenseIncreased specificMessage = (DefenseIncreased)message;

                this.defense += specificMessage.defenseBonus;

                messages.Dequeue();
            }
            else if (message is DefenseDecreased)
            {
                DefenseDecreased specificMessage = (DefenseDecreased)message;

                this.defense -= specificMessage.defenseReduction;
                if (this.defense < 0)
                {
                    this.defense = 0;
                }

                messages.Dequeue();
            }
            else if (message is DamageOverTime)
            {
                DamageOverTime specificMessage = (DamageOverTime)message;

                //Default is to do nothing.  A sub class could process this instead and instantly heal the damage over time

                messages.Dequeue();
            }
            else if (message is Taunt)
            {
                Taunt specificMessage = (Taunt)message;

                //No resistance by default
                if (specificMessage.target.id == this.id)
                {
                    specificMessage.status = Taunt.Status.PassedTarget;
                    addOutgoingMessage(specificMessage);
                }
                else if (specificMessage.switchTarget.id == this.id)
                {
                    specificMessage.status = Taunt.Status.PassedSwitchTarget;
                    addOutgoingMessage(specificMessage);
                }

                messages.Dequeue();
            }
            else if (message is IsHPBelowXPercentage)
            {
                IsHPBelowXPercentage specificMessage = (IsHPBelowXPercentage)message;

                double percentage = (double)hp / (double)maxHp;
                YesOrNo yesOrNo = new YesOrNo();
                if (percentage <= specificMessage.percentage)
                {
                    yesOrNo.isYes = true;
                }

                yesOrNo.inquirer = specificMessage.inquirer;
                yesOrNo.answered = specificMessage.answerer;
                addOutgoingMessage(yesOrNo);

                messages.Dequeue();
            }
        }

        public override void processDeltaTime(int time)
        {
            currentTime = time;

            while (messages.Count > 0)
            {
                processMessage(messages.Dequeue());
            }

            if (isDefeated())
            {
                Defeated defeated = new Defeated();
                defeated.id = this.id;
                addOutgoingMessage(defeated);
            }
        }

        public override void processFinal()
        {

        }
    }
}
