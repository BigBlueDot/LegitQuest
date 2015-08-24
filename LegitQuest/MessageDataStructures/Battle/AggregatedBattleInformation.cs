using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDataStructures.Battle
{
    public class AggregatedBattleInformation : Message
    {
        public BattleGeneration.BattleGenerationMessage battleGenerationInfo { get; set; }
        public EnemyGeneration.EnemyGeneratedMessage enemyGenerationInfo { get; set; }
        public Player.CharacterBattleMessage characterBattleInfo { get; set; }
    }
}
