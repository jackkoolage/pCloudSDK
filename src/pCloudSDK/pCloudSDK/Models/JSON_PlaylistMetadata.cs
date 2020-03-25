using Newtonsoft.Json;
using System.Collections.Generic;

namespace pCloudSDK.JSON
{
    public class JSON_PlaylistMetadata
    {
        public string name { get; set; }
        public string created { get; set; }
        public bool ismine { get; set; }
        public string type { get; set; }
        public string modified { get; set; }
        public int id { get; set; }
        public bool system { get; set; }

        public int items { get; set; }
        public int page { get; set; }
        public int pagesize { get; set; }
        [JsonProperty("contents")]
        public List<JSON_FileMetadata> Files { get; set; }
    }
}




