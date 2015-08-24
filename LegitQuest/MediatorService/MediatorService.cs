using MessageDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorService
{
    public class MediatorService
    {
        private Dictionary<ServiceType, MessageDataStructures.MessageWriter> writers;
        private MessageReader messageReader;

        public MediatorService()
        {
            initWriters();
        }

        private void initWriters()
        {
            writers = new Dictionary<ServiceType, MessageDataStructures.MessageWriter>();
            //TODO:  Add writers here
        }

        private void initReader()
        {
            //TODO:  Add reader (or readers, maybe?  I haven't decided if there will be one queue for the mediator or one per service)f

        }
    }
}
