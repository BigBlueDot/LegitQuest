using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.InternalMessage.DataRequests
{
    public class DataRequest : InternalMessage
    {
        public Guid source { get; set; }
    }
}
