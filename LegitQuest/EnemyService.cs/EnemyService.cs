using MessageDataStructures;
using MessageDataStructures.BattleGeneration;
using MessageDataStructures.EnemyGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyServiceLibrary
{
    public class EnemyService : BaseService
    {
        public EnemyService(MessageReader messageReader, MessageWriter messageWriter)
            : base(messageReader, messageWriter)
        {
            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            Message message = args.message;
            if (message is BattleGenerationMessage)
            {
                EnemyGeneratedMessage enemyGeneratedMessage = new EnemyGeneratedMessage();
                enemyGeneratedMessage.conversationId = message.conversationId;
                enemyGeneratedMessage.enemies = new List<MessageDataStructures.EnemyGeneration.Enemy>();
                foreach (MessageDataStructures.BattleGeneration.Enemy e in ((BattleGenerationMessage)message).enemies)
                {
                    MessageDataStructures.EnemyGeneration.Enemy enemy = getEnemy(e.enemyType,e.level);
                    enemy.battleGenerationInfo = e;
                    enemyGeneratedMessage.enemies.Add(enemy);
                }

                this.messageWriter.writeMessage(enemyGeneratedMessage);
            }
        }

        private MessageDataStructures.EnemyGeneration.Enemy getEnemy(EnemyType enemyType, int level)
        {
            //This will be structured differently later
            MessageDataStructures.EnemyGeneration.Enemy enemy = new MessageDataStructures.EnemyGeneration.Enemy();
            enemy.name = "Goblin";
            enemy.hp = 50;
            enemy.maxHp = 50;
            enemy.strength = 5;
            enemy.dexterity = 5;
            enemy.vitality = 5;
            enemy.magic = 5;
            enemy.resistance = 5;
            enemy.accuracy = 5;
            enemy.dodge = 5;
            enemy.critical = 5;
            return enemy;
        }
    }
}
