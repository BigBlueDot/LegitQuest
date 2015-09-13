using MessageDataStructures.Player;
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
        public Guid characterId { get; set; }
        public Ability commandOne { get; set; }
        public bool commandOneEnabled { get; set; }
        public Ability commandTwo { get; set; }
        public bool commandTwoEnabled { get; set; }
        public Ability commandThree { get; set; }
        public bool commandThreeEnabled { get; set; }
    }
}
