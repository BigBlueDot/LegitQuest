using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Battle
{
    public class MaxHPChange : Message
    {
        public Guid target { get; set; }
        public int maxHPMod { get; set; }
    }
}
