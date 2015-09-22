using BattleServiceLibrary.Actors;
using BattleServiceLibrary.Actors.Characters;
using BattleServiceLibrary.Actors.Characters.Classes;
using BattleServiceLibrary.Actors.Characters.Enemies;
using BattleServiceLibrary.Utility;
using MessageDataStructures;
using MessageDataStructures.Battle;
using MessageDataStructures.EnemyGeneration;
using MessageDataStructures.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                createBattle(aggregateBattleInformation.enemyGenerationInfo.enemies, aggregateBattleInformation.characterBattleInfo.characters, message.conversationId, aggregateBattleInformation.battleGenerationInfo.mana, aggregateBattleInformation.battleGenerationInfo.affinity, aggregateBattleInformation.battleGenerationInfo.affinityMana);
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

                Thread.Sleep(1000);
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

        private void createBattle(List<Enemy> enemies, List<BattleCharacter> characters, Guid conversationId, int mana, ManaAffinity affinity, int affinityMana)
        {
            //There will be more to this later
            PlayerCharacter pointCharacter = getPlayerCharacter(characters[0]);
            PlayerCharacter leftWingCharacter = getPlayerCharacter(characters[1]);
            PlayerCharacter rightWingCharacter = getPlayerCharacter(characters[2]);
            NonPlayerCharacter pointEnemy = getRandomNonPlayerCharacter(enemies[0]);
            NonPlayerCharacter leftWingEnemy = getRandomNonPlayerCharacter(enemies[1]);
            NonPlayerCharacter rightWingEnemy = getRandomNonPlayerCharacter(enemies[2]);
            pointCharacter.engagedWith = pointEnemy.id;
            pointEnemy.engagedWith = pointCharacter.id;
            leftWingCharacter.engagedWith = leftWingEnemy.id;
            leftWingEnemy.engagedWith = leftWingCharacter.id;
            rightWingCharacter.engagedWith = rightWingEnemy.id;
            rightWingEnemy.engagedWith = rightWingCharacter.id;
            Battle battle = new Battle(new Guid(), pointCharacter, leftWingCharacter, rightWingCharacter, pointEnemy, leftWingEnemy, rightWingEnemy, mana, affinity, affinityMana);
            battles.Add(battle.id, battle);
            
            //Now get initialization
            BattleInitialization battleInitialization = new BattleInitialization();
            battleInitialization.conversationId = conversationId;
            battleInitialization.battleId = battle.id;
            battleInitialization.mana = mana;
            battleInitialization.affinity = affinity;
            battleInitialization.affinityMana = affinityMana;
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
            if (character.characterClass == CharacterClass.Warrior)
            {
                return new Warrior(character.level, character.name, character.maxHp, character.strength, character.dexterity, character.vitality, character.maxHp, character.mind, character.resistance, character.accuracy, character.dodge, character.critical, character.abilities);
            }
            else if (character.characterClass == CharacterClass.Mage)
            {
                return new Mage(character.level, character.name, character.maxHp, character.strength, character.dexterity, character.vitality, character.maxHp, character.mind, character.resistance, character.accuracy, character.dodge, character.critical, character.abilities);
            }
            else if (character.characterClass == CharacterClass.Priest)
            {
                return new Priest(character.level, character.name, character.maxHp, character.strength, character.dexterity, character.vitality, character.maxHp, character.mind, character.resistance, character.accuracy, character.dodge, character.critical, character.abilities);
            }
            else
            {
                return new PlayerCharacter(character.level, character.name, character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical, character.abilities);
            }
        }

        private RandomNonPlayerCharacter getRandomNonPlayerCharacter(Enemy character)
        {
            if (character.battleGenerationInfo.enemyType == MessageDataStructures.BattleGeneration.EnemyType.Goblin)
            {
                return new Goblin(character.battleGenerationInfo.level, character.name, character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical);
            }
            else if (character.battleGenerationInfo.enemyType == MessageDataStructures.BattleGeneration.EnemyType.BlueSlime)
            {
                return new BlueSlime(character.battleGenerationInfo.level, character.name, character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical);
            }
            else if (character.battleGenerationInfo.enemyType == MessageDataStructures.BattleGeneration.EnemyType.RedSlime)
            {
                return new RedSlime(character.battleGenerationInfo.level, character.name, character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical);
            }
            return new RandomNonPlayerCharacter(character.battleGenerationInfo.level, character.name, character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical);
        }

        private MessageDataStructures.ViewModels.Character getCharacterViewModel(Character character, MessageDataStructures.ViewModels.Position position)
        {
            MessageDataStructures.ViewModels.Character viewModel = new MessageDataStructures.ViewModels.Character();
            viewModel.id = character.id;
            viewModel.hp = character.hp;
            viewModel.maxHp = character.maxHp;
            viewModel.position = position;
            viewModel.weaknesses = new List<string>();
            viewModel.name = character.name;
            if (character is PlayerCharacter)
            {
                viewModel.abilities = ((PlayerCharacter)character).abilities;
            }

            return viewModel;
        }

        public void writeMessage(MessageDataStructures.Message message)
        {
            this.messageWriter.writeMessage(message);
        }
    }
}
