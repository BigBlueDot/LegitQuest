using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageDataStructures;

namespace BattleService.Actors
{
    public abstract class Actor
    {
        public Guid id { get; set; }
        public Queue<Message> messages { get; set; }
        public Queue<Message> outgoingMessages { get; set; }

        public Actor()
        {
            messages = new Queue<Message>();
            outgoingMessages = new Queue<Message>();
        }

        public abstract void process(int time);

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
            outgoingMessages.Enqueue(message);
        }

        public List<Message> retrieveOutgoingMessages()
        {
            return outgoingMessages.ToList();
        }

        public void clearOutgoingMessages()
        {
            outgoingMessages.Clear();
        }
    }
}
