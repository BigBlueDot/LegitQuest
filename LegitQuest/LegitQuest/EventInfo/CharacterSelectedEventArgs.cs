using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegitQuest.EventInfo
{
    public class CharacterSelectedEventArgs : EventArgs
    {
        public Guid id { get; set; }
    }
}
