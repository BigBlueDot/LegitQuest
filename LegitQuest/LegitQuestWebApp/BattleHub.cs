using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MediatorServiceLibrary;
using MessageDataStructures;
using MessageDataStructures.BattleGeneration;

namespace LegitQuestWebApp
{
    public class BattleHub : Hub
    {
        private MediatorService mediatorService;
        private DirectMessageReader messageWriter;
        private DirectMessageReader messageReader;

        public BattleHub()
        {
            this.messageReader = new DirectMessageReader();
            this.messageWriter = new DirectMessageReader();
            this.mediatorService = new MediatorService(ref messageReader, ref messageWriter);
            this.messageReader.MessageReceived += MessageReader_MessageReceived;
        }

        public void startCombat()
        {
            BattleGenerationRequest battleGenerationRequest = new BattleGenerationRequest();
            messageWriter.writeMessage(battleGenerationRequest);
        }

        public void useCommand(CommandIssued commandIssued)
        {
            messageWriter.writeMessage(commandIssued);
        }

        private void MessageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            this.Clients.All.processMessage(args.message);
        }
    }
}