using System;
using System.Collections.Generic;
using System.Text;

namespace pCloudSDK.JSON 
{
    public class JSON_TokenMetadata
    {
        public string device { get; set; } // As JSONlisttokensDevice
        public string tokenid { get; set; }
        public string expiresInactive { get; set; }
        public bool current { get; set; }
        public string expires { get; set; }
        public string created { get; set; }
    }

}
