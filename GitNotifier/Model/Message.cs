using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace denolk.GitNotifier.Model
{
    public class Message
    {
        public string Author { get; set; }
        public string Date { get; set; }
        public string Repository { get; set; }
        public string Url { get; set; }
    }
}