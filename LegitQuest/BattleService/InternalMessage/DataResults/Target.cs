using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataResults
{
    public class Target : InternalMessage
    {
        public Guid target { get; set; }
        public Guid source { get; set; }
    }
}
