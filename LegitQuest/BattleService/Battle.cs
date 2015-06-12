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
        public SortedSet<InternalMessage.InternalMessage> messages { get; set; }
        public Queue<Message> outgoingMessages { get; set; }

        public Battle()
        {
            messages = new SortedSet<InternalMessage.InternalMessage>();
            outgoingMessages = new Queue<Message>();
        }


        public void addOutgoingMessage(Message message)
        {
            //Do nothing for now
        }
    }
}
