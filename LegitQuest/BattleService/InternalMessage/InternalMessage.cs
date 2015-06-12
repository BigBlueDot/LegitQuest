using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageDataStructures;

namespace BattleService.InternalMessage
{
    public class InternalMessage : Message, IComparable
    {
        public int executeTime { get; set; }

        public int CompareTo(object obj)
        {
            return ((InternalMessage)obj).executeTime - this.executeTime;
        }
    }
}
