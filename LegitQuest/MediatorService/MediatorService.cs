using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleGenerationServiceLibrary;
using BattleServiceLibrary;
using PlayerServiceLibrary;
using EnemyServiceLibrary;

namespace MediatorServiceLibrary
{
    public class MediatorService
    {
        private Dictionary<ServiceType, MessageDataStructures.DirectMessageReader> writers;
        private DirectMessageReader messageReader;

        private BattleGenerationService battleGenerationService;
        private BattleService battleService;
        private EnemyService enemyService;
        private PlayerService playerService;

        public MediatorService()
        {
            initReader();
            initWriters();
            initServices();
        }

        private void initServices()
        {
            this.battleGenerationService = new BattleGenerationService(this.writers[ServiceType.BattleGeneration], this.messageReader);
            this.battleService = new BattleService(this.writers[ServiceType.BattleGeneration], this.messageReader);
            this.enemyService = new EnemyService(this.writers[ServiceType.BattleGeneration], this.messageReader);
            this.playerService = new PlayerService(this.writers[ServiceType.BattleGeneration], this.messageReader);
        }

        private void initWriters()
        {
            writers = new Dictionary<ServiceType, MessageDataStructures.DirectMessageReader>();
            writers.Add(ServiceType.Battle, new DirectMessageReader());
            writers.Add(ServiceType.BattleGeneration, new DirectMessageReader());
            writers.Add(ServiceType.Enemy, new DirectMessageReader());
            writers.Add(ServiceType.Player, new DirectMessageReader());
        }

        private void initReader()
        {
            this.messageReader = new DirectMessageReader();
            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            //Begin message routing here
            throw new NotImplementedException();
        }
    }
}
