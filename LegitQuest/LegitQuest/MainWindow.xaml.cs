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

        public MainWindow()
        {
            InitializeComponent();
            messageDisplay = new MessageDisplay();
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
            if (message is BattleInitialization)
            {
                BattleInitialization battleInitialization = (BattleInitialization)message;
                this.battleDisplay = new BattleDisplay(battleInitialization.PlayerCharacters, battleInitialization.NonPlayerCharacters);
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
    }
}
