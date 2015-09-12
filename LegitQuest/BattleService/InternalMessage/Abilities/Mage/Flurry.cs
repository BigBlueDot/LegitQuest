using BattleServiceLibrary.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities.Mage
{
    public class Flurry : Ability
    {
        public Guid source { get; set; }
        public int potency { get; set; }
        public int magicAttack { get; set; }
        public int accuracy { get; set; }
        public int crit { get; set; }

        public Flurry()
        {
            this.hasComplexProcessing = true;
            this.execute = new Func<Ability, List<Actors.Actor>, List<Guid>, List<Guid>, List<InternalMessage>>((Ability ability, List<Actors.Actor> Actors, List<Guid> allies, List<Guid> enemies) =>
            {
                List<InternalMessage> messages = new List<InternalMessage>();
                Flurry flurry = (Flurry)ability;

                foreach (Guid guid in enemies)
                {
                    //Create a MagicalAttack for each enemy
                    foreach (Actor actor in Actors)
                    {
                        if (actor.id == guid)
                        {
                            MagicalAttack magicalAttack = new MagicalAttack();
                            magicalAttack.abilityStrength = flurry.potency;
                            magicalAttack.magicAttack = flurry.magicAttack;
                            magicalAttack.conversationId = flurry.conversationId;
                            magicalAttack.executeTime = flurry.executeTime;
                            magicalAttack.source = flurry.source;
                            magicalAttack.target = guid;
                            magicalAttack.accuracy = flurry.accuracy;
                            magicalAttack.crit = flurry.crit;
                            messages.Add(magicalAttack);
                        }
                    }

                }

                return messages;
            });

        }
    }
}
