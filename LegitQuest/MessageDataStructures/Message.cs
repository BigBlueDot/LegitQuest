using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class Message
    {
        public Guid conversationId { get; set; } 
        public string className {  get { return this.GetType().Name; } }
    }
}
