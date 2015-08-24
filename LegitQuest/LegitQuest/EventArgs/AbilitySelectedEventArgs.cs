using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegitQuest.EventInfo
{
    public class AbilitySelectedEventArgs : EventArgs
    {
        public CommandIssued commandIssued { get; set; }
    }
}
