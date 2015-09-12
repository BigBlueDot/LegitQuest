using MessageDataStructures;
using MessageDataStructures.BattleGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleGenerationServiceLibrary
{
    public class BattleGenerationService : BaseService
    {
        public BattleGenerationService(MessageReader messageReader, MessageWriter messageWriter)
            : base(messageReader, messageWriter)
        {
            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        private void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            Message message = args.message;
            if (message is BattleGenerationRequest)
            {
                BattleGenerationMessage battleGenerationMessage = new BattleGenerationMessage();
                battleGenerationMessage.fieldType = FieldType.Plains;
                battleGenerationMessage.enemies = new List<Enemy>();
                battleGenerationMessage.enemies.Add(new Enemy() { level = 1, enemyType = EnemyType.RedSlime });
                battleGenerationMessage.enemies.Add(new Enemy() { level = 1, enemyType = EnemyType.RedSlime });
                battleGenerationMessage.enemies.Add(new Enemy() { level = 1, enemyType = EnemyType.RedSlime });
                this.messageWriter.writeMessage(battleGenerationMessage);
            }
        }
    }
}
