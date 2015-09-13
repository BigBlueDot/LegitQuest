using BattleServiceLibrary.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities.Priest
{
    public class Prayer : AbilityMessage
    {
        public int healValue { get; set; }
        public Guid source { get; set; }

        public Prayer()
        {
            this.hasComplexProcessing = true;
            this.execute = new Func<AbilityMessage, List<Actors.Actor>, List<Guid>, List<Guid>, List<InternalMessage>>((AbilityMessage ability, List<Actors.Actor> Actors, List<Guid> allies, List<Guid> enemies) =>
            {
                List<InternalMessage> messages = new List<InternalMessage>();
                Prayer prayer = (Prayer)ability;

                foreach (Guid guid in allies)
                {
                    foreach (Actor actor in Actors)
                    {
                        if (actor.id == guid)
                        {
                            Heal heal = new Heal();
                            heal.conversationId = prayer.conversationId;
                            heal.executeTime = prayer.executeTime;
                            heal.healValue = prayer.healValue;
                            heal.source = prayer.source;
                            heal.target = guid;
                            messages.Add(heal);
                        }
                    }
                }

                return messages;
            });
        }
    }
}
