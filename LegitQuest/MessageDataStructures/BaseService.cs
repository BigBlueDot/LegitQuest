using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class BaseService
    {
        protected MessageReader messageReader { get; set; }
        protected MessageWriter messageWriter { get; set; }

        public BaseService(MessageReader messageReader, MessageWriter messageWriter)
        {
            this.messageReader = messageReader;
            this.messageWriter = messageWriter;
        }
    }
}
