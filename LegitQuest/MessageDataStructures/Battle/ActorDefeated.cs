﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class ActorDefeated : Message
    {
        public Guid targetId { get; set; }
    }
}
