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
        public bool IsLocal { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            if (IsLocal)
            {
                return string.Format("{0} {1}", Text, Url);
            }
            else
            {
                return string.Format("[{0}] pushed on [{1}] at {2}", Author, Repository, Date);
            }
        }

    }
}