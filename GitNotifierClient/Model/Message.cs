using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace denolk.GitNotifierClient.Model
{
    public class Message
    {
        public string Author { get; set; }
        public string Date { get; set; }
        public string Repository { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return string.Format("[{0}] pushed on [{1}] at {2}", Author, Repository, Date);
        }

    }
}