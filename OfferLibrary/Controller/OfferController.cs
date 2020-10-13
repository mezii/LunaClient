using Dapper;
using OfferLibrary.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfferLibrary.Controller
{
    public class OfferController
    {
        public static List<OfferModel> GetAllOffers() {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString())) {
                var output = cnn.Query<OfferModel>("SELECT * FROM OFFER", new DynamicParameters());
                return output.ToList();
            }
        
        }
        public static OfferModel FindOffer(int offerId) {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString()))
            {
                var output = cnn.QuerySingle<OfferModel>($"SELECT * FROM OFFER WHERE OfferId = {offerId}", new DynamicParameters());
                return output;
            }
        }
        public static void UpdateOffer(OfferModel offer,int offerId)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString()))
            {
                cnn.Execute($"UPDATE Offer SET Name = '{offer.Name}', DeviceId = '{offer.DeviceId}', IsActive = {offer.IsActive}, CTITAppStore = {offer.CTITAppStore},IsRandomCTIT = {offer.IsRandomCTIT},FromRandomCTIT={offer.FromRandomCTIT},ToRandomCTIT = {offer.ToRandomCTIT},TrackingLink='{offer.TrackingLink}',AppName = '{offer.AppName}',InAppCTIT={offer.InAppCTIT},ScriptName='{offer.ScriptName}',IsRandomScript={offer.IsRandomScript} WHERE OfferId = {offerId} ");
                               

            }
        }
        public static void SaveOffer(OfferModel offer) {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString())) {
                cnn.Execute("INSERT INTO OFFER (OfferId,DeviceId,Name,IsActive,CTITAppStore,IsRandomCTIT,FromRandomCTIT,ToRandomCTIT,TrackingLink,AppName,InAppCTIT,ScriptName,IsRandomScript) values (@OfferId,@DeviceId,@Name,@IsActive,@CTITAppStore,@IsRandomCTIT,@FromRandomCTIT,@ToRandomCTIT,@TrackingLink,@AppName,@InAppCTIT,@ScriptName,@IsRandomScript)", offer);
            }
        }

        public static void RemoveOffer(int offerId)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString()))
            {
                cnn.Execute($"DELETE FROM Offer WHERE OfferId = '{offerId}'");
            }
        }
        public static void RemoveAllOffer()
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString()))
            {
                cnn.Execute($"DELETE FROM Offer");
            }
        }   
    }
}
