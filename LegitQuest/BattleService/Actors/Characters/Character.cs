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
using BattleServiceLibrary.Utility;
using MessageDataStructures.Battle;

namespace BattleServiceLibrary.Actors.Characters
{
    public abstract class Character : Actor
    {
        public string name { get; set; }
        public int level { get; set; }
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

        public Guid engagedWith { get; set; }

        public Character()
        {
            castTimeComplete = 3000; //Have a buffer at the start of combat
        }

        protected bool isDefeated()
        {
            return (hp <= 0);
        }

        protected virtual void processMessage(Message message)
        {
            if (message is PhysicalAttack)
            {
                PhysicalAttack specificMessage = (PhysicalAttack)message;

                int hitChance = 95 + specificMessage.accuracy - this.dodge;
                double critChance = specificMessage.crit / Math.Pow(this.level, 1.1);
                double critModifier = 1.0;

                int dmg = 0;
                bool criticalHit = false;

                if (RNG.random.Next(100) < critChance)
                {
                    critModifier = 1.5;
                    criticalHit = true;
                }

                if (RNG.random.Next(100) < hitChance)
                {
                    dmg = Convert.ToInt32(Math.Floor((Math.Pow((double)specificMessage.attack, 1.65f)) / ((double)(this.vitality <= 0 ? 1 : this.vitality)) * specificMessage.abilityStrength * critModifier));

                    if (specificMessage.source == this.engagedWith)
                    {
                        dmg = Convert.ToInt32(dmg * 1.25);
                    }

                    hp -= dmg;

                    if(criticalHit)
                    {
                        Crit crit = new Crit();
                        crit.source = specificMessage.source;
                        crit.conversationId = specificMessage.conversationId;
                        crit.target = specificMessage.target;
                        this.addOutgoingMessage(crit);
                    }
                }
                else
                {
                    Dodge dodge = new Dodge();
                    dodge.conversationId = specificMessage.conversationId;
                    dodge.source = specificMessage.source;
                    dodge.target = specificMessage.target;
                    this.addOutgoingMessage(dodge);
                }


                DamageDealt damageDealt = new DamageDealt();
                damageDealt.damage = dmg;
                damageDealt.target = this.id;
                damageDealt.source = specificMessage.source;
                addOutgoingMessage(damageDealt);
            }
            else if (message is MagicalAttack)
            {
                MagicalAttack specificMessage = (MagicalAttack)message;

                int hitChance = 95 + specificMessage.accuracy - this.dodge;
                double critChance = specificMessage.crit / Math.Pow(this.level, 1.1);
                double critModifier = 1.0;

                int dmg = 0;
                bool criticalHit = false;

                if (RNG.random.Next(100) < critChance)
                {
                    critModifier = 1.5;
                    criticalHit = true;
                }


                if (RNG.random.Next(100) < hitChance)
                {
                    dmg = Convert.ToInt32(Math.Floor((Math.Pow((double)specificMessage.magicAttack, 1.65f)) / ((double)(this.resistance <= 0 ? 1 : this.resistance)) * specificMessage.abilityStrength * critModifier));

                    if (specificMessage.source == this.engagedWith)
                    {
                        dmg = Convert.ToInt32(dmg * 1.25);
                    }

                    if (criticalHit)
                    {
                        Crit crit = new Crit();
                        crit.source = specificMessage.source;
                        crit.conversationId = specificMessage.conversationId;
                        crit.target = specificMessage.target;
                        this.addOutgoingMessage(crit);
                    }
                }
                else
                {
                    Dodge dodge = new Dodge();
                    dodge.conversationId = specificMessage.conversationId;
                    dodge.source = specificMessage.source;
                    dodge.target = specificMessage.target;
                    this.addOutgoingMessage(dodge);
                }

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

                int heal = specificMessage.healValue;
                if (heal + this.hp > this.maxHp)
                {
                    heal = this.maxHp - this.hp;
                }

                this.hp += heal;

                HealingDone healingDone = new HealingDone();
                healingDone.healValue = heal;
                healingDone.conversationId = specificMessage.conversationId;
                healingDone.source = specificMessage.source;
                healingDone.target = this.id;
                addOutgoingMessage(healingDone);
            }
            else if (message is AttackIncreased)
            {
                AttackIncreased specificMessage = (AttackIncreased)message;

                this.strength += specificMessage.attackIncrease;
            }
            else if (message is AttackDecreased)
            {
                AttackDecreased specificMessage = (AttackDecreased)message;

                this.strength -= specificMessage.attackReduction;
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
