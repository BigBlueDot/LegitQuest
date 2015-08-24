using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities
{
    public class Heal : Targeted
    {
        public int healValue { get; set; }
    }
}
