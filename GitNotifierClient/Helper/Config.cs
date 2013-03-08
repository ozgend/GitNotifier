using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitNotifierClient.Helper
{
    internal class Config
    {
        public static string GetServiceLocation() {
            return ConfigurationManager.AppSettings["ServiceLocation"];
        }
    }
}
