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
                    battleCharacter.abilities = new List<Ability>();
                    battleCharacter.abilities.Add(new Ability() { name= "Sword and Board", manaCost= 4, cooldown = 3000, castTime = 3000 });
                    battleCharacter.abilities.Add(new Ability() { name = "Stagger", manaCost = 6, cooldown = 8000, castTime = 3000});
                    battleCharacter.abilities.Add(new Ability() { name = "Haymaker", manaCost = 8, cooldown = 5000, castTime = 3000 });
                    battleCharacter.name = "Tonin";
                    battleCharacter.level = 1;
                    battleCharacter.maxHp = 1000;
                    battleCharacter.hp = 1000;
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
                    battleCharacter.abilities = new List<Ability>();
                    battleCharacter.abilities.Add(new Ability() { name = "Arcane Bullet", manaCost = 3, cooldown = 3000, castTime = 3000 });
                    battleCharacter.abilities.Add(new Ability() { name = "Flurry", manaCost = 10, cooldown = 10000, castTime = 3000 });
                    battleCharacter.abilities.Add(new Ability() { name = "Enfeeble", manaCost = 5, cooldown = 6000, castTime = 3000 });
                    battleCharacter.characterClass = characterClass;
                    battleCharacter.level = 1;
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
                    battleCharacter.abilities = new List<Ability>();
                    battleCharacter.abilities.Add(new Ability() { name = "Heal", manaCost = 3, castTime = 3000, cooldown = 6000 });
                    battleCharacter.abilities.Add(new Ability() { name = "Prayer", manaCost = 7, castTime = 3000, cooldown = 10000 });
                    battleCharacter.abilities.Add(new Ability() { name = "Smite", manaCost = 5, castTime = 3000, cooldown = 4000 });
                    battleCharacter.characterClass = characterClass;
                    battleCharacter.level = 1;
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
