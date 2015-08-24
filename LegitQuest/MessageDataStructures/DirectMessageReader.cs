using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    /*
     * This is designed to work as an same-thread implementation of readers and writers
     * This makes it very easy to move to a distributed architecture in the future
     * Note that this is synchronous
     * This is also a one-way communication.  Each channel between services will need two of these
     */ 
    public class DirectMessageReader : MessageReader, MessageWriter
    {
        public event MessageReceivedEventHandler MessageReceived;

        public void writeMessage(Message message)
        {
            if (MessageReceived != null)
            {
                MessageReceivedEventArgs args = new MessageReceivedEventArgs();
                args.message = message;
                this.MessageReceived(args);
            }
        }
    }
}
