using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json.Linq;
using OfferLibrary.Controller;
using OfferLibrary.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfferLibrary
{
    public class FormHelper
    {
        public static async Task AddAppsToXtraComboBoxEditAsync(ComboBoxEdit host, ComboBoxEdit cbe) {

            string hostname = host.Text;
            cbe.Properties.Items.Clear();
            if (String.IsNullOrEmpty(hostname))
            {
                XtraMessageBox.Show("Chưa Chọn Device Để Lấy Apps", "Connection Lost ?", MessageBoxButtons.OK);
                return;
            }
            try
            {
                JObject listapp = await RequestController.GetApp(hostname);
                List<string> apps = listapp.Properties().Select(p => p.Name).ToList();
                apps.ForEach(app => cbe.Properties.Items.Add(app));

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Cannot Connect To Current Device IP Address {host.Text} \n Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private static string ConvertMemoEditDataToStringJSON(MemoEdit memoEditAuthorizedIps) {
            string ips = "";
            string[] memoLines = memoEditAuthorizedIps.Lines;
            string[] lines = memoLines.Take(memoLines.Count() - 1).ToArray();
            if (lines.Count() <= 2)
            {
                throw new Exception("Phải Connect Micro Account Hoặc Update Thông Tin GEOs IPs và Authorized IPs trước khi thay đổi thông tin Microleaves");
            }
            foreach (string line in lines)
            {
                string ip = "\"" + line + "\"" + ",";
                ips += ip;
            }
            return ips.Remove(ips.Length - 1);

        }
        public static async Task UpdateMicroAuthorizedIP(MemoEdit memo, string domain, string membership, string token) {
            string lastIPS = ConvertMemoEditDataToStringJSON(memo);
            try
            {
                await ProxyController.PutMicroleavesAuthorizedIps(domain, membership, token, lastIPS);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static async Task UpdateMicroGeoIps(MemoEdit memo, string domain, string membership, string token)
        {
            string lastGEOs = ConvertMemoEditDataToStringJSON(memo);
            try
            {
                await ProxyController.PutMicroleavesGEOsIps(domain, membership, token, lastGEOs);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static bool IsGoodToGoOffer(Label labelDeviceId) {
            bool isAvailableDeviceId = (labelDeviceId.Text == "NO SELECTED DEVICE");
            if (isAvailableDeviceId)
            {
                MessageBox.Show("Device Is NOT CONNECTED", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
            else return true;
        }
        public static void LoadOffer(GridControl control, GridView view) {
            control.DataSource = OfferController.GetAllOffers();
            view.Columns["DeviceId"].Visible = false;
            view.Columns["IsRandomCTIT"].Visible = false;
            view.Columns["FromRandomCTIT"].Visible = false;
            view.Columns["ToRandomCTIT"].Visible = false;
            view.Columns["InAppCTIT"].Visible = false;
            view.BestFitColumns();
        }
        public static void LoadSettingProfiles(ComboBoxEdit comboBoxEditSelectSetting) {
            try
            {
                List<SettingModel> settings = SettingController.GetSettingFromDatabase();
                comboBoxEditSelectSetting.Properties.Items.Clear();
                settings.ForEach(setting => comboBoxEditSelectSetting.Properties.Items.Add(setting.Name));
                // comboBoxEditSelectSetting.SelectedIndex = 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static async Task LoadMicroAccount(List<TextBox> listMicroTextbox, ComboBoxEdit comboBoxEditProxyPort)
        {
            try
            {
                string[] mAccount = File.ReadAllLines(GlobalConfig.SettingPath + "\\MicroAccount.txt", Encoding.UTF8)[0].Split('|');
                listMicroTextbox[0].Text = mAccount[0];
                listMicroTextbox[1].Text = mAccount[1];
                listMicroTextbox[2].Text = mAccount[2];
                JObject data = await ProxyController.GetMicroleavesProxies(mAccount[0], mAccount[1], mAccount[2]);
                JArray proxies = (JArray)data["data"];
                List<string> proxyList = proxies.Select(ip => (string)ip).ToList();
                comboBoxEditProxyPort.Properties.Items.Clear();
                proxyList.ForEach(proxy => comboBoxEditProxyPort.Properties.Items.Add(proxy));

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        

    }
}
