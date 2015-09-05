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
            BattleCharacter battleCharacter = new BattleCharacter();
            switch (characterClass)
            {
                case CharacterClass.Warrior:
                    battleCharacter.characterClass = characterClass;
                    battleCharacter.abilities = new List<string>();
                    battleCharacter.abilities.Add("Sword and Board");
                    battleCharacter.abilities.Add("Stagger");
                    battleCharacter.abilities.Add("Haymaker");
                    battleCharacter.name = "Tonin";
                    battleCharacter.maxHp = 100;
                    battleCharacter.hp = 100;
                    battleCharacter.strength = 10;
                    battleCharacter.dexterity = 5;
                    battleCharacter.vitality = 8;
                    battleCharacter.magic = 2;
                    battleCharacter.mind = 2;
                    battleCharacter.resistance = 2;
                    battleCharacter.accuracy = 7;
                    battleCharacter.critical = 3;
                    battleCharacter.dodge = 6;
                    return battleCharacter;
                case CharacterClass.Mage:
                    battleCharacter.name = "Noktix";
                    battleCharacter.abilities = new List<string>();
                    battleCharacter.abilities.Add("Arcane Bullet");
                    battleCharacter.abilities.Add("Flurry");
                    battleCharacter.abilities.Add("Enfeeble");
                    battleCharacter.characterClass = characterClass;
                    battleCharacter.maxHp = 45;
                    battleCharacter.hp = 45;
                    battleCharacter.strength = 3;
                    battleCharacter.dexterity = 4;
                    battleCharacter.vitality = 5;
                    battleCharacter.magic = 12;
                    battleCharacter.mind = 7;
                    battleCharacter.resistance = 6;
                    battleCharacter.accuracy = 8;
                    battleCharacter.critical = 8;
                    battleCharacter.dodge = 2;
                    return battleCharacter;
                case CharacterClass.Priest:
                    battleCharacter.name = "Cohlm";
                    battleCharacter.abilities = new List<string>();
                    battleCharacter.abilities.Add("Heal");
                    battleCharacter.abilities.Add("Pray");
                    battleCharacter.abilities.Add("Smite");
                    battleCharacter.characterClass = characterClass;
                    battleCharacter.maxHp = 65;
                    battleCharacter.hp = 65;
                    battleCharacter.strength = 8;
                    battleCharacter.dexterity = 2;
                    battleCharacter.vitality = 6;
                    battleCharacter.magic = 7;
                    battleCharacter.mind = 10;
                    battleCharacter.resistance = 9;
                    battleCharacter.accuracy = 5;
                    battleCharacter.critical = 5;
                    battleCharacter.dodge = 5;
                    return battleCharacter;
            }

            return battleCharacter;
        }
    }
}
