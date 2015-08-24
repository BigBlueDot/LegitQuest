using MessageDataStructures;
using MessageDataStructures.BattleGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleGenerationService
{
    public class BattleGenerationService
    {
        private MessageReader messageReader { get; set; }
        private MessageWriter messageWriter { get; set; }

        public BattleGenerationService(MessageReader messageReader, MessageWriter messageWriter)
        {
            this.messageReader = messageReader;
            this.messageWriter = messageWriter;

            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        private void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            Message message = args.message;
            if (message is BattleGenerationRequest)
            {
                BattleGenerationMessage battleGenerationMessage = new BattleGenerationMessage();
                battleGenerationMessage.fieldType = FieldType.Plains;
                battleGenerationMessage.enemies = new List<EnemyType>();
                battleGenerationMessage.enemies.Add(EnemyType.Goblin);
                battleGenerationMessage.enemies.Add(EnemyType.Goblin);
                battleGenerationMessage.enemies.Add(EnemyType.Goblin);
                this.messageWriter.writeMessage(battleGenerationMessage);
            }
        }
    }
}
