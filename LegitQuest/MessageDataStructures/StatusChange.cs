using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class StatusChange
    {
        public bool isAdd { get; set; } //False if status is removed
        public bool targetIsAlly { get; set; }
        public int target { get; set; } //1 2 or 3
        public String name { get; set; }
        //TODO: add graphic information
    }
}
