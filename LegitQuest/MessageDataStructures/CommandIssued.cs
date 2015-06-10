using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class CommandIssued
    {
        public int commandNumber { get; set; } // 1 2 or 3
        public int source { get; set; } //1 2 or 3
        public int target { get; set; } //1 2 or 3.  0 Indicates a target all
    }
}
