using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace LegitQuestWebApp
{
    public class BattleHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
            Clients.All.broadcastMessage("something", "Something");
        }
    }
}