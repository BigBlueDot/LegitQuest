﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService.InternalMessage.DataResults
{
    public class TargetList : InternalMessage
    {
        public List<Abilities.Targeted> targets { get; set; }
    }
}