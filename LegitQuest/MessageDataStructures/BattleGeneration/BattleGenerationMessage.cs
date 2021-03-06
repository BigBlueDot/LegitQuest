﻿using MessageDataStructures.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.BattleGeneration
{
    public class BattleGenerationMessage : Message
    {
        public int mana { get; set; }
        public ManaAffinity affinity { get; set; }
        public int affinityMana { get; set; }
        public List<Enemy> enemies { get; set; }
        public FieldType fieldType { get; set; }
    }
}
