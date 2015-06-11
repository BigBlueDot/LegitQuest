using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures
{
    public class BattleInitialization : Message
    {
        public List<ViewModels.Character> PlayerCharacters { get; set; }
        public List<ViewModels.Character> NonPlayerCharacters { get; set; }
    }
}
