using Newtonsoft.Json.Linq;
using OfferLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfferLibrary.Controller
{
    public class ProxyController
    {
        public static async Task<JObject> GetMicroleavesProxies(string APIDomain, string Membership, string Token) {
            string url = MicroProxyConnector.APILinkBuilder(APIDomain, Membership, Token, MicroProxyConnector.ProxiesEndPoint);
            using (HttpResponseMessage response = await APIController.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    JObject data = await response.Content.ReadAsAsync<JObject>();
                    return data;
                }
                else throw new Exception(response.ReasonPhrase);
            }

        }
        public static async Task<JObject> GetMicroleavesAuthorizedIps(string APIDomain, string Membership, string Token)
        {

            string url = MicroProxyConnector.APILinkBuilder(APIDomain, Membership, Token, MicroProxyConnector.AuthorizedIpsEndPoint);
            using (HttpResponseMessage response = await APIController.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    JObject data = await response.Content.ReadAsAsync<JObject>();
                    return data;
                }
                else throw new Exception(response.ReasonPhrase);
            }


        }
        public static async Task<JObject> GetMicroleavesGEOs(string APIDomain, string Membership, string Token)
        {

            string url = MicroProxyConnector.APILinkBuilder(APIDomain, Membership, Token, MicroProxyConnector.GeosEndPoint);
            using (HttpResponseMessage response = await APIController.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    JObject data = await response.Content.ReadAsAsync<JObject>();
                    return data;
                }
                else throw new Exception(response.ReasonPhrase);
            }


        }

        public static async Task PutMicroleavesAuthorizedIps(string APIDomain, string Membership, string Token,string ips)
        {
            string url = MicroProxyConnector.APILinkBuilder(APIDomain, Membership, Token, MicroProxyConnector.AuthorizedIpsEndPoint);

            var payload = new StringContent($"{{\"ips\": [{ips}]}}", Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await APIController.ApiClient.PutAsync(url, payload))
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Success Update Authorized IPs", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else throw new Exception(response.ReasonPhrase);

            }


        }

        public static async Task PutMicroleavesGEOsIps(string APIDomain, string Membership, string Token, string geos)
        {
            string url = MicroProxyConnector.APILinkBuilder(APIDomain, Membership, Token, MicroProxyConnector.GeosEndPoint);
            //geo": [ "us", "gb" ],
            var payload = new StringContent($"{{\"geo\": [\"us\"],\"advanced_geo\": [{geos}]}}", Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await APIController.ApiClient.PutAsync(url, payload))
            {
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Success Update GEOs", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else throw new Exception(response.ReasonPhrase);

            }


        }
        public static void SaveMicroAccountToSetting(string APIDomains, string Membership, string Token) {
            string lines = $"{APIDomains}|{Membership}|{Token}";
            File.WriteAllText(GlobalConfig.SettingPath + "\\MicroAccount.txt", lines);
        }
    }
}
