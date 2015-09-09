using MessageDataStructures.BattleGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyServiceLibrary.EnemyGenerators
{
    public class GoblinGenerator
    {
        public MessageDataStructures.EnemyGeneration.Enemy getEnemy(EnemyType enemyType, int level)
        {
            MessageDataStructures.EnemyGeneration.Enemy enemy = new MessageDataStructures.EnemyGeneration.Enemy();
            enemy.name = "Goblin";
            enemy.hp = 35;
            enemy.maxHp = 35;
            enemy.strength = 6;
            enemy.dexterity = 3;
            enemy.vitality = 4;
            enemy.magic = 3;
            enemy.resistance = 2;
            enemy.accuracy = 6;
            enemy.dodge = 4;
            enemy.critical = 4;
            return enemy;
        }
    }
}
