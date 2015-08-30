using BattleServiceLibrary.Actors;
using BattleServiceLibrary.Actors.Characters;
using MessageDataStructures;
using MessageDataStructures.Battle;
using MessageDataStructures.EnemyGeneration;
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

        public BattleService(MessageReader messageReader, MessageWriter messageWriter) : base(messageReader, messageWriter)
        {
            battles = new Dictionary<Guid, Battle>();
            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            this.processMessage(args.message);
        }

        private void processMessage(Message message)
        {
            if (message is AggregatedBattleInformation)
            {
                AggregatedBattleInformation aggregateBattleInformation = (AggregatedBattleInformation)message;
                
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

        }

        private void createBattle(List<Enemy> enemies, List<Character> characters)
        {
            //There will be more to this later
            Actor pointCharacter = getPlayerCharacter(characters[0]);
            Actor leftWingCharacter = getPlayerCharacter(characters[1]);
            Actor rightWingCharacter = getPlayerCharacter(characters[2]);
            Actor pointEnemy = getRandomNonPlayerCharacter(enemies[0]);
            Actor leftWingEnemy = getRandomNonPlayerCharacter(enemies[1]);
            Actor rightWingEnemy = getRandomNonPlayerCharacter(enemies[2]);
            Battle battle = new Battle(new Guid(), pointCharacter, leftWingCharacter, rightWingCharacter, pointEnemy, leftWingEnemy, rightWingEnemy);
            battles.Add(battle.id, battle);
        }

        private PlayerCharacter getPlayerCharacter(Character character)
        {
            return new PlayerCharacter(character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical);
        }

        private RandomNonPlayerCharacter getRandomNonPlayerCharacter(Enemy character)
        {
            return new RandomNonPlayerCharacter(character.maxHp, character.strength, character.dexterity, character.vitality, character.magic, character.mind, character.resistance, character.accuracy, character.dodge, character.critical);
        }

        public void writeMessage(MessageDataStructures.Message message)
        {
            this.messageWriter.writeMessage(message);
        }
    }
}
