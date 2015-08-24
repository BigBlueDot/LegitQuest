using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Battle
{
    public class AggregatedBattleInformation
    {
        public BattleGeneration.BattleGenerationMessage battleGenerationInfo { get; set; }
        public EnemyGeneration.EnemyGeneratedMessage enemyGenerationInfo { get; set; }
    }
}
