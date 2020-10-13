using Dapper;
using OfferLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OfferLibrary.Controller
{
    public class DeviceController
    {
        public static void SaveDevices(List<DeviceModel> devices) {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString()))
            {
                devices.ForEach(device => cnn.Execute("INSERT INTO Device (HostName, Serial) values (@HostName, @Serial)", device));
            }
        }
        public static List<DeviceModel> GetDevices() {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString())) {
               var output = cnn.Query<DeviceModel>("select * from Device", new DynamicParameters());
                return output.ToList();
            }
        }
        
        private static string GetCurrentHostAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");

        }
        public static List<string> AllIPsFromCurrentHost()
        {
            string host = GetCurrentHostAddress();
            List<string> ips = new List<string>();

            if (host == "")
            {
                return ips;
            }
            string[] arr = host.Split('.');
            string subHost = $"{arr[0]}.{arr[1]}.{arr[2]}.";
            for (int i = 1; i < 256; i++)
            {
                ips.Add($"{subHost}{i.ToString()}");
            }
            return ips;

        }

    }
}
