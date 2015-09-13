using BattleServiceLibrary.InternalMessage.Abilities;
using BattleServiceLibrary.Utility;
using MessageDataStructures;
using MessageDataStructures.Battle;
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
        public List<long> abilityAvailable { get; set; }
        public List<bool> onCooldown { get; set; }

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
            this.abilityAvailable = new List<long>();
            this.abilityAvailable.Add(0);
            this.abilityAvailable.Add(0);
            this.abilityAvailable.Add(0);
            this.onCooldown = new List<bool>();
            this.onCooldown.Add(false);
            this.onCooldown.Add(false);
            this.onCooldown.Add(false);
        }

        private void addCommandAvailableMessage()
        {
            CommandAvailable commandAvailable = new CommandAvailable();
            commandAvailable.conversationId = Guid.NewGuid();
            commandAvailable.commandOne = abilities[0];
            commandAvailable.commandOneEnabled = (this.manaStore.mana > abilities[0].manaCost && currentTime >= abilityAvailable[0]);
            commandAvailable.commandTwo = abilities[1];
            commandAvailable.commandTwoEnabled = (this.manaStore.mana > abilities[1].manaCost && currentTime >= abilityAvailable[1]);
            commandAvailable.commandThree = abilities[2];
            commandAvailable.commandThreeEnabled = (this.manaStore.mana > abilities[2].manaCost && currentTime >= abilityAvailable[2]);
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
                this.castTimeComplete = currentTime + ms;
            }
            else
            {
                this.castTimeComplete += ms;
            }
        }

        protected void setCooldown(long ms, int index)
        {
            if (currentTime > this.castTimeComplete)
            {
                this.abilityAvailable[index] = currentTime + ms;
                this.onCooldown[index] = true;
            }
            else
            {
                this.abilityAvailable[index] = this.castTimeComplete + ms;
                this.onCooldown[index] = true;
            }
        }

        public override void process(long time)
        {
            base.process(time);

            if (canUseCommand() && cooldownComplete(time))
            {
                commandSent = true;
                addCommandAvailableMessage();
            }

            if (canUseCommand() && !commandSent)
            {
                commandSent = true;
                addCommandAvailableMessage();
            }
        }

        private bool cooldownComplete(long time)
        {
            //Check all cooldowns and see if one has come off cooldown
            bool offCooldown = false;

            for (int i = 0; i < 3; i++)
            {
                if (abilityAvailable[i] < this.currentTime && onCooldown[i])
                {
                    offCooldown = true;
                    onCooldown[i] = false;
                }
            }

            return offCooldown;
        }

        protected bool hasMana(int manaCost, ManaAffinity affinity)
        {
            if (affinity == manaStore.affinity)
            {
                return (manaStore.mana + manaStore.affinityMana >= manaCost);
            }
            else
            {
                return (manaStore.mana >= manaCost);
            }
        }

        protected UseMana useMana(int manaCost, ManaAffinity affinity)
        {
            UseMana useMana = new UseMana();

            if (manaStore.affinity == affinity)
            {
                useMana.affinity = affinity;
                if (manaCost > manaStore.affinityMana)
                {
                    useMana.affinityMana = manaStore.affinityMana;
                    manaCost -= manaStore.affinityMana;
                    manaStore.affinityMana = 0;
                    manaStore.mana -= manaCost;
                    useMana.mana = manaCost;
                }
                else
                {
                    manaStore.affinityMana -= manaCost;
                    useMana.affinityMana = manaCost;
                    useMana.mana = 0;
                }
            }
            else
            {
                manaStore.mana -= manaCost;
                useMana.affinity = ManaAffinity.None;
                useMana.affinityMana = 0;
                useMana.mana = manaCost;
            }

            return useMana;
        }
    }
}
