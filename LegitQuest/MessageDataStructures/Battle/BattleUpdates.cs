﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class BattleUpdates : Message
    {
        public List<BattleUpdate> updates { get; set; }
    }
}