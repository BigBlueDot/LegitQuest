﻿using MessageDataStructures.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class BattleInitialization : Message
    {
        public Guid battleId { get; set; }
        public int mana { get; set; }
        public ManaAffinity affinity { get; set; }
        public int affinityMana { get; set; }
        public List<ViewModels.Character> PlayerCharacters { get; set; }
        public List<ViewModels.Character> NonPlayerCharacters { get; set; }
    }
}
