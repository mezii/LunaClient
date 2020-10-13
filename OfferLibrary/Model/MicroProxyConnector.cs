using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfferLibrary.Model
{
    public class MicroProxyConnector 
    {

        public static string AuthorizedIpsEndPoint = "authorized-ips";
        public static string GeosEndPoint = "geo";
        public static string ProxiesEndPoint = "proxies";
        
        public static string APILinkBuilder(string APIDomain,string Membership,string Token,string Endpoint) {
            if (String.IsNullOrEmpty(APIDomain) || String.IsNullOrEmpty(Membership) || String.IsNullOrEmpty(Token) || String.IsNullOrEmpty(Endpoint)) {
                throw new Exception("Chưa Điền Đầy Đủ Thông Tin Proxies");
            }
            return $"https://{APIDomain}/api/v1/backconnect/{Membership}/{Endpoint}?api_token={Token}";
        }

       
       


    }
}
