using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    //EventArgs derived wrapper for messages
    public class MessageReceivedEventArgs : EventArgs
    {
        public Message message { get; set; }
    }
}
