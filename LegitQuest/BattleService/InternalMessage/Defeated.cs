using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage
{
    public class Defeated : InternalMessage
    {
        public bool isAlly { get; set; }
        public Guid id { get; set; } //1 2 or 3
    }
}
