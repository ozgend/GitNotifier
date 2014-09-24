﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using denolk.GitNotifier.Model;

namespace denolk.GitNotifier.Hubs
{
    [HubName("ClientNotificationHub")]
    public class ClientNotificationHub : Hub
    {

        public void Send(Message message)
        {
            Clients.All.send(message);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}