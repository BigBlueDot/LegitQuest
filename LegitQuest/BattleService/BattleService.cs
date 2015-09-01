using BattleServiceLibrary.Actors;
using BattleServiceLibrary.Actors.Characters;
using MessageDataStructures;
using MessageDataStructures.Battle;
using MessageDataStructures.EnemyGeneration;
using MessageDataStructures.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary
{
    public class BattleService : BaseService
    {
        Dictionary<Guid, Battle> battles;
        private List<Message> incomingMessageQueue;

        public BattleService(MessageReader messageReader, MessageWriter messageWriter) : base(messageReader, messageWriter)
        {
            battles = new Dictionary<Guid, Battle>();
            this.incomingMessageQueue = new List<Message>();
            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            this.incomingMessageQueue.Add(args.message);
        }

        private void processMessage(Message message)
        {
            if (message is AggregatedBattleInformation)
            {
                AggregatedBattleInformation aggregateBattleInformation = (AggregatedBattleInformation)message;
                createBattle(aggregateBattleInformation.enemyGenerationInfo.enemies, aggregateBattleInformation.characterBattleInfo.characters, message.conversationId);
            }
            else if (message is CommandIssued)
            {
                //Write to appropriate battle 
                battles[battles.Keys.First()].addInternalMessage(message);
            }
        }

        //This will be called as the thread method
        //In the future we may have battles split into multiple threads
        public void process()
        {
            while (true)
            {
                //This is synchronous for now to avoid possible threading issues with adding battles (not hard to make thread safe,
                //but doesn't exist right now)
                processMessages();
                foreach(Guid key in battles.Keys)
                {
                    battles[key].process();
                    foreach (MessageDataStructures.Message message in battles[key].getOutgoingMessages())
                    {
                        this.messageWriter.writeMessage(message);
                    }
                    battles[key].clearOutgoingMessages();
                }
            }
        }

        private void processMessages()
        {
            List<Message> messages = new List<Message>();
            lock (incomingMessageQueue)
            {
                messages.AddRange(incomingMessageQueue);
                incomingMessageQueue.Clear();
            }
            foreach(Message message in messages)
            {
                processMessage(message);
            }
        }

        private void createBattle(List<Enemy> enemies, List<BattleCharacter> characters, Guid conversationId)
        {
            //There will be more to this later
            PlayerCharacter pointCharacter = getPlayerCharacter(characters[0]);
            PlayerCharacter leftWingCharacter = getPlayerCharacter(characters[1]);
            PlayerCharacter rightWingCharacter = getPlayerCharacter(characters[2]);
            NonPlayerCharacter pointEnemy = getRandomNonPlayerCharacter(enemies[0]);
            NonPlayerCharacter leftWingEnemy = getRandomNonPlayerCharacter(enemies[1]);
            NonPlayerCharacter rightWingEnemy = getRandomNonPlayerCharacter(enemies[2]);
            Battle battle = new Battle(new Guid(), pointCharacter, leftWingCharacter, rightWingCharacter, pointEnemy, leftWingEnemy, rightWingEnemy);
            battles.Add(battle.id, battle);
            
            //Now get initialization
            BattleInitialization battleInitialization = new BattleInitialization();
            battleInitialization.conversationId = conversationId;
            battleInitialization.battleId = battle.id;
            battleInitialization.NonPlayerCharacters = new List<MessageDataStructures.ViewModels.Character>();
            battleInitialization.PlayerCharacters = new List<MessageDataStructures.ViewModels.Character>();
            battleInitialization.NonPlayerCharacters.Add(getCharacterViewModel(pointEnemy, MessageDataStructures.ViewModels.Position.Point));
            battleInitialization.NonPlayerCharacters.Add(getCharacterViewModel(leftWingEnemy, MessageDataStructures.ViewModels.Position.LeftWing));
            battleInitialization.NonPlayerCharacters.Add(getCharacterViewModel(rightWingEnemy, MessageDataStructures.ViewModels.Position.RightWing));
            battleInitialization.PlayerCharacters.Add(getCharacterViewModel(pointCharacter, MessageDataStructures.ViewModels.Position.Point));
            battleInitialization.PlayerCharacters.Add(getCharacterViewModel(leftWingCharacter, MessageDataStructures.ViewModels.Position.LeftWing));
            battleInitialization.PlayerCharacters.Add(getCharacterViewModel(rightWingCharacter, MessageDataStructures.ViewModels.Position.RightWing));

            writeMessage(battleInitialization);
        }

        private PlayerCharacter getPlayerCharacter(BattleCharacter character)
        {
            return new PlayerCharacter(character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical);
        }

        private RandomNonPlayerCharacter getRandomNonPlayerCharacter(Enemy character)
        {
            return new RandomNonPlayerCharacter(character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical);
        }

        private MessageDataStructures.ViewModels.Character getCharacterViewModel(Character character, MessageDataStructures.ViewModels.Position position)
        {
            MessageDataStructures.ViewModels.Character viewModel = new MessageDataStructures.ViewModels.Character();
            viewModel.id = character.id;
            viewModel.hp = character.hp;
            viewModel.maxHp = character.maxHp;
            viewModel.position = position;
            viewModel.weaknesses = new List<string>();
            return viewModel;
        }

        public void writeMessage(MessageDataStructures.Message message)
        {
            this.messageWriter.writeMessage(message);
        }
    }
}
