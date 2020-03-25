using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace pCloudSDK.JSON
{

    #region JSON_Error
    public class JSON_Error
    {
        [JsonProperty("error")]
        public string _ErrorMessage { get; set; }
        [JsonProperty("result")]
        public int _ERRORCODE { get; set; }
    }
    #endregion

    #region JSON_GetCurrentServer
    public class JSON_GetCurrentServer
    {
        public string ip { get; set; }
        public string hostname { get; set; }
        public string ipbin { get; set; }
        public string ipv6 { get; set; }
    }
    #endregion

    #region JSON_IP
    public class JSON_IP
    {
        public string ip { get; set; }
        public string country { get; set; }
    }
    #endregion

    #region JSON_Checksum
    public class JSON_Checksum
    {
        public string md5 { get; set; }
        public string sha1 { get; set; }
    }
    #endregion

    #region ListFolder
    public class JSON_ListFolder
    {
        public int FilesCount { get; set; }
        public int FoldersCount { get; set; }
        public List<JSON_FileMetadata> Files { get; set; }
        public List<JSON_FolderMetadata> Folders { get; set; }
    }
    #endregion

    #region JSON_VideoResolutions
    public class JSON_VideoResolutions
    {
        public int width { get; set; }
        [Browsable(true)] public string path { get; set; }
        public string fps { get; set; }
        public bool isoriginal { get; set; }
        public int height { get; set; }
        public string videocodec { get; set; }
        public int videobitrate { get; set; }
        public int audiobitrate { get; set; }
        public string audiocodec { get; set; }
        public string duration { get; set; }
        [Browsable(true)] public List<string> hosts { get; set; }
        public string Url { get { return $"https://{hosts[0]}{path.Replace(@"\", string.Empty)}"; } }
    }
    #endregion

    #region JSON_PublicMetadata
    public class JSON_PublicMetadata
    {
        public bool isshared { get; set; }
        public string icon { get; set; }
        public string modified { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public int folderid { get; set; }
        public bool ismine { get; set; }
        public bool isfolder { get; set; }
        public string created { get; set; }
        public bool thumb { get; set; }
        public List<JSON_PublicContent> contents { get; set; }
    }

    internal enum FileFolderEnum { File, Dir }
    public class JSON_PublicContent
    {
        [Browsable(false)] public string isfolder { get; set; }
        [Browsable(false)] public long fileid { get; set; }
        [Browsable(false)] public long folderid { get; set; }
        public long parentfolderid { get; set; }
        public long size { get; set; }
        public Utilitiez.FileFolderEnum Type { get { return Convert.ToBoolean(isfolder) ? Utilitiez.FileFolderEnum.folder : Utilitiez.FileFolderEnum.file; } }
        public string name { get; set; }
        public long ID { get { return Type == Utilitiez.FileFolderEnum.folder ? folderid : fileid; } }
    }
    #endregion

    #region JSON_PublicLinkMetadata
    public class JSON_PublicLinkMetadata
    {
        public bool downloads { get; set; }
        public string link { get; set; }
        public string traffic { get; set; }
        public string linkid { get; set; }
        [TypeConverter(typeof(CustomConverter))] public JSON_PublicContent metadata { get; set; }
    }
    #endregion



}
