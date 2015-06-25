using BattleService.Actors;
using BattleService.InternalMessage;
using BattleService.InternalMessage.Abilities;
using BattleService.InternalMessage.DataRequests;
using BattleService.InternalMessage.DataResults;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService
{
    public class Battle
    {
        private List<InternalMessage.InternalMessage> messages { get; set; } //TODO:  Need to sort based on execute time
        private List<Message> globalMessages { get; set; }
        private Queue<Message> outgoingMessages { get; set; }
        private List<Actor> actors { get; set; }
        private List<Guid> allies { get; set; } //Allows it to track down ally actors for global data requests
        private List<Guid> enemies { get; set; } //Allows it to track down enemy actors for global data requests
        private long startingTime { get; set; }

        public Battle()
        {
            messages = new List<InternalMessage.InternalMessage>();
            outgoingMessages = new Queue<Message>();
            this.actors = new List<Actor>();
            this.startingTime = getCurrentTimeMs();
            this.globalMessages = new List<Message>();
            this.allies = new List<Guid>();
            this.enemies = new List<Guid>();
        }

        private long getCurrentTimeMs()
        {
            return (long)DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public void process()
        {
            //Local variable initalization
            List<Message> externalMessages = new List<Message>();

            //Get current time
            long currentTime = getCurrentTimeMs() - this.startingTime;

            //Assign all internal messages and process all events for actors
            do
            {
                foreach (InternalMessage.InternalMessage message in this.messages)
                {
                    assignMessage(message);
                }
                this.messages.Clear();

                processActorMessages(externalMessages, currentTime);
            } while ((this.messages.Count != 0)); //Need to run at least once


            //Write external messages
            foreach(Message message in externalMessages)
            {
                addOutgoingMessage(message);
            }
        }

        private void processActorMessages(List<Message> externalMessages, long currentTime)
        {
            //Process for each actor
            List<Guid> actorsToRemove = new List<Guid>();
            long defeatedTime = 0;
            foreach(Actor actor in this.actors)
            {
                actor.process(currentTime); //Note: currentTime is the actual current time for combat, not where processing is at
                List<Message> thisActorsMessages = new List<Message>();
                bool addMessages = true;
                foreach (Message message in actor.retrieveOutgoingMessages())
                {
                    if (message is Defeated)
                    {
                        //This actor was defeated, don't do anything else
                        addMessages = false;
                        actorsToRemove.Add(actor.id);
                        defeatedTime = ((Defeated)message).executeTime;
                    }
                }

                //Assign internal and external messages
                if (addMessages)
                {
                    foreach (Message message in actor.retrieveOutgoingMessages())
                    {
                        if (message is InternalMessage.InternalMessage)
                        {
                            this.messages.Add((InternalMessage.InternalMessage)message);
                        }
                        else
                        {
                            addOutgoingMessage(message);
                        }
                    }
                }
            }

            //Remove any defeated actors
            foreach (Guid id in actorsToRemove)
            {
                removeActor(id, defeatedTime);
            }

            //Process global messages
            foreach(Message message in this.globalMessages)
            {
                if (message is WhoIsEngagedWithMe)
                {
                    //Find the index of this enemy
                    WhoIsEngagedWithMe specificMessage = (WhoIsEngagedWithMe)message;
                    int index = enemies.IndexOf(specificMessage.source.id);
                    Guid target = allies[index];

                    //Verify that the target is alive
                    bool specificTargetAlive = false;
                    bool[] targetAlive = new bool[3];
                    foreach (Actor actor in actors)
                    {
                        if (actor.id == target)
                        {
                            specificTargetAlive = true;
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            if (actor.id == allies[i])
                            {
                                targetAlive[i] = true;
                            }
                        }
                    }

                    if (!specificTargetAlive)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (targetAlive[i])
                            {
                                target = allies[i];
                                break;
                            }
                        }
                    }

                    Target result = new Target();
                    result.id = target;
                    result.inquirer = specificMessage.source.id;
                    this.messages.Add(result);
                }

            }
            this.globalMessages.Clear();
        }

        private void removeActor(Guid id, long defeatedTime)
        {
            //Send a defeated external message
            ActorDefeated actorDefeated = new ActorDefeated();
            actorDefeated.targetId = id;
            addOutgoingMessage(actorDefeated);

            //Send a message to the ending condition
            Defeated defeated = new Defeated();
            defeated.executeTime = defeatedTime;
            defeated.id = id;
            messages.Add(defeated);

            //Remove the actor from records
            Actor toRemove = null;
            foreach (Actor actor in actors)
            {
                if(actor.id == id)
                {
                    toRemove = actor;
                }
            }

            actors.Remove(toRemove);
            this.allies.Remove(id);
            this.enemies.Remove(id);
        }

        private void assignMessages()
        {
            //Assign all messages that match the earliest timestamp 
            //This verifies that messages aren't processed when they aren't supposed to be
            if (this.messages.Count != 0)
            {
                this.messages.Sort(); //Sort by execution time
                long currentProcessingTime = this.messages[0].executeTime;

                foreach (InternalMessage.InternalMessage internalMessage in this.messages)
                {
                    if (internalMessage.executeTime == currentProcessingTime)
                    {
                        assignMessage(internalMessage);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void assignMessage(InternalMessage.InternalMessage message)
        {
            if (message is Targeted)
            {
                //Assign to the targetted actor
                foreach(Actor actor in actors)
                {
                    if(actor.id == ((Targeted)message).id)
                    {
                        actor.addEventMessage(message);
                    }
                }
            }
            else if (message is IsHPBelowXPercentage)
            {
                IsHPBelowXPercentage specificMessage = (IsHPBelowXPercentage)message;
                foreach (Actor actor in actors)
                {
                    if (specificMessage.answerer.id == actor.id)
                    {
                        actor.addEventMessage(message);
                    }
                }
            }
            else if (message is Target)
            {
                Target specificMessage = (Target)message;
                foreach (Actor actor in actors)
                {
                    if (specificMessage.inquirer == actor.id)
                    {
                        actor.addEventMessage(message);
                    }
                }
            }
            else if (message is WhoHasLowestHealth ||
                        message is WhoIsEngagedWithMe ||
                        message is WhoLacksStatusEffects)
            {
                this.messages.Add(message); //These need to be processed globally
            }
        }

        public void addOutgoingMessage(Message message)
        {
            //Do nothing for now
        }
    }
}
