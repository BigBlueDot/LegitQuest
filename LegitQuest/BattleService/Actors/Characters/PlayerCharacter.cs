using BattleServiceLibrary.InternalMessage.Abilities;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters
{
    public class PlayerCharacter : Character
    {
        private bool started { get; set; }
        private bool commandSent { get; set; }
        public List<string> abilities { get; set; }

        public PlayerCharacter()
        {
            this.started = false;
        }

        public PlayerCharacter(string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical, List<string> abilities)
        {
            this.name = name;
            this.maxHp = maxHP;
            this.hp = maxHP;
            this.strength = strength;
            this.dexterity = dexterity;
            this.vitality = vitality;
            this.magic = magic;
            this.mind = mind;
            this.resistance = resistance;
            this.accuracy = accuracy;
            this.dodge = dodge;
            this.critical = critical;
            this.abilities = abilities;
            this.started = false;
            this.id = Guid.NewGuid();

            this.addCommandAvailableMessage();
            this.commandSent = true;
        }

        private void addCommandAvailableMessage()
        {
            CommandAvailable commandAvailable = new CommandAvailable();
            commandAvailable.conversationId = Guid.NewGuid();
            commandAvailable.commandOne = abilities[0];
            commandAvailable.commandTwo = abilities[1];
            commandAvailable.commandThree = abilities[2];
            commandAvailable.characterId = this.id;
            this.addOutgoingMessage(commandAvailable);
        }

        private bool canUseCommand()
        {
            return (this.castTimeComplete <= this.currentTime + 1000); //Can cast 1 second ahead of time
        }

        private bool AoEAttackAllowed(CommandIssued command)
        {
            return false; //In the future, will check the ability api or something
        }

        protected override void processMessage(Message message)
        {
            if (message is CommandIssued)
            {
                if (canUseCommand())
                {
                    CommandIssued specificMessage = (CommandIssued)message;
                    
                    //For now we are just assuming it's an attack
                    PhysicalAttack physicalAttack = new PhysicalAttack();
                    physicalAttack.abilityStrength = 15;
                    physicalAttack.attack = this.strength;
                    physicalAttack.target = specificMessage.target;
                    physicalAttack.source = this.id;
                    setCastTime(4000); //4s cast time
                    physicalAttack.executeTime = this.castTimeComplete;
                    physicalAttack.conversationId = specificMessage.conversationId;
                    addOutgoingMessage(physicalAttack);

                    AbilityUsed abilityUsed = new AbilityUsed();
                    abilityUsed.conversationId = specificMessage.conversationId;
                    abilityUsed.message = "An attack has been used!";
                    addOutgoingMessage(abilityUsed);

                    this.commandSent = false;
                }
            }

            base.processMessage(message);
        }

        private void setCastTime(long ms)
        {
            this.castTimeComplete += ms;
        }

        public override void process(long time)
        {
            base.process(time);

            if (canUseCommand() && !commandSent)
            {
                commandSent = true;
                addCommandAvailableMessage();
            }
        }
    }
}
