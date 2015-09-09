using BattleServiceLibrary.Actors;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities.Goblin
{
    public class GoblinPunch : Ability
    {
        public Guid source { get; set; }
        public Guid target { get; set; }
        public int potency { get; set; }
        public int physicalAttack { get; set; }

        public GoblinPunch()
        {
            this.hasComplexProcessing = true;
            this.execute = new Func<Ability, List<Actors.Actor>, List<Guid>, List<Guid>, List<InternalMessage>>((Ability ability, List<Actors.Actor> Actors, List<Guid> allies, List<Guid> enemies) =>
            {
                List<InternalMessage> messages = new List<InternalMessage>();
                GoblinPunch goblinPunch = (GoblinPunch)ability;
                int goblinCount = 0;

                foreach (Guid guid in enemies)
                {
                    //Create a MagicalAttack for each enemy
                    foreach (Actor actor in Actors)
                    {
                        if (actor.id == guid)
                        {
                            if(actor is Actors.Characters.Enemies.Goblin)
                            {
                                goblinCount++;
                            }
                        }
                    }
                }

                PhysicalAttack physicalAttack = new PhysicalAttack();
                physicalAttack.abilityStrength = 4 + (2 * goblinCount);
                physicalAttack.attack = goblinPunch.physicalAttack;
                physicalAttack.conversationId = ability.conversationId;
                physicalAttack.executeTime = ability.executeTime;
                physicalAttack.source = goblinPunch.source;
                physicalAttack.target = goblinPunch.target;
                messages.Add(physicalAttack);

                return messages;
            });

        }
    }
}
