using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace pCloudSDK.JSON 
{
    public class JSON_FolderMetadata
    {
        [JsonProperty("comments")]public int CommentsCount { get; set; }
        public string name { get; set; }
        public string created { get; set; }
        public bool ismine { get; set; }
        public bool thumb { get; set; }
        public string modified { get; set; }
        public bool isshared { get; set; }
        public string icon { get; set; }
        public bool isfolder { get; set; }
        public string parentfolderid { get; set; }
        public long folderid { get; set; }
        // <JsonProperty("contents")> Public Property SubFolders As List(Of JSON_FolderMetadata) = New List(Of JSON_FolderMetadata) ''list tree

        [Browsable(false)][Bindable(false)][DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)][EditorBrowsable(EditorBrowsableState.Never)]
        [JsonProperty("contents")]
        public object SubFolders { get; set; }

        public List<JSON_FileMetadata> Files
        {
            get
            {
                return (from f in Newtonsoft.Json.Linq.JArray.Parse(this.SubFolders.ToString()).ToArray().Where(f=> Convert.ToBoolean(f.SelectToken("isfolder").ToString()) == false)
                        select JsonConvert.DeserializeObject<JSON_FileMetadata>(f.ToString(), Basic.JSONhandler)).ToList(); // (From x In Linq.JObject.Parse(SubFolders)("contents").ToList Where x.Value(Of Boolean)("isfolder") = False Select JsonConvert.DeserializeObject(Of JSON_FileMetadata)(x.ToString, JSONhandler)).ToList
            }
        }
        public List<JSON_FolderMetadata> Folders
        {
            get
            {
                return (from x in Newtonsoft.Json.Linq.JArray.Parse(SubFolders.ToString()).Where(x => Convert.ToBoolean(x.SelectToken("isfolder").ToString()) == true) 
                        select JsonConvert.DeserializeObject<JSON_FolderMetadata>(x.ToString(), Basic.JSONhandler)).ToList();
            }
        }
    }

}
