using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleServiceLibrary
{
    public class BattleService : BaseService
    {
        Dictionary<Guid, Battle> battles;

        public BattleService(MessageReader messageReader, MessageWriter messageWriter) : base(messageReader, messageWriter)
        {
            battles = new Dictionary<Guid, Battle>();
            this.messageReader.MessageReceived += messageReader_MessageReceived;
        }

        void messageReader_MessageReceived(MessageReceivedEventArgs args)
        {
            throw new NotImplementedException();
        }

        //This will be called as the thread method
        //In the future we may have battles split into multiple threads
        public void process()
        {
            while (true)
            {
                //This is synchronous for now to avoid possible threading issues with adding battles (not hard to make thread safe,
                //but doesn't exist right now)
                processMessages();
                foreach(Guid key in battles.Keys)
                {
                    battles[key].process();
                    foreach (MessageDataStructures.Message message in battles[key].getOutgoingMessages())
                    {
                        this.messageWriter.writeMessage(message);
                    }
                    battles[key].clearOutgoingMessages();
                }
            }
        }

        private void processMessages()
        {

        }

        private void createBattle()
        {

        }

        public void writeMessage(MessageDataStructures.Message message)
        {
            this.messageWriter.writeMessage(message);
        }
    }
}
