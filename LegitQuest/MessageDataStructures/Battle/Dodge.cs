using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Battle
{
    public class Dodge : Message
    {
        public Guid source { get; set; }
        public Guid target { get; set; }
    }
}
