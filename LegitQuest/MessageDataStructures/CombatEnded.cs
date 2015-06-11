using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class CombatEnded : Message
    {
        public bool playerWon { get; set; }
        public int xp { get; set; }
        public List<ViewModels.Treasure> treasure { get; set; }
    }
}
