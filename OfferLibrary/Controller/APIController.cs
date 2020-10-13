using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OfferLibrary.Controller
{
    public static class APIController
    {
        /// <summary>
        /// API SUMMARY WEBUPLOADER
        /// </summary>
        public static string ListPath = "list?path=";

        public static string DownloadPath = "download?path=";

        public static string GetSerial = "app/serial";

        /// <summary>
        /// API SUMMARY WEBSERVER
        /// </summary>
        /// 

        public static string OpenApp = "app/open?bundle=";
        public static string CloseApp = "app/kill?bundle=";
        public static string GetApp = "app/installed";
        public static string APMode = "/app/airplane";



        /// <summary>
        /// API RETENTION SUMMARY
        /// </summary>
        public static string WipeApp = "ret/wipe?bundle=";
        public static string BackupApp = "ret/backup?bundle=";


        public static string ChaneInfo = "data/change";

        public static string SendText = "mouse/text";

        /// <summary>
        /// API PROXY 
        /// </summary>




        public static string EndPointWebUploader(string hostname, string api)
        {
            if (String.IsNullOrEmpty(hostname))
            {
                throw new Exception("Chưa Chọn Device");
            }
            return $"http://{hostname}/{api}";
        }
        public static string EndPointWebServer(string hostname, string api) {
            if (String.IsNullOrEmpty(hostname)) {
                throw new Exception("Chưa Chọn Device");
            }
            return $"http://{hostname}:1710/{api}";
        }
        public static string EndPointProxy(string hostname, string ipaddr, string port, string enable) {
            if (String.IsNullOrEmpty(hostname))
            {
                throw new Exception("Chưa Chọn Device");
            }
            return $"http://{hostname}:1710/wifi/socks5?ipaddr={ipaddr}&port={port}&setEnable={enable}";

        }
        public static HttpClient ApiClient { get; set; }
        public static void InitClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


        }


    }
}
