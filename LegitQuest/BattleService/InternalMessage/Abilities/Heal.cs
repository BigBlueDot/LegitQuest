using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class Heal : InternalMessage
    {
        public int healValue { get; set; }
        public Guid source { get; set; }
        public Guid target { get; set; }
    }
}
