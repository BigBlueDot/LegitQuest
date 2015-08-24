using MediatorServiceLibrary;
using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegitQuest
{
    public class GuiServiceHelper
    {
        MediatorService mediatorService;
        DirectMessageReader messageWriter;
        DirectMessageReader messageReader;

        public GuiServiceHelper()
        {
            mediatorService = new MediatorService(messageWriter, messageReader);
            messageReader.MessageReceived += messageReader_MessageReceived;
        }

        public void useCommand()
        {
            //Oh god something goes here, but what
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            //Process message from the mediator (Will just fire events up to the Battle Display, more likely than not
            throw new NotImplementedException();
        }
    }
}
