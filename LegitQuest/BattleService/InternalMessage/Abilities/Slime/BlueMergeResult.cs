using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.Abilities.Slime
{
    public class BlueMergeResult : DataResults.DataResult
    {
        public Guid target { get; set; }
        public bool should { get; set; }
    }
}
