﻿using MessageDataStructures.BattleGeneration;
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
            enemy.hp = 100;
            enemy.maxHp = 100;
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

        public MessageDataStructures.EnemyGeneration.Enemy getRedSlime(EnemyType enemyType, int level)
        {
            MessageDataStructures.EnemyGeneration.Enemy enemy = new MessageDataStructures.EnemyGeneration.Enemy();
            enemy.name = "Red Slime";
            enemy.hp = 100;
            enemy.maxHp = 100;
            enemy.strength = 7;
            enemy.dexterity = 4;
            enemy.vitality = 3;
            enemy.magic = 3;
            enemy.resistance = 5;
            enemy.accuracy = 4;
            enemy.dodge = 3;
            enemy.critical = 3;
            return enemy;
        }
    }
}
