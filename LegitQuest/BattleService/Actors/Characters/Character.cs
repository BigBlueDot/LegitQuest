using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.DataRequests;
using BattleServiceLibrary.InternalMessage.DataResults;
using MessageDataStructures;
using BattleServiceLibrary.InternalMessage;

namespace BattleServiceLibrary.Actors.Characters
{
    public abstract class Character : Actor
    {
        public string name { get; set; }
        public int hp { get; set; }
        public int maxHp { get; set; }
        public int strength { get; set; } //Physical attack
        public int dexterity { get; set; } //Physical attack for speed-based moves, increases accuracy, crit, and dodge
        public int vitality { get; set; } //Physical resistance, also contributes to some status resistance
        public int magic { get; set; } //Magic Attack
        public int mind { get; set; } //Healing strength
        public int resistance { get; set; } //Magic Defense
        public int accuracy { get; set; } //Higher hit rate, generally useful when fighting enemies much higher than you
        public int critical { get; set; } //Increases critical hit rate
        public int dodge { get; set; } //Increases chance to dodge

        public long currentTime { get; set; }
        public long castTimeComplete { get; set; }

        protected bool isDefeated()
        {
            return (hp <= 0);
        }

        protected virtual void processMessage(Message message)
        {
            if (message is PhysicalAttack)
            {
                PhysicalAttack specificMessage = (PhysicalAttack)message;

                int dmg = Convert.ToInt32(Math.Floor((Math.Pow((double)specificMessage.attack, 1.65f)) / ((double)this.vitality) * specificMessage.abilityStrength));
                hp -= dmg;

                DamageDealt damageDealt = new DamageDealt();
                damageDealt.damage = dmg;
                damageDealt.target = this.id;
                damageDealt.source = specificMessage.source;
                addOutgoingMessage(damageDealt);
            }
            else if (message is MagicalAttack)
            {
                MagicalAttack specificMessage = (MagicalAttack)message;

                int dmg = Convert.ToInt32(Math.Floor((Math.Pow((double)specificMessage.magicAttack, 1.65f)) / ((double)this.resistance) * specificMessage.abilityStrength));

                this.hp -= dmg;

                DamageDealt damageDealt = new DamageDealt();
                damageDealt.damage = dmg;
                damageDealt.target = this.id;
                damageDealt.source = specificMessage.source;
                addOutgoingMessage(damageDealt);
            }
            else if (message is Heal)
            {
                Heal specificMessage = (Heal)message;

                this.hp += specificMessage.healValue;

                HealingDone healingDone = new HealingDone();
                healingDone.healValue = specificMessage.healValue;
                healingDone.target = this.id;
                addOutgoingMessage(healingDone);
            }
            else if (message is DefenseIncreased)
            {
                DefenseIncreased specificMessage = (DefenseIncreased)message;

                this.vitality += specificMessage.defenseBonus;

            }
            else if (message is DefenseDecreased)
            {
                DefenseDecreased specificMessage = (DefenseDecreased)message;

                this.vitality -= specificMessage.defenseReduction;
                if (this.vitality < 0)
                {
                    this.vitality = 0;
                }
            }
            else if (message is DamageOverTime)
            {
                DamageOverTime specificMessage = (DamageOverTime)message;

                //Default is to do nothing.  A sub class could process this instead and instantly heal the damage over time

            }
            else if (message is DealStaticDamage)
            {
                DealStaticDamage specificMessage = (DealStaticDamage)message;

                int dmg = specificMessage.damage;

                this.hp -= dmg;

                DamageDealt damageDealt = new DamageDealt();
                damageDealt.damage = dmg;
                damageDealt.target = this.id;
                damageDealt.source = specificMessage.source;
                addOutgoingMessage(damageDealt);
            }
            else if (message is Taunt)
            {
                Taunt specificMessage = (Taunt)message;

                //No resistance by default
                if (specificMessage.target == this.id)
                {
                    specificMessage.status = Taunt.Status.PassedTarget;
                    addOutgoingMessage(specificMessage);
                }
                else if (specificMessage.switchTarget == this.id)
                {
                    specificMessage.status = Taunt.Status.PassedSwitchTarget;
                    addOutgoingMessage(specificMessage);
                }
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
            }
        }

        public override void process(long time)
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
    }
}
