using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class HealingDone : Message
    {
        public Guid target { get; set; }
        public int healValue { get; set; }
    }
}
