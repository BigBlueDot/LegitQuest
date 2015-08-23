using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class DamageDealt : Message
    {
        public Guid target { get; set; }
        public int damage { get; set; }
    }
}
