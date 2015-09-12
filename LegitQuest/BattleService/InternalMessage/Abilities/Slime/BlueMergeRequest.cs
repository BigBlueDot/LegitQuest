using BattleServiceLibrary.Actors;
using BattleServiceLibrary.Actors.Characters;
using BattleServiceLibrary.Actors.Characters.Enemies;
using BattleServiceLibrary.InternalMessage.DataRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities.Slime
{
    public class BlueMergeRequest : AbilityDataRequest
    {
        public BlueMergeRequest()
        {
            this.hasComplexProcessing = true;
            this.execute = new Func<Ability, List<Actors.Actor>, List<Guid>, List<Guid>, List<InternalMessage>>((Ability ability, List<Actors.Actor> Actors, List<Guid> allies, List<Guid> enemies) =>
            {
                List<InternalMessage> messages = new List<InternalMessage>();
                BlueMergeRequest blueMerge = (BlueMergeRequest)ability;
                BlueMergeResult response = new BlueMergeResult();
                response.conversationId = blueMerge.conversationId;
                response.executeTime = blueMerge.executeTime;

                //Verify that another slime exists, and that this slime is in the optimal position
                bool useMerge = false;
                bool foundOtherSlime = false;
                int currentCount = 0;
                int thisIndex = 0;
                int maxIndex = 0;
                Guid targetId = new Guid();
                foreach (Guid guid in enemies)
                {
                    foreach (Actor actor in Actors)
                    {
                        if (actor.id == guid)
                        {
                            if (actor.id == blueMerge.source)
                            {
                                thisIndex = currentCount;
                                if (thisIndex > maxIndex)
                                {
                                    maxIndex = thisIndex;
                                }
                            }
                            else if (actor is BlueSlime || actor is RedSlime)
                            {
                                Character character = (Character)actor;
                                if (actor is BlueSlime)
                                {
                                    if (currentCount > maxIndex)
                                    {
                                        maxIndex = currentCount;
                                    }
                                }
                                if (character.hp < (character.maxHp / 2))
                                {
                                    foundOtherSlime = true;
                                    targetId = guid;
                                }
                            }
                        }
                    }
                    currentCount++;
                }

                if (foundOtherSlime && maxIndex == thisIndex)
                {
                    response.should = true;
                }
                response.target = targetId;
                response.inquirer = blueMerge.source;
                messages.Add(response);

                return messages;
            });
        }
    }
}
