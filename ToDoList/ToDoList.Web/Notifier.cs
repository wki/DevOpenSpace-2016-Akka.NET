using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ToDoList.Web
{
    [HubName("notifier")]
    public class Notifier: Hub
    {
        public Notifier()
        {
            // this.Clients.Group("name");
            System.Diagnostics.Trace.WriteLine("notifier starting");
        }

        // hier sind nur Methoden, die von JS aus aufgerufen werden.
        public void DoSomething()
        {

        }
    }
}
