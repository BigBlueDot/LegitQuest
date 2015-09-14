using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage
{
    public class TargetedMessage : InternalMessage
    {
        public Guid target { get; set; }
        public Guid source { get; set; }
    }
}
