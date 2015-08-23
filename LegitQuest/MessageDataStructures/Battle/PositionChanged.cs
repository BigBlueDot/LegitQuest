using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class PositionChanged : Message
    {
        public bool isAlly { get; set; }
        public int positionOne { get; set; } //1 2 or 3
        public int positionTwo { get; set; } //1 2 or 3
    }
}
