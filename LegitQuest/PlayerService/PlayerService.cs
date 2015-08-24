using MessageDataStructures;
using MessageDataStructures.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerServiceLibrary
{
    public class PlayerService : BaseService
    {
        public PlayerService(MessageReader messageReader, MessageWriter messageWriter)
            : base(messageReader, messageWriter)
        {
            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            Message message = args.message;
            if (message is CharacterBattleRequest)
            {
                CharacterBattleMessage characterBattleMessage = new CharacterBattleMessage();
                characterBattleMessage.conversationId = message.conversationId;
                characterBattleMessage.characters = new List<BattleCharacter>();
                characterBattleMessage.characters.Add(getBattleCharacter(CharacterClass.Warrior));
                characterBattleMessage.characters.Add(getBattleCharacter(CharacterClass.Mage));
                characterBattleMessage.characters.Add(getBattleCharacter(CharacterClass.Priest));
                this.messageWriter.writeMessage(characterBattleMessage);
            }
        }

        private BattleCharacter getBattleCharacter(CharacterClass characterClass)
        {
            //Just return a default for now
            BattleCharacter battleCharacter = new BattleCharacter();
            battleCharacter.characterClass = characterClass;
            battleCharacter.maxHp = 10;
            battleCharacter.hp = 10;
            battleCharacter.strength = 5;
            battleCharacter.dexterity = 5;
            battleCharacter.vitality = 5;
            battleCharacter.magic = 5;
            battleCharacter.mind = 5;
            battleCharacter.resistance = 5;
            battleCharacter.accuracy = 5;
            battleCharacter.critical = 5;
            battleCharacter.dodge = 5;
            return battleCharacter;
        }
    }
}
