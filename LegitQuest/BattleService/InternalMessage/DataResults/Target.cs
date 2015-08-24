using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataResults
{
    public class Target : Targeted
    {
        public Guid inquirer { get; set; }
    }
}
