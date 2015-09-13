using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.Utility;
using MessageDataStructures;
using MessageDataStructures.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary.Actors.Characters
{
    public class PlayerCharacter : Character
    {
        protected bool started { get; set; }
        protected bool commandSent { get; set; }
        public List<Ability> abilities { get; set; }
        private ManaStore _manaStore;
        public ManaStore manaStore
        {
            get
            {
                return _manaStore;
            }
            set
            {
                this._manaStore = value;
                this.addCommandAvailableMessage();
                this.commandSent = true;
            }
        }

        public PlayerCharacter()
        {
            this.started = false;
        }

        public PlayerCharacter(int level, string name, int maxHP, int strength, int dexterity, int vitality, int magic, int mind, int resistance, int accuracy, int dodge, int critical, List<Ability> abilities)
        {
            this.level = level;
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
        }

        private void addCommandAvailableMessage()
        {
            CommandAvailable commandAvailable = new CommandAvailable();
            commandAvailable.conversationId = Guid.NewGuid();
            commandAvailable.commandOne = abilities[0];
            commandAvailable.commandOneEnabled = (this.manaStore.mana > abilities[0].manaCost);
            commandAvailable.commandTwo = abilities[1];
            commandAvailable.commandTwoEnabled = (this.manaStore.mana > abilities[1].manaCost);
            commandAvailable.commandThree = abilities[2];
            commandAvailable.commandThreeEnabled = (this.manaStore.mana > abilities[2].manaCost);
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
                    useCommand(specificMessage);
                }
            }

            base.processMessage(message);
        }

        protected virtual void useCommand(CommandIssued commandIssued)
        {
            //For now we are just assuming it's an attack
            PhysicalAttack physicalAttack = new PhysicalAttack();
            physicalAttack.abilityStrength = 15;
            physicalAttack.attack = this.strength;
            physicalAttack.target = commandIssued.target;
            physicalAttack.source = this.id;
            physicalAttack.accuracy = this.accuracy;
            physicalAttack.crit = this.critical;
            setCastTime(4000); //4s cast time
            physicalAttack.executeTime = this.castTimeComplete;
            physicalAttack.conversationId = commandIssued.conversationId;
            addOutgoingMessage(physicalAttack);

            AbilityUsed abilityUsed = new AbilityUsed();
            abilityUsed.conversationId = commandIssued.conversationId;
            abilityUsed.message = "An attack has been used!";
            addOutgoingMessage(abilityUsed);

            this.commandSent = false;
        }

        protected void setCastTime(long ms)
        {
            if (currentTime > this.castTimeComplete)
            {
                this.castTimeComplete += currentTime + ms;
            }
            else
            {
                this.castTimeComplete += ms;
            }
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

        protected bool hasMana(int manaCost)
        {
            return (manaStore.mana >= manaCost);
        }

        protected void useMana(int manaCost)
        {
            manaCost -= manaCost;
        }
    }
}
