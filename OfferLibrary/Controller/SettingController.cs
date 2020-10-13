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
    public class SettingController
    {
        public static void SaveSettingToDatabase(SettingModel setting){
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString()))
            {
                 cnn.Execute("INSERT INTO Setting (Name,IsBackup, BackupRate,IsLimit,LimitRate,IsSmartLink,SmartLinkRate,IsSaveComment,Comment,IsXoaIPA,IsCheckApp) values (@Name,@IsBackup, @BackupRate,@IsLimit,@LimitRate,@IsSmartLink,@SmartLinkRate,@IsSaveComment,@Comment,@IsXoaIPA,@IsCheckApp)", setting);
            }
        }

        public static List<SettingModel> GetSettingFromDatabase() {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString())) 
            {
                var output = cnn.Query<SettingModel>("SELECT * FROM Setting",new DynamicParameters());
                return output.ToList();
                
            }
        }

        public static SettingModel GetSettingFromName(string currentSettingName)
        {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString()))
            {
                var output = cnn.QuerySingle<SettingModel>($"SELECT * FROM Setting WHERE Name = '{currentSettingName}'", new DynamicParameters());
                return output;

            }
        }
        public static void UpdateCurrentSetting(SettingModel setting) {
            using (IDbConnection cnn = new SQLiteConnection(GlobalConfig.ConnectionString())) {
                cnn.Execute($"UPDATE Setting SET IsBackup = {setting.IsBackup}, BackupRate = {setting.BackupRate},IsLimit = {setting.IsLimit}, LimitRate = {setting.LimitRate},IsSmartLink = {setting.IsSmartLink},SmartLinkRate = {setting.SmartLinkRate},IsSaveComment = {setting.IsSaveComment},Comment = '{setting.Comment}',IsXoaIPA = {setting.IsXoaIPA},IsCheckApp={setting.IsCheckApp} WHERE Name = '{setting.Name}'");
            }
            
        }


    }
}
