using Newtonsoft.Json;

namespace pCloudSDK.JSON
{

    public class JSON_FileMetadata
    {
        public string videocodec { get; set; }
        public int videobitrate { get; set; }
        public string fps { get; set; }
        public string duration { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int audiosamplerate { get; set; }
        public int audiobitrate { get; set; }
        public string audiocodec { get; set; }
        public int rotate { get; set; }
        [JsonProperty("comments")]
        public int CommentsCount { get; set; }
        public string name { get; set; }
        public string created { get; set; }
        public bool thumb { get; set; }
        public string modified { get; set; }
        public long size { get; set; }
        public bool isfolder { get; set; }
        public long fileid { get; set; }
        public string hash { get; set; }
        public string path { get; set; }
        public int category { get; set; }
        public bool isshared { get; set; }
        public bool ismine { get; set; }
        public string parentfolderid { get; set; }
        public string contenttype { get; set; }
        public string icon { get; set; }
        // '
        public bool isdeleted { get; set; } // DeleteFile
                                            // '
        public string md5 { get; set; }
        public string sha1 { get; set; }
        // '
        public int position { get; set; } // 'playlist
    }
}
