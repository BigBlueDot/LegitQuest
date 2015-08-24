using MessageDataStructures;
using MessageDataStructures.BattleGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiServiceLibrary
{
    //Handles basic interactions with the Gui
    public class GuiService : BaseService
    {
        public GuiService(MessageReader messageReader, MessageWriter messageWriter)
            : base(messageReader, messageWriter)
        {
            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            //Handle gui messages
            throw new NotImplementedException();
        }

        public void startBattle()
        {
            //Kick off the chain of events that will start a battle
            BattleGenerationRequest battleGenerationRequest = new BattleGenerationRequest();
            this.messageWriter.writeMessage(battleGenerationRequest);
        }
    }
}
