using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage
{
    public class Defeated
    {
        public bool isAlly { get; set; }
        public int id { get; set; } //1 2 or 3
    }
}
