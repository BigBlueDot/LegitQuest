using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Battle
{
    public class Crit : Message
    {
        public Guid target { get; set; }
        public Guid source { get; set; }
    }
}
