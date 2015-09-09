using MessageDataStructures.BattleGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyServiceLibrary.EnemyGenerators
{
    public class SlimeGenerator
    {
        public MessageDataStructures.EnemyGeneration.Enemy getBlueSlime(EnemyType enemyType, int level)
        {
            MessageDataStructures.EnemyGeneration.Enemy enemy = new MessageDataStructures.EnemyGeneration.Enemy();
            enemy.name = "Blue Slime";
            enemy.hp = 30;
            enemy.maxHp = 30;
            enemy.strength = 5;
            enemy.dexterity = 4;
            enemy.vitality = 6;
            enemy.magic = 3;
            enemy.resistance = 6;
            enemy.accuracy = 5;
            enemy.dodge = 3;
            enemy.critical = 2;
            return enemy;
        }
    }
}
