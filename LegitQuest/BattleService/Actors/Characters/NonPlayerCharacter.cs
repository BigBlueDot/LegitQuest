using BattleServiceLibrary.InternalMessage.Abilities.TriAttack;
using MessageDataStructures;
using MessageDataStructures.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters
{
    public abstract class NonPlayerCharacter : Character
    {
        protected abstract void doAction();
        protected List<TriAttackContribution> triAttackContribution = new List<TriAttackContribution>();

        public override void process(long time)
        {
            //Determine if they should do an attack
            if (castTimeComplete <= time)
            {
                doAction();
            }

            List<TriAttackContribution> toRemove = new List<TriAttackContribution>();
            foreach (TriAttackContribution tac in triAttackContribution)
            {
                if (tac.executeTime + 5000 < time)
                {
                    toRemove.Add(tac);
                }
            }

            foreach (TriAttackContribution tac in toRemove)
            {
                triAttackContribution.Remove(tac);
            }

            base.process(time);
        }

        protected override void processMessage(MessageDataStructures.Message message)
        {
            if (message is TriAttackContribution)
            {
                TriAttackContribution triAttackContributionMessage = (TriAttackContribution)message;
                this.triAttackContribution.Add(triAttackContributionMessage);
                if (this.triAttackContribution.Count >= 3)
                {
                    List<Guid> sources = new List<Guid>();
                    foreach (TriAttackContribution tac in this.triAttackContribution)
                    {
                        if (!sources.Contains(tac.source))
                        {
                            sources.Add(tac.source);
                        }
                    }

                    if (sources.Count >= 3)
                    {
                        //It was from three different sources
                        Dictionary<TriAttackType, int> triAttackTypeCount = new Dictionary<TriAttackType, int>();

                        foreach (TriAttackContribution tac in this.triAttackContribution)
                        {
                            foreach (TriAttackInfo triAttackInfo in tac.contributions)
                            {
                                if (triAttackTypeCount.ContainsKey(triAttackInfo.type))
                                {
                                    triAttackTypeCount[triAttackInfo.type] += triAttackInfo.contribution;
                                }
                                else
                                {
                                    triAttackTypeCount.Add(triAttackInfo.type, triAttackInfo.contribution);
                                }
                            }
                        }

                        int max = 0;
                        TriAttackType currentType = TriAttackType.None;
                        foreach (TriAttackType triAttackType in triAttackTypeCount.Keys)
                        {
                            if (triAttackTypeCount[triAttackType] > max)
                            {
                                max = triAttackTypeCount[triAttackType];
                                currentType = triAttackType;
                            }
                        }

                        if (max >= 100)
                        {
                            if (currentType == TriAttackType.Strike)
                            {
                                int damageDone = this.level * 300 / this.vitality;
                                this.hp -= damageDone;

                                DamageDealt damageDealt = new DamageDealt();
                                damageDealt.conversationId = message.conversationId;
                                damageDealt.damage = damageDone;
                                damageDealt.source = new Guid();
                                damageDealt.target = this.id;
                                addOutgoingMessage(damageDealt);

                                AbilityUsed abilityUsed = new AbilityUsed();
                                abilityUsed.conversationId = message.conversationId;
                                abilityUsed.message = "A striking Triple Attack has been inflicted on " + this.name + "!";
                                addOutgoingMessage(abilityUsed);
                            }
                        }
                    }
                }
            }

            base.processMessage(message);
        }
    }
}
