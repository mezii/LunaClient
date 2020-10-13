using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfferLibrary
{
    public class GlobalConfig
    {

        public static string ConnectionString(string id = "DeviceManager") {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public static string ScriptPath = AppDomain.CurrentDomain.BaseDirectory + "Script";
        public static string SettingPath = AppDomain.CurrentDomain.BaseDirectory + "Setting";

     
    }
}
