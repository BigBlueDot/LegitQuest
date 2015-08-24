using MessageDataStructures;
using MessageDataStructures.Battle;
using MessageDataStructures.BattleGeneration;
using MessageDataStructures.EnemyGeneration;
using MessageDataStructures.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorServiceLibrary
{
    public class BattleAggregator
    {
        private Dictionary<Guid, BattleAggregatorInfo> aggregations;

        public BattleAggregator()
        {
            aggregations = new Dictionary<Guid, BattleAggregatorInfo>();
        }

        internal class BattleAggregatorInfo
        {
            internal MessageWriter messageWriter { get; set; }
            internal AggregatedBattleInformation aggregatedBattleInformation { get; set; }

            internal bool isReady()
            {
                //Return true once all data is ready to go
                return aggregatedBattleInformation.battleGenerationInfo != null && aggregatedBattleInformation.enemyGenerationInfo != null && aggregatedBattleInformation.characterBattleInfo != null;
            }
        }

        public void addAggregatedInformation(AggregatedBattleInformation aggregatedBattleInformation, MessageWriter targetWriter)
        {
            BattleAggregatorInfo battleAggregatorInfo = new BattleAggregatorInfo();
            battleAggregatorInfo.aggregatedBattleInformation = aggregatedBattleInformation;
            battleAggregatorInfo.messageWriter = targetWriter;
            aggregations.Add(aggregatedBattleInformation.conversationId, battleAggregatorInfo);
        }

        public void addBattleGenerationMessageInfo(Guid conversationId, BattleGenerationMessage battleGenerationMessage)
        {
            this.aggregations[conversationId].aggregatedBattleInformation.battleGenerationInfo = battleGenerationMessage;
            checkMessage(conversationId);
        }

        public void addEnemyGenerationMessageInfo(Guid conversationId, EnemyGeneratedMessage enemyGenerationMessage)
        {
            this.aggregations[conversationId].aggregatedBattleInformation.enemyGenerationInfo = enemyGenerationMessage;
            checkMessage(conversationId);
        }

        public void addPlayerCharacterMessageInfo(Guid conversationId, CharacterBattleMessage characterBattleMessage)
        {
            this.aggregations[conversationId].aggregatedBattleInformation.characterBattleInfo = characterBattleMessage;
            checkMessage(conversationId);
        }

        private void checkMessage(Guid guid)
        {
            //Check to see if the message is ready and then send it if so
            if (this.aggregations[guid].isReady())
            {
                this.aggregations[guid].messageWriter.writeMessage(this.aggregations[guid].aggregatedBattleInformation);
                this.aggregations.Remove(guid);
            }
        }
    }
}
