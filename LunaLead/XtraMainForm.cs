using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OfferLibrary.Controller;
using OfferLibrary.Model;
using DevExpress.Utils.Menu;
using System.IO;
using OfferLibrary;
using Newtonsoft.Json.Linq;
using DevExpress.XtraLayout;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;

namespace LunaLead
{
    public partial class XtraMainForm : DevExpress.XtraEditors.XtraForm
    {

        public XtraMainForm()
        {
            InitializeComponent();
            APIController.InitClient();
            comboBoxSelectHostname.Properties.Items.Add("192.168.31.231");
            LoadSettings();



        }
        private SettingModel GetCurrentSetting(string result)
        {

            SettingModel setting = new SettingModel();
            setting.Name = result.Trim();
            setting.IsBackup = checkBoxBackupPercent.Checked;
            setting.BackupRate = (int)numericBackupPercent.Value;
            setting.IsLimit = checkBoxLimitConversion.Checked;
            setting.LimitRate = (int)numericLimitConversion.Value;
            setting.IsSmartLink = checkBoxSmartLink.Checked;
            setting.SmartLinkRate = (int)numericSmartLinkPercent.Value;
            setting.IsSaveComment = checkBoxComment.Checked;
            setting.Comment = textBoxComment.Text;
            setting.IsXoaIPA = checkBoxFullWipe.Checked;
            setting.IsCheckApp = checkBoxCheckApp.Checked;
            return setting;

        }
        private void LoadCurrentSetting(SettingModel setting)
        {
            checkBoxBackupPercent.Checked = setting.IsBackup;
            numericBackupPercent.Value = setting.BackupRate;
            checkBoxLimitConversion.Checked = setting.IsLimit;
            numericLimitConversion.Value = setting.LimitRate;
            checkBoxSmartLink.Checked = setting.IsSmartLink;
            numericSmartLinkPercent.Value = setting.SmartLinkRate;
            checkBoxComment.Checked = setting.IsSaveComment;
            textBoxComment.Text = setting.Comment;
            checkBoxFullWipe.Checked = setting.IsXoaIPA;
            checkBoxCheckApp.Checked = setting.IsCheckApp;
        }

        private async void LoadSettings() {




            // Load Proxy Default Settings
            comboBoxEditProxyType.Properties.Items.Clear();
            comboBoxEditProxyType.Properties.Items.Add("Microleaves");
            comboBoxEditProxyType.Properties.Items.Add("Vip72");

            try
            {
                List<TextBox> textBoxes = new List<TextBox>();
                textBoxes.Add(textBoxDomainMicro);
                textBoxes.Add(textBoxMembershipMicro);
                textBoxes.Add(textBoxAPITokenMicro);
                await FormHelper.LoadMicroAccount(textBoxes, comboBoxEditProxyPort);
                comboBoxEditProxyPort.SelectedIndex = 0;

                // Load Setting Profile
                FormHelper.LoadSettingProfiles(comboBoxEditSelectSetting);
                // Load Offer
                FormHelper.LoadOffer(gridControlOffers, gridViewOffers);
                //Set Device
                comboBoxSelectHostname.SelectedIndex = 0;





            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
            



        }

  

        private async void btnDeviceConnect_Click(object sender, EventArgs e)
        {

            List<DeviceModel> devices = await RequestController.GetDevicesAsync();
            foreach (DeviceModel device in devices)
            {
                comboBoxSelectHostname.Properties.Items.Add(device.HostName);

            }
            DeviceController.SaveDevices(devices);
        }

        private void btnAddOffer_Click(object sender, EventArgs e)
        {
            try
            {
                OfferConfig offerConf = new OfferConfig(labelDeviceId.Text);
                if (FormHelper.IsGoodToGoOffer(labelDeviceId) == false) return;
                string statusDevice = labelDeviceId.Text;
                if (XtraDialog.Show(offerConf, $"Offer Configuration || { statusDevice}", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    FormHelper.LoadOffer(gridControlOffers, gridViewOffers);

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            

        }



        private void btnOpenScriptFile_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select your path"})
            {
                if (fbd.ShowDialog() == DialogResult.OK) {
                    webBrowserScriptFile.Url = new Uri(fbd.SelectedPath);
                    textBoxScriptFilePath.Text = fbd.SelectedPath;
                    
                }
            }
        }

        private void btnBackScriptFile_Click(object sender, EventArgs e)
        {
            if (webBrowserScriptFile.CanGoBack) {
                webBrowserScriptFile.GoBack();
            }
        }

        private void btnGoScriptFile_Click(object sender, EventArgs e)
        {
            if (webBrowserScriptFile.CanGoForward) {
                webBrowserScriptFile.GoForward();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            webBrowserScriptFile.Url = new Uri(AppDomain.CurrentDomain.BaseDirectory + "Script");
            textBoxScriptFilePath.Text = AppDomain.CurrentDomain.BaseDirectory + "Script";
        }

      

  

        private async void btnHelperOpen_Click(object sender, EventArgs e)
        {
            try
            {
                await RequestController.AppExecution(comboBoxSelectHostname.Text, APIController.OpenApp, comboBoxHelperAppList.Text);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }

        }

        private async void btnHelperGetApp_Click(object sender, EventArgs e)
        {
            statusMainForm.Text = "Get Apps From Device ...";
            await FormHelper.AddAppsToXtraComboBoxEditAsync(comboBoxSelectHostname, comboBoxHelperAppList);
            statusMainForm.Text = "Finish";
        }

        private async void btnHelperClose_Click(object sender, EventArgs e)
        {
            try
            {
                await RequestController.AppExecution(comboBoxSelectHostname.Text, APIController.CloseApp, comboBoxHelperAppList.Text);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private async void btnHelperChange_Click(object sender, EventArgs e)
        {
            try
            {
                await RequestController.AppExecution(comboBoxSelectHostname.Text, APIController.ChaneInfo);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                
                JObject data = await ProxyController.GetMicroleavesAuthorizedIps(textBoxDomainMicro.Text, textBoxMembershipMicro.Text, textBoxAPITokenMicro.Text);
                JArray authorizedips = (JArray)data["data"];
                List<string> authIps = authorizedips.Select(ip => (string)ip).ToList();
                memoEditAuthorizedIps.Text = "";
                authIps.ForEach(authIp => memoEditAuthorizedIps.Text += authIp + Environment.NewLine);

                JObject dataGEO = await ProxyController.GetMicroleavesGEOs(textBoxDomainMicro.Text, textBoxMembershipMicro.Text, textBoxAPITokenMicro.Text);
                JArray geos = (JArray)dataGEO["advanced_geo"];
                List<string> geosIps = geos.Select(geo => (string)geo).ToList();
                memoEditGeoSelection.Text = "";
                geosIps.ForEach(geo => memoEditGeoSelection.Text += geo + Environment.NewLine);


                ProxyController.SaveMicroAccountToSetting(textBoxDomainMicro.Text, textBoxMembershipMicro.Text, textBoxAPITokenMicro.Text);
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Micro Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           

        }

        private async void btnUpdateAllProxyMicro_Click(object sender, EventArgs e)
        {
            try
            {
                await FormHelper.UpdateMicroAuthorizedIP(memoEditAuthorizedIps, textBoxDomainMicro.Text, textBoxMembershipMicro.Text, textBoxAPITokenMicro.Text);
                await FormHelper.UpdateMicroGeoIps(memoEditGeoSelection, textBoxDomainMicro.Text, textBoxMembershipMicro.Text, textBoxAPITokenMicro.Text);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }




        }

        private async void btnUpdateCountryProxyMicro_Click(object sender, EventArgs e)
        {
            try
            {
                await FormHelper.UpdateMicroGeoIps(memoEditGeoSelection, textBoxDomainMicro.Text, textBoxMembershipMicro.Text, textBoxAPITokenMicro.Text);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private async  void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                await FormHelper.UpdateMicroAuthorizedIP(memoEditAuthorizedIps, textBoxDomainMicro.Text, textBoxMembershipMicro.Text, textBoxAPITokenMicro.Text);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void XtraMainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!checkBoxKeyboardHelper.Checked) return;
            try
            {
                await RequestController.AppPostExecution(comboBoxSelectHostname.Text, APIController.SendText,e.KeyChar.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }

        }

        private async void btnHelperWipe_Click(object sender, EventArgs e)
        {
            try
            {
                await RequestController.AppExecution(comboBoxSelectHostname.Text, APIController.WipeApp, comboBoxHelperAppList.Text);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private async void btnHelperBackup_Click(object sender, EventArgs e)
        {
            try
            {
                await RequestController.AppExecution(comboBoxSelectHostname.Text, APIController.BackupApp, comboBoxHelperAppList.Text);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void comboBoxEditProxyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string proxyType = comboBoxEditProxyType.Text;
                if (proxyType == "Microleaves")
                {
                    this.LoadSettings();
                }
                else throw new Exception("Not Update YET");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnChangeIP_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        

        private async void btnApplyProxy_Click(object sender, EventArgs e)
        {
            try
            {
                if (toggleSwitch1.IsOn)
                {
                    string[] proxy = comboBoxEditProxyPort.Text.Split(':');
                    if (proxy.Count() == 0) throw new Exception("Please select Proxy");
                    await RequestController.AppExecution(comboBoxSelectHostname.Text, "", proxy[0], proxy[1], "1");
                }
                else
                {
                    await RequestController.AppExecution(comboBoxSelectHostname.Text, "", "192.168.1.1", "42345", "0");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnChangeIPAirplane_Click(object sender, EventArgs e)
        {
            try
            {
                await RequestController.AppExecution(comboBoxSelectHostname.Text, APIController.APMode,"");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            try
            {

                if (!String.IsNullOrEmpty(comboBoxEditSelectSetting.Text)) {
                    SettingController.UpdateCurrentSetting(GetCurrentSetting(comboBoxEditSelectSetting.Text));
                }
             
                FormHelper.LoadSettingProfiles(comboBoxEditSelectSetting);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }
       

        private void comboBoxEditSelectSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SettingModel currentSetting = SettingController.GetSettingFromName(comboBoxEditSelectSetting.Text);
                LoadCurrentSetting(currentSetting);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditOffer_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormHelper.IsGoodToGoOffer(labelDeviceId) == false) return;

                ColumnView view = gridControlOffers.MainView as ColumnView;
                int[] selectedRows = gridViewOffers.GetSelectedRows();
                string offerId;
                if (selectedRows.Length > 0)
                {
                    offerId = view.GetRowCellDisplayText(selectedRows[0], view.Columns["OfferId"]);
                    OfferConfig offerConf = new OfferConfig(labelDeviceId.Text, int.Parse(offerId));
                    string statusDevice = labelDeviceId.Text;

                    if (XtraDialog.Show(offerConf, $"Offer Configuration || { statusDevice}", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        FormHelper.LoadOffer(gridControlOffers, gridViewOffers);

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            






        }

        private async void comboBoxSelectHostname_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string serial = await RequestController.GetSerial(comboBoxSelectHostname.Text);
                labelDeviceId.Text = serial;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
        }

        private void btnRemoveOffer_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormHelper.IsGoodToGoOffer(labelDeviceId) == false) return;

                ColumnView view = gridControlOffers.MainView as ColumnView;
                int[] selectedRows = gridViewOffers.GetSelectedRows();
                string offerId;
                if (selectedRows.Length > 0)
                {
                    offerId = view.GetRowCellDisplayText(selectedRows[0], view.Columns["OfferId"]);
                    //Delete Offer
                    OfferController.RemoveOffer(int.Parse(offerId));
                    FormHelper.LoadOffer(gridControlOffers, gridViewOffers);


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);

            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormHelper.IsGoodToGoOffer(labelDeviceId) == false) return;
                if (MessageBox.Show("Are you sure to erase all offer data", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    OfferController.RemoveAllOffer();
                }
                
                FormHelper.LoadOffer(gridControlOffers, gridViewOffers);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Question);

            }
        }
    }
}