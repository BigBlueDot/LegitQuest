using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.Abilities
{
    public abstract class Targeted
    {
        bool isAlly { get; set; }
        int target { get; set; } //1 2 or 3
    }
}
