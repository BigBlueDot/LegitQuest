using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageDataStructures;

namespace BattleServiceLibrary.Actors
{
    public abstract class Actor
    {
        public Guid id { get; set; }
        public Queue<Message> messages { get; set; }
        public List<Message> outgoingMessages { get; set; }

        public Actor()
        {
            messages = new Queue<Message>();
            outgoingMessages = new List<Message>();
        }

        public abstract void process(long time);

        public void addEventMessage(Message message)
        {
            messages.Enqueue(message);
        }

        public void clearEventMessages()
        {
            messages.Clear();
        }

        public void addOutgoingMessage(Message message)
        {
            outgoingMessages.Add(message);
        }

        public List<Message> retrieveOutgoingMessages()
        {
            return outgoingMessages.ToList();
        }

        public virtual void removeMessagesAfterDefeat()
        {
            //Remove any messages that shouldn't be sent out after a defeat
            List<Message> toRemove = new List<Message>();
            foreach (Message message in this.outgoingMessages)
            {
                if (message is DamageDealt)
                {

                }
                else
                {
                    toRemove.Add(message);
                }
            }

            foreach (Message message in toRemove)
            {
                this.outgoingMessages.Remove(message);
            }
        }

        public void clearOutgoingMessages()
        {
            outgoingMessages.Clear();
        }
    }
}
