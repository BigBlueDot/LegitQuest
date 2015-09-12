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
using GuiServiceLibrary;
using MessageDataStructures.BattleGeneration;
using MessageDataStructures.Battle;
using MediatorServiceLibrary;
using MessageDataStructures.Player;
using MessageDataStructures.EnemyGeneration;
using System.Threading;
using System.ComponentModel;

namespace MediatorServiceLibrary
{
    public class MediatorService
    {
        /* this is kind of confusing, but writers are how the mediator write to a service,
         * and readers are where the Mediator receives new messages.  The reader here corresponds
         * to the writers elsewhere, and the writers here correspond to each individual reader
         */ 
        private Dictionary<ServiceType, MessageDataStructures.DirectMessageReader> writers;
        private DirectMessageReader messageReader;

        private BattleGenerationService battleGenerationService;
        private BattleService battleService;
        private EnemyService enemyService;
        private PlayerService playerService;

        //Aggregators
        private BattleAggregator battleAggregator;

        //Threads
        private Thread battleThread;

        public MediatorService(ref DirectMessageReader guiMessageWriter, ref DirectMessageReader guiMessageReader)
        {
            initReader();
            initWriters();
            initServices();
            initAggregators();
            initThreads();
            this.writers.Add(ServiceType.Gui, guiMessageWriter);
            guiMessageReader = messageReader; //Set this so that we can communicate
            
        }

        private void initServices()
        {
            this.battleGenerationService = new BattleGenerationService(this.writers[ServiceType.BattleGeneration], this.messageReader);
            this.battleService = new BattleService(this.writers[ServiceType.Battle], this.messageReader);
            this.enemyService = new EnemyService(this.writers[ServiceType.Enemy], this.messageReader);
            this.playerService = new PlayerService(this.writers[ServiceType.Player], this.messageReader);
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

        private void initAggregators()
        {
            this.battleAggregator = new BattleAggregator();
        }

        private void initThreads()
        {
            //this.battleBackgroundWorker = new BackgroundWorker();
            //this.battleBackgroundWorker.DoWork += battleBackgroundWorker_DoWork;
            //this.battleBackgroundWorker.RunWorkerAsync();
            this.battleThread = new Thread(new ThreadStart(battleService.process));
            this.battleThread.IsBackground = true;
            this.battleThread.Name = "BattleThread";
            this.battleThread.SetApartmentState(ApartmentState.STA);
            this.battleThread.Start();
        }

        void battleBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.battleService.process();
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            //Begin message routing here
            Message message = args.message;

            if (message.conversationId == null) //If it's a new message, give it a conversation id so that it can be tracked
            {
                message.conversationId = Guid.NewGuid();
            }

            if (message is BattleGenerationRequest)
            {
                writers[ServiceType.BattleGeneration].writeMessage(message);
            }
            else if (message is BattleGenerationMessage)
            {
                //Need to get enemy information and player information
                AggregatedBattleInformation aggregatedBattleInformation = new AggregatedBattleInformation();
                aggregatedBattleInformation.conversationId = message.conversationId;
                aggregatedBattleInformation.battleGenerationInfo = (BattleGenerationMessage)message;

                //Add info to aggregator
                battleAggregator.addAggregatedInformation(aggregatedBattleInformation, this.writers[ServiceType.Battle]);

                //Now request info from PlayerService and EnemyService
                CharacterBattleRequest characterBattleRequest = new CharacterBattleRequest();
                characterBattleRequest.conversationId = message.conversationId;
                this.writers[ServiceType.Player].writeMessage(characterBattleRequest);

                this.writers[ServiceType.Enemy].writeMessage(message); //Enemy Service needs info from BattleGenerationMessage to process
            }
            else if (message is CharacterBattleMessage)
            {
                //Add to the aggregator, it will handle writing if it has everything it needs
                battleAggregator.addPlayerCharacterMessageInfo(message.conversationId, (CharacterBattleMessage)message);
            }
            else if (message is EnemyGeneratedMessage)
            {
                //Add to the aggregator, it will handle writing if it has everything it needs
                battleAggregator.addEnemyGenerationMessageInfo(message.conversationId, (EnemyGeneratedMessage)message);
            }
            else if (message is AbilityUsed ||
                message is BattleInitialization ||
                message is BattleUpdate ||
                message is BattleUpdates ||
                message is CombatEnded ||
                message is CommandAvailable ||
                message is DamageDealt ||
                message is HealingDone ||
                message is StatusChange ||
                message is MaxHPChange ||
                message is Dodge ||
                message is Crit)
            {
                //These are simple Gui outputs
                this.writers[ServiceType.Gui].writeMessage(message);
            }
            else if (message is CommandIssued)
            {
                this.writers[ServiceType.Battle].writeMessage(message);
            }
        }
    }
}
