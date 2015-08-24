using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Player
{
    public class CharacterBattleMessage : Message
    {
        public List<BattleCharacter> characters { get; set; }
    }
}
