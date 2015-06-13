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
        public long executeTime { get; set; }

        public int CompareTo(object obj)
        {
            long value = ((InternalMessage)obj).executeTime - this.executeTime;
            int returnValue;
            if(value > 0)
            {
                returnValue = 1;
            }
            else if (value < 0)
            {
                returnValue = -1;
            }
            else{
                returnValue = 0;
            }
            return returnValue;
        }
    }
}
