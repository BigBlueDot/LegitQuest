using BattleServiceLibrary.Actors;
using BattleServiceLibrary.Actors.Characters;
using BattleServiceLibrary.InternalMessage;
using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.InternalMessage.DataRequests;
using BattleServiceLibrary.InternalMessage.DataResults;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary
{
    public class Battle
    {
        private List<InternalMessage.InternalMessage> messages { get; set; } //TODO:  Need to sort based on execute time
        private List<Message> globalMessages { get; set; }
        private List<Message> outgoingMessages { get; set; }
        private List<Actor> actors { get; set; }
        private List<Guid> allies { get; set; } //Allows it to track down ally actors for global data requests
        private List<Guid> enemies { get; set; } //Allows it to track down enemy actors for global data requests
        private long startingTime { get; set; }
        public Guid id { get; set; }

        public Battle(Guid id, Actor pointCharacter, Actor leftWingCharacter, Actor rightWingCharacter, Actor pointEnemy, Actor leftWingEnemy, Actor rightWingEnemy)
        {
            messages = new List<InternalMessage.InternalMessage>();
            outgoingMessages = new List<Message>();
            this.actors = new List<Actor>();
            this.startingTime = getCurrentTimeMs();
            this.globalMessages = new List<Message>();
            this.allies = new List<Guid>();
            this.enemies = new List<Guid>();
            this.id = id;
            this.allies.Add(pointCharacter.id);
            this.allies.Add(leftWingCharacter.id);
            this.allies.Add(rightWingCharacter.id);
            this.enemies.Add(pointEnemy.id);
            this.enemies.Add(leftWingEnemy.id);
            this.enemies.Add(rightWingEnemy.id);

            this.actors.Add(pointCharacter);
            this.actors.Add(leftWingCharacter);
            this.actors.Add(rightWingCharacter);
            this.actors.Add(pointEnemy);
            this.actors.Add(leftWingEnemy);
            this.actors.Add(rightWingEnemy);
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
                List<InternalMessage.InternalMessage> toProcess = new List<InternalMessage.InternalMessage>();
                toProcess.AddRange(this.messages);
                this.messages.Clear();
                foreach (InternalMessage.InternalMessage message in toProcess)
                {
                    assignMessage(message);
                }
                toProcess.Clear();

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
                        checkCombatEnded();
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

                    actor.clearOutgoingMessages();
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
                    int index = enemies.IndexOf(specificMessage.source);
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
                    result.target = target;
                    result.source = specificMessage.source;
                    this.messages.Add(result);
                }
                else if (message is WhoHasLowestHealth)
                {
                    WhoHasLowestHealth specificMessage = (WhoHasLowestHealth)message;
                    int currentLowest = int.MaxValue;
                    Guid currentLowestId = new Guid();
                    List<Guid> guidsToCheck = new List<Guid>();

                    if (specificMessage.isAlly)
                    {
                        guidsToCheck.AddRange(allies);
                    }
                    else
                    {
                        guidsToCheck.AddRange(enemies);
                    }

                    foreach (Guid id in guidsToCheck)
                    {
                        foreach (Actor actor in actors)
                        {
                            if (id == actor.id)
                            {
                                if (((Character)actor).hp < currentLowest)
                                {
                                    currentLowest = ((Character)actor).hp;
                                    currentLowestId = ((Character)actor).id;
                                }
                            }
                        }
                    }

                    Target target = new Target();
                    target.source = specificMessage.inquirer;
                    target.target = currentLowestId;
                    this.messages.Add(target);
                }
                else if (message is WhoLacksStatusEffects)
                {
                    //TODO: Need to write once status effects are more fleshed out
                    WhoLacksStatusEffects specificMessage = (WhoLacksStatusEffects)message;
                    
                }
                else if (message is CommandIssued)
                {
                    foreach (Actor actor in actors)
                    {
                        if (actor.id == ((CommandIssued)message).source)
                        {
                            actor.addEventMessage(message);
                        }
                    }
                }

            }
            this.globalMessages.Clear();
        }

        private void checkCombatEnded()
        {
            bool alliesDefeated = true;
            foreach (Guid id in allies)
            {
                foreach (Actor actor in actors)
                {
                    if (actor.id == id)
                    {
                        if (actor is Character)
                        {
                            if (((Character)actor).hp > 0)
                            {
                                alliesDefeated = false;
                            }
                        }
                    }
                }
            }

            if (alliesDefeated)
            {
                CombatEnded combatEnded = new CombatEnded();
                combatEnded.playerWon = false;
                combatEnded.treasure = new List<MessageDataStructures.ViewModels.Treasure>();
                combatEnded.xp = 0;
                this.addOutgoingMessage(combatEnded);
            }

            bool enemiesDefeated = true;
            foreach (Guid id in enemies)
            {
                foreach (Actor actor in actors)
                {
                    if (actor.id == id)
                    {
                        if (actor is Character)
                        {
                            if (((Character)actor).hp > 0)
                            {
                                enemiesDefeated = false;
                            }
                        }
                    }
                }
            }

            if (enemiesDefeated)
            {
                CombatEnded combatEnded = new CombatEnded();
                combatEnded.playerWon = true;
                combatEnded.treasure = new List<MessageDataStructures.ViewModels.Treasure>();
                combatEnded.xp = 0;
                this.addOutgoingMessage(combatEnded);
            }
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
            if (message is DamageOverTime ||
                message is DealStaticDamage ||
                message is DefenseDecreased ||
                message is DefenseIncreased ||
                message is MagicalAttack ||
                message is PhysicalAttack ||
                message is Taunt ||
                message is Heal)
            {
                //Assign to the targetted actor
                Guid target = new Guid();

                if (message is DamageOverTime)
                {
                    target = ((DamageOverTime)message).target;
                }
                else if (message is DealStaticDamage)
                {
                    target = ((DealStaticDamage)message).target;
                }
                else if (message is DefenseDecreased)
                {
                    target = ((DefenseDecreased)message).target;
                }
                else if (message is DefenseIncreased)
                {
                    target = ((DefenseIncreased)message).target;
                }
                else if (message is MagicalAttack)
                {
                    target = ((MagicalAttack)message).target;
                }
                else if (message is PhysicalAttack)
                {
                    target = ((PhysicalAttack)message).target;
                }
                else if (message is Taunt)
                {
                    target = ((Taunt)message).target;
                }
                else if (message is Heal)
                {
                    target = ((Heal)message).target;
                }
                else if (message is DealStaticDamage)
                {
                    target = ((DealStaticDamage)message).target;
                }


                foreach(Actor actor in actors)
                {
                    if (actor.id == target)
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
                    if (specificMessage.answerer == actor.id)
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
                    if (specificMessage.source == actor.id)
                    {
                        actor.addEventMessage(message);
                    }
                }
            }
            else if (message is WhoHasLowestHealth ||
                        message is WhoIsEngagedWithMe ||
                        message is WhoLacksStatusEffects)
            {
                this.globalMessages.Add(message); //These need to be processed globally
            }
        }

        public void addInternalMessage(Message message)
        {
            lock (messages)
            {
                globalMessages.Add(message);
            }
        }

        public void addOutgoingMessage(Message message)
        {
            this.outgoingMessages.Add(message);
        }

        public List<Message> getOutgoingMessages()
        {
            return this.outgoingMessages;
        }

        public void clearOutgoingMessages()
        {
            this.outgoingMessages.Clear();
        }
    }
}
