namespace OfferLibrary.Model
{
    public class SettingModel
    {
        public string Name { get; set; }
        public bool IsBackup { get; set; }
        public int BackupRate { get; set; }

        public bool IsLimit { get; set; }

        public int LimitRate { get; set; }
        public bool IsSmartLink { get; set; }
        public int SmartLinkRate { get; set; }

        public bool IsSaveComment { get; set; }
        public string Comment { get; set; }
        public bool IsXoaIPA { get; set; }
        public  bool IsCheckApp { get; set; }

     

    }
}