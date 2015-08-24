using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.EnemyGeneration
{
    public class EnemyGeneratedMessage : Message
    {
        public List<Enemy> enemies { get; set; }
    }
}
