using denolk.GitNotifier.Helper;
using denolk.GitNotifier.Hubs;
using denolk.GitNotifier.Model;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace denolk.GitNotifier.Controllers
{
    public class HookController : ApiController
    {
        [HttpPost]
        [HttpGet]
        public void Notify()
        {
            bool isFormData = Request.Content.IsFormData();
            if (isFormData)
            {
                Data.SendToClients();
            }
        }

        [HttpPost]
        [HttpGet]
        public object Test()
        {
            return new { Ok = true };
        }

    }
}
