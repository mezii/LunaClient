using Newtonsoft.Json.Linq;
using OfferLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OfferLibrary.Controller
{
    public class RequestController
        
    {
        public static async Task<JObject> GetApp(string hostname) {

            string url = APIController.EndPointWebServer(hostname, APIController.GetApp);
            using (HttpResponseMessage response = await APIController.ApiClient.GetAsync(url)) {
                if (response.IsSuccessStatusCode)
                {

                    JObject listapp = await response.Content.ReadAsAsync<JObject>();
                    return listapp;
                }
                else {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public static async Task<string> GetSerial(string hostname)
        {

            string url = APIController.EndPointWebServer(hostname, APIController.GetSerial);
            using (HttpResponseMessage response = await APIController.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {

                    string listapp = await response.Content.ReadAsStringAsync();
                    return listapp;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task AppExecution(string hostname, string api, string param1 = "",string param2 = "",string param3 = "") {

            string url = APIController.EndPointWebServer(hostname, api) + param1;
            if (!String.IsNullOrEmpty(param2) && !String.IsNullOrEmpty(param3)) {
                url = APIController.EndPointProxy(hostname, param1, param2, param3);
                
            }
            using (HttpResponseMessage response = await APIController.ApiClient.GetAsync(url)) {
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Status code 200");
                }
                else throw new Exception(response.ReasonPhrase);
            }
                
        }
        public static async Task AppPostExecution(string hostname, string api, string optionalParam = "") {

            /* var payload = new StringContent($"{{\"value\": {optionalParam}}}",Encoding.UTF8,"application/x-www-form-urlencoded");
             using (HttpResponseMessage response = await APIController.ApiClient.PostAsync(url,payload))
             {
                 if (response.IsSuccessStatusCode)
                 {
                     Console.WriteLine("Status code 200");
                 }
                 else throw new Exception(response.ReasonPhrase);
             }*/
            try
            {
                string url = APIController.EndPointWebServer(hostname, api);

                var dict = new Dictionary<string, string>();
                dict.Add("value", optionalParam);
                var client = new HttpClient();
                var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
                var res = await client.SendAsync(req);
            }
            catch (Exception e)
            {

                throw e;
            }
           
        }
       
       
        public static async Task<List<DeviceModel>> GetDevicesAsync()
        {
            List<string> hosts = DeviceController.AllIPsFromCurrentHost();
            List<DeviceModel> iphoneModels = new List<DeviceModel>();

            using (var client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(1000) })
            {
                var requests = hosts.Select(host => client.GetAsync(HostURI(host))).ToList();
                try
                {
                    await Task.WhenAll(requests);
                }
                catch (Exception)
                {

                }


                foreach (var r in requests)
                {
                    try
                    {
                        var task = r.Result;
                        var host = task.RequestMessage.RequestUri.Host;
                        var name = await task.Content.ReadAsStringAsync();
                        DeviceModel device = new DeviceModel();
                        device.HostName = host;
                        device.Serial = name;
                        iphoneModels.Add(device);




                    }
                    catch (Exception)
                    {
                        // DO not need to catch exception here

                    }

                }




            }



            return iphoneModels;




        }
        private static string HostURI(string host)
        {
            return $"http://{host}:1710/app/serial";
        }
    }
}
