﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Battle
{
    public class UseMana : Message
    {
        public int mana { get; set; }
        public ManaAffinity affinity { get; set; }
        public int affinityMana { get; set; }
    }
}
