using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    /*
     * Just a simple MessageWriter interface so that I can avoid getting tightly coupled to any specific messaging package
     * (Probably will just be using it for distinguishing between in-process messaging and message queue messaging
     */
    public interface MessageWriter
    {
        void writeMessage(Message message);
    }
}
