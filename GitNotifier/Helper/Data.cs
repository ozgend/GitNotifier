using denolk.GitNotifier.Hubs;
using denolk.GitNotifier.Model;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace denolk.GitNotifier.Helper
{
    internal class Data
    {
        
        public static void SendToClients()
        {
            GitPayload payload = JsonConvert.DeserializeObject<GitPayload>(HttpContext.Current.Request.Form["payload"]);
            GlobalHost.ConnectionManager.GetHubContext<ClientNotificationHub>().Clients.All.Send(payload.ToMessage());
        }
    }
}