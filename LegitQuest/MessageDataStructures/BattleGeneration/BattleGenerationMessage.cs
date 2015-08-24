using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.BattleGeneration
{
    public class BattleGenerationMessage : Message
    {
        public List<EnemyType> enemies { get; set; }
        public FieldType fieldType { get; set; }
    }
}
