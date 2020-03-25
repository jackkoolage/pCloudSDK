namespace pCloudSDK.JSON
{
    public class JSON_UserInfo
    {
        public enum PlanEnum { free = 0 }

        public bool cryptosetup { get; set; }
        public PlanEnum plan { get; set; }
        public bool cryptosubscription { get; set; }
        public long publiclinkquota { get; set; }
        public string email { get; set; }
        public bool emailverified { get; set; }
        public int trashrevretentiondays { get; set; }
        public long userid { get; set; }
        public bool agreedwithpp { get; set; }
        public long quota { get; set; }
        public bool haspassword { get; set; }
        public bool premium { get; set; }
        public bool premiumlifetime { get; set; }
        public bool cryptolifetime { get; set; }
        public long usedquota { get; set; }
        public string language { get; set; }
        public bool business { get; set; }
        public long freequota { get; set; }
        public string registered { get; set; }
        public JSON_Journey journey { get; set; }
    }
    public class JSON_Journey
    {
        public JSON_Steps steps { get; set; }
    }
    public class JSON_Steps
    {
        public bool verifymail { get; set; }
        public bool uploadfile { get; set; }
        public bool autoupload { get; set; }
        public bool downloadapp { get; set; }
        public bool downloaddrive { get; set; }
        public bool sentinvitation { get; set; }
    }

}
