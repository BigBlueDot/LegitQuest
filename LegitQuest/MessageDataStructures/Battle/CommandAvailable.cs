using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class CommandAvailable : Message
    {
        //TODO: Add graphic information as well
        public string commandOne { get; set; }
        public string commandTwo { get; set; }
        public string commandThree { get; set; }
    }
}
