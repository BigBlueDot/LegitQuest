using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleService
{
    public class BattleService
    {
        Dictionary<Guid, Battle> battles;
        List<MessageDataStructures.Message> outboundMessages;
        List<MessageDataStructures.Message> inboundMessages;

        public BattleService()
        {
            battles = new Dictionary<Guid, Battle>();
            outboundMessages = new List<MessageDataStructures.Message>();
            inboundMessages = new List<MessageDataStructures.Message>();
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
            this.inboundMessages.Add(message);
        }
    }
}
