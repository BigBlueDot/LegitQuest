using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataResults
{
    public class DataResult : InternalMessage
    {
        public Guid inquirer { get; set; }
    }
}
