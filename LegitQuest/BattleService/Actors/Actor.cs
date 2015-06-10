using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.Actors
{
    public abstract class Actor
    {
        public Guid id { get; set; }
        public Queue<InternalMessage.InternalMessage> messages { get; set; }
        public Queue<InternalMessage.InternalMessage> outgoingMessages { get; set; }

        public Actor()
        {
            messages = new Queue<InternalMessage.InternalMessage>();
            outgoingMessages = new Queue<InternalMessage.InternalMessage>();
        }

        public abstract List<InternalMessage.InternalMessage> processEvents();
        public abstract List<InternalMessage.InternalMessage> processDeltaTime(int time);
        public abstract List<InternalMessage.InternalMessage> processFinal();

        public void addEventMessage(InternalMessage.InternalMessage message)
        {
            messages.Enqueue(message);
        }

        public void clearEventMessages()
        {
            messages.Clear();
        }

        public void addOutgoingMessage(InternalMessage.InternalMessage message)
        {
            outgoingMessages.Enqueue(message);
        }

        public List<InternalMessage.InternalMessage> retrieveOutgoingMessages()
        {
            return outgoingMessages.ToList();
        }

        public void clearOutgoingMessages()
        {
            outgoingMessages.Clear();
        }
    }
}
