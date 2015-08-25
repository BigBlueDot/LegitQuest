using MediatorServiceLibrary;
using MessageDataStructures;
using MessageDataStructures.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegitQuest
{
    public class GuiServiceHelper
    {
        private MediatorService mediatorService;
        private DirectMessageReader messageWriter;
        private DirectMessageReader messageReader;
        public delegate void ReceiveMessageEventHandler(MessageReceivedEventArgs e);
        public event ReceiveMessageEventHandler MessageReceived;

        public GuiServiceHelper()
        {
            this.messageWriter = new DirectMessageReader();
            this.messageReader = new DirectMessageReader();
            mediatorService = new MediatorService(messageReader, messageWriter);
            messageReader.MessageReceived += messageReader_MessageReceived;
        }

        public void startCombat()
        {
            StartCombatMessage startCombatMessage = new StartCombatMessage();
            messageWriter.writeMessage(startCombatMessage);
        }

        public void useCommand()
        {
            //Oh god something goes here, but what
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            //Process message from the mediator (Will just fire events up to the Battle Display, more likely than not
            if (MessageReceived != null)
            {
                MessageReceived(args);
            }
        }
    }
}
