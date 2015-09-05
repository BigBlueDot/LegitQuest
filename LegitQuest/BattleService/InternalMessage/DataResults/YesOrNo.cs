using BattleServiceLibrary.InternalMessage.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataResults
{
    public class YesOrNo : InternalMessage
    {
        public bool isYes { get; set; }
        public Guid inquirer { get; set; }
        public Guid answered { get; set; }
    }
}
