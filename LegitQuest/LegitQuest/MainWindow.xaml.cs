using MessageDataStructures;
using MessageDataStructures.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LegitQuest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BattleDisplay battleDisplay;
        private ManaDisplay manaDisplay;
        private ManaDisplay affinityManaDisplay;
        private MessageDisplay messageDisplay;
        private GuiServiceHelper guiServiceHelper;
        private AbilityAggregator abilityAggregator;
        private Dictionary<Guid, String> characterMapper;

        public MainWindow()
        {
            InitializeComponent();
            messageDisplay = new MessageDisplay();
            this.abilityAggregator = new AbilityAggregator();
            guiServiceHelper = new GuiServiceHelper();
            this.characterMapper = new Dictionary<Guid, string>();
            guiServiceHelper.MessageReceived += guiServiceHelper_MessageReceived;
            guiServiceHelper.startCombat();
        }

        private void draw()
        {
            this.pnlMain.Children.Clear();
            this.pnlMain.Children.Add(battleDisplay);
            this.pnlMain.Children.Add(manaDisplay);
            if (affinityManaDisplay != null)
            {
                this.pnlMain.Children.Add(affinityManaDisplay);
            }
            this.pnlMain.Children.Add(messageDisplay);
        }

        void guiServiceHelper_MessageReceived(MessageDataStructures.MessageReceivedEventArgs e)
        {
            //Handle all of the messages
            Message message = e.message;
            this.Dispatcher.BeginInvoke((Action)(() => processMessage(message)));
        }

        private void processMessage(Message message)
        {
            if (message is BattleInitialization)
            {
                BattleInitialization battleInitialization = (BattleInitialization)message;
                foreach (MessageDataStructures.ViewModels.Character character in battleInitialization.PlayerCharacters)
                {
                    characterMapper.Add(character.id, character.name);
                }
                foreach (MessageDataStructures.ViewModels.Character character in battleInitialization.NonPlayerCharacters)
                {
                    characterMapper.Add(character.id, character.name);
                }
                this.battleDisplay = new BattleDisplay(battleInitialization.PlayerCharacters, battleInitialization.NonPlayerCharacters);
                this.battleDisplay.characterClicked += battleDisplay_characterClicked;
                this.battleDisplay.enemyClicked += battleDisplay_enemyClicked;
                this.battleDisplay.abilityClicked += battleDisplay_abilityClicked;
                this.manaDisplay = new ManaDisplay(battleInitialization.mana, "");
                if (battleInitialization.affinity != ManaAffinity.None)
                {
                    this.affinityManaDisplay = new ManaDisplay(battleInitialization.affinityMana, battleInitialization.affinity.ToString());
                }
                draw();
            }
            else if (message is CommandAvailable)
            {
                CommandAvailable commandAvailable = (CommandAvailable)message;
                battleDisplay.addCommand(commandAvailable.characterId, 0, commandAvailable.commandOne.name, commandAvailable.commandOneEnabled);
                battleDisplay.addCommand(commandAvailable.characterId, 1, commandAvailable.commandTwo.name, commandAvailable.commandTwoEnabled);
                battleDisplay.addCommand(commandAvailable.characterId, 2, commandAvailable.commandThree.name, commandAvailable.commandThreeEnabled);
            }
            else if (message is AbilityUsed)
            {
                messageDisplay.addMessage(((AbilityUsed)message).message);
            }
            else if (message is DamageDealt)
            {
                DamageDealt specificMessage = (DamageDealt)message;
                int dmg = specificMessage.damage;
                Guid target = specificMessage.target;
                if (characterMapper.ContainsKey(specificMessage.source))
                {
                    messageDisplay.addMessage(characterMapper[specificMessage.source] + " has dealt " + dmg + " to " + characterMapper[specificMessage.target] + "!");
                }
                else
                {
                    messageDisplay.addMessage(characterMapper[specificMessage.target] + " has taken " + dmg + "!");
                }
                
                battleDisplay.modifyHP(specificMessage.target, specificMessage.damage);
            }
            else if (message is CombatEnded)
            {
                messageDisplay.addMessage("Combat has ended");
            }
            else if (message is HealingDone)
            {
                HealingDone healingDone = (HealingDone)message;
                battleDisplay.modifyHP(healingDone.target, -healingDone.healValue);
                messageDisplay.addMessage(characterMapper[healingDone.source] + " has healed " + characterMapper[healingDone.target] + healingDone.healValue + " healing has been done!");
            }
            else if (message is MaxHPChange)
            {
                MaxHPChange maxHPChange = (MaxHPChange)message;
                battleDisplay.modifyMaxHP(maxHPChange.target, maxHPChange.maxHPMod);
                messageDisplay.addMessage(characterMapper[maxHPChange.target] + "'s Max Hit Points has increased by " + maxHPChange.maxHPMod + "!");
            }
            else if (message is Dodge)
            {
                Dodge dodge = (Dodge)message;
                messageDisplay.addMessage(characterMapper[dodge.source] + " has missed " + characterMapper[dodge.target] + "!");
            }
            else if (message is Crit)
            {
                Crit crit = (Crit)message;
                messageDisplay.addMessage(characterMapper[crit.source] + " has critically hit " + characterMapper[crit.target] + "!");
            }
            else if (message is UseMana)
            {
                UseMana useMana = (UseMana)message;
                manaDisplay.modifyMana(-useMana.mana);
                if (useMana.affinity != ManaAffinity.None)
                {
                    affinityManaDisplay.modifyMana(-useMana.affinityMana);
                }
            }
        }

        void battleDisplay_abilityClicked(EventInfo.AbilitySelectedEventArgs e)
        {
            this.abilityAggregator.startAbility(e.commandIssued.source, e.commandIssued.commandNumber);
        }

        void battleDisplay_enemyClicked(EventInfo.CharacterSelectedEventArgs args)
        {
            this.abilityAggregator.addTarget(args.id);
            CommandIssued commandIssued = this.abilityAggregator.getCommand();
            if (commandIssued != null)
            {
                //Disable commands
                battleDisplay.disableAbilities(commandIssued.source);

                //Send gui message
                this.guiServiceHelper.useCommand(commandIssued);
            }
        }

        void battleDisplay_characterClicked(EventInfo.CharacterSelectedEventArgs args)
        {
            //Nothing for now
            this.abilityAggregator.addTarget(args.id);
            CommandIssued commandIssued = this.abilityAggregator.getCommand();
            if (commandIssued != null)
            {
                //Disable commands
                battleDisplay.disableAbilities(commandIssued.source);

                //Send gui message
                this.guiServiceHelper.useCommand(commandIssued);
            }
        }
    }
}
