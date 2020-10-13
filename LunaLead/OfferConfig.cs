using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OfferLibrary.Model;
using OfferLibrary.Controller;

namespace LunaLead
{
    public partial class OfferConfig : DevExpress.XtraEditors.XtraUserControl
    {
        public OfferConfig(string DeviceId,int OfferId = -1)
        {
            InitializeComponent();
            checkedComboBoxSelectScript.Properties.Items.Add("Test");
            comboBoxEditAppName.Properties.Items.Add("test");
            labelDeviceId.Text = DeviceId;
            if (OfferId >= 1) {
                OfferModel offer = OfferController.FindOffer(OfferId);
                LoadOfferToOfferConfigDialog(offer);
                btnCreateNewConfigOffer.Text = $"Update Offer";
                textBoxOfferId.Text = OfferId.ToString();
                textBoxOfferId.Enabled = false;
            }
        }
        private void LoadOfferToOfferConfigDialog(OfferModel offer) {
            textEditOfferName.Text = offer.Name;
            checkBoxActiveOffer.Checked = offer.IsActive;
            numericUpDownCTITAppStore.Value = offer.CTITAppStore;
            checkBoxRandomCTIT.Checked = offer.IsRandomCTIT;
            numericUpDownFromRandomCTIT.Value = offer.FromRandomCTIT;
            numericUpDownToRandomCTIT.Value = offer.ToRandomCTIT;
            textEditTrackingLink.Text = offer.TrackingLink;
            comboBoxEditAppName.Text = offer.AppName;
            numericUpDownInAppCTIT.Value = offer.InAppCTIT;
            checkedComboBoxSelectScript.Text = offer.ScriptName;
            checkBoxRandomScript.Checked = offer.IsRandomScript;
            textBoxOfferId.Text = offer.OfferId.ToString();

        }

      
        private  OfferModel GenerateNewConfigOffer() {
            OfferModel offer = new OfferModel();
            offer.OfferId = int.Parse(textBoxOfferId.Text);
            offer.Name = textEditOfferName.Text;
            offer.IsActive = checkBoxActiveOffer.Checked;
            offer.CTITAppStore = (int)numericUpDownCTITAppStore.Value;
            offer.IsRandomCTIT = checkBoxRandomCTIT.Checked;
            offer.FromRandomCTIT = (int)numericUpDownFromRandomCTIT.Value;
            offer.ToRandomCTIT = (int)numericUpDownToRandomCTIT.Value;
            offer.TrackingLink = textEditTrackingLink.Text;
            offer.AppName = comboBoxEditAppName.Text;
            offer.InAppCTIT = (int)numericUpDownInAppCTIT.Value;
            offer.ScriptName = checkedComboBoxSelectScript.Text;
            offer.IsRandomScript = checkBoxRandomScript.Checked;
            offer.DeviceId = labelDeviceId.Text.Trim();



            return offer;

        }
        private bool ValidateOffer(OfferModel offer) {
            if (String.IsNullOrEmpty(offer.Name)) return false;
            if (String.IsNullOrEmpty(offer.TrackingLink)) return false;
            if (String.IsNullOrEmpty(offer.AppName)) return false;
            if (String.IsNullOrEmpty(offer.ScriptName)) return false;
            if (String.IsNullOrEmpty(offer.DeviceId)) return false;
            
            return true;

        }

        private void btnCreateNewConfigOffer_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxOfferId.Text))
            {
                MessageBox.Show("Điền OfferId Trên Site Offer18");
                return;
            }
            OfferModel offer = this.GenerateNewConfigOffer();
            bool isAbleCreate = ValidateOffer(offer);
            if (!isAbleCreate) {
                MessageBox.Show("Tạo Offer Cần Điền Đầy Đủ Thông Tin Hoặc Chọn Thiết Bị Đã Cài Tweak");
                return;
            }
            if (btnCreateNewConfigOffer.Text == "Add New Offer")
            {
                //Check new constraint
              
                OfferController.SaveOffer(offer);
                MessageBox.Show($"Add Offer {offer.AppName} Có Tracking {offer.TrackingLink} Thành Công. Chúc Nhiều Lead","Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                OfferController.UpdateOffer(offer, int.Parse(textBoxOfferId.Text));
                MessageBox.Show($"Update Offer {offer.AppName} Có Tracking {offer.TrackingLink} Thành Công. Chúc Nhiều Lead","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }



        }

    }
}
