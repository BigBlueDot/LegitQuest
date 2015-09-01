using MessageDataStructures;
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
        private MessageDisplay messageDisplay;
        private GuiServiceHelper guiServiceHelper;
        private AbilityAggregator abilityAggregator;

        public MainWindow()
        {
            InitializeComponent();
            messageDisplay = new MessageDisplay();
            this.abilityAggregator = new AbilityAggregator();
            guiServiceHelper = new GuiServiceHelper();
            guiServiceHelper.MessageReceived += guiServiceHelper_MessageReceived;
            guiServiceHelper.startCombat();
        }

        private void draw()
        {
            this.pnlMain.Children.Clear();
            this.pnlMain.Children.Add(battleDisplay);
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
                this.battleDisplay = new BattleDisplay(battleInitialization.PlayerCharacters, battleInitialization.NonPlayerCharacters);
                this.battleDisplay.characterClicked += battleDisplay_characterClicked;
                this.battleDisplay.enemyClicked += battleDisplay_enemyClicked;
                this.battleDisplay.abilityClicked += battleDisplay_abilityClicked;
                draw();
            }
            else if (message is CommandAvailable)
            {
                CommandAvailable commandAvailable = (CommandAvailable)message;
                battleDisplay.addCommand(commandAvailable.characterId, 0, commandAvailable.commandOne);
                battleDisplay.addCommand(commandAvailable.characterId, 1, commandAvailable.commandTwo);
                battleDisplay.addCommand(commandAvailable.characterId, 2, commandAvailable.commandThree);
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
                this.guiServiceHelper.useCommand(commandIssued);
            }
        }

        void battleDisplay_characterClicked(EventInfo.CharacterSelectedEventArgs args)
        {
            //Nothing for now
        }
    }
}
