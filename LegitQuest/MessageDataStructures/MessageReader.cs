using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public delegate void MessageReceivedEventHandler(MessageDataStructures.MessageReceivedEventArgs args);

    public interface MessageReader
    {
        event MessageReceivedEventHandler MessageReceived;
    }
}
