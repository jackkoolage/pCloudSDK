using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static pCloudSDK.Basic;
using static pCloudSDK.Utilitiez;

namespace pCloudSDK
{
    public class FileClient : IFile
    {


        private long FileID { get; set; }
        public FileClient(long FileID)
        {
            this.FileID = FileID;
        }

        #region PublicFile
        public async Task<string> Public(int? MaxDownloads = null, long? MaxTrafficInBytes = null)
        {
            var parameters = new AuthDictionary
            {
                { "fileid", FileID.ToString() },
                { "maxdownloads", MaxDownloads.HasValue?MaxDownloads.Value.ToString():null },
                { "maxtraffic", MaxTrafficInBytes.HasValue  ? MaxTrafficInBytes.Value.ToString() : null }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getfilepublink"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters.RemoveEmptyValues());
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JObject.Parse(result).SelectToken("link").ToString().Replace(@"\", string.Empty);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region CopyFile
        public async Task<JSON_FileMetadata> Copy(long DestinationFolderID, bool AutoRename, string RenameTo = null)
        {
            var parameters = new AuthDictionary
            {
                { "fileid", FileID.ToString() },
                { "tofolderid", DestinationFolderID.ToString() },
                { "noover", AutoRename ? "1" : null },
                { "toname", RenameTo }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/copyfile"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters.RemoveEmptyValues());
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FileMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region MoveFile
        public async Task<JSON_FileMetadata> Move(long DestinationFolderID, string RenameTo = null)
        {
            var parameters = new AuthDictionary
            {
                { "fileid", FileID.ToString() },
                { "tofolderid", DestinationFolderID.ToString() },
                { "toname", RenameTo }
            };
            var encodedContent = new FormUrlEncodedContent(parameters);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/renamefile"));
                HtpReqMessage.Content = encodedContent;
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FileMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region DeleteFile
        public async Task<bool> Delete()
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/deletefile"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FileMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler).isdeleted;
                    }
                    else
                    {
                        ShowError(result);
                        return false;
                    }
                }
            }
        }
        #endregion

        #region RenameFile
        public async Task<JSON_FileMetadata> Rename(string RenameTo)
        {
            var parameters = new AuthDictionary
            {
                { "fileid", FileID.ToString() },
                { "toname", RenameTo }
            };
            var encodedContent = new FormUrlEncodedContent(parameters);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/renamefile"));
                HtpReqMessage.Content = encodedContent;
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FileMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region ChecksumFile
        public async Task<JSON_Checksum> Checksum()
        {
            var parameters = new AuthDictionary { { "fileid", FileID.ToString() } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/checksumfile"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_Checksum>(result, JSONhandler);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region VideoDirectUrl
        public async Task<string> VideoDirectUrl()
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getvideolink"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return string.Format("https://{0}{1}", JObject.Parse(result).SelectToken("hosts[0]").ToString(), JObject.Parse(result).SelectToken("path").ToString().Replace(@"\", string.Empty));
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region VideoResolutionUrls
        public async Task<List<JSON_VideoResolutions>> VideoResolutionUrls()
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() }, { "forcedownload", "1" }, { "skipfilename", "false" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getvideolinks"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<List<JSON_VideoResolutions>>(JObject.Parse(result).SelectToken("variants").ToString(), JSONhandler);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region AudioDirectUrl
        public async Task<Uri> AudioDirectUrl(int AudioBitRate)
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() }, { "abitrate", AudioBitRate.ToString() }, { "forcedownload", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getaudiolink"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return new Uri(string.Format("https://{0}{1}", JObject.Parse(result).SelectToken("hosts[0]").ToString(), JObject.Parse(result).SelectToken("path").ToString().Replace(@"\", string.Empty)));
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region VideoToMp3
        public async Task<Uri> VideoToMp3(int AudioBitRate)
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() }, { "abitrate", AudioBitRate.ToString() }, { "forcedownload", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getaudiolink"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return new Uri(string.Format("https://{0}{1}", JObject.Parse(result).SelectToken("hosts[0]").ToString(), JObject.Parse(result).SelectToken("path").ToString().Replace(@"\", string.Empty)));
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region DirectUrl
        public async Task<string> DirectUrl()
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getfilelink"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return string.Format("https://{0}{1}", JObject.Parse(result).SelectToken("hosts[0]").ToString(), JObject.Parse(result).SelectToken("path").ToString().Replace(@"\", string.Empty));
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region UnCompress
        public async Task<string> UnCompressAsync(long ExtractIntoFolderID, string ArchivePassword = null, IfExistsEnum IfExists = IfExistsEnum.rename)
        {
            var GeneJobID = RandomString(8);
            var parameters = new AuthDictionary
            {
                { "password", ArchivePassword ?? null },
                { "tofolderid", ExtractIntoFolderID.ToString() },
                { "nooutput", "1" },
                { "overwrite", IfExists.ToString() },
                { "progresshash", GeneJobID }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/extractarchive"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters.RemoveEmptyValues());
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return GeneJobID;
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region UnCompressTaskProgress
        public async Task<bool> UnCompressTaskProgress(string JobID)
        {
            IClient clint = new PClient(Username, Password, ConnectionSetting);
            var TheDomin = (await clint.Account().GetCurrentServer()).hostname;

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new Uri(string.Format("https://{0}/extractarchiveprogress", TheDomin) + AsQueryString(new AuthDictionary() { { "progresshash", JobID } })), System.Net.Http.HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return Convert.ToBoolean(JObject.Parse(result).SelectToken("finished").ToString());
                    }
                    else
                    {
                        ShowError(result);
                        return false;
                    }
                }
            }
        }
        #endregion

        #region Compress
        public async Task<string> CompressAsync(long SaveIntoFolderID, string Filename = null)
        {
            var parameters = new AuthDictionary();
            parameters.Add("fileid", FileID.ToString());
            parameters.Add("tofolderid", SaveIntoFolderID.ToString());
            parameters.Add("toname", Filename);
            var GeneJobID = RandomString(8);
            parameters.Add("progresshash", GeneJobID);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/savezip"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return GeneJobID;
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region ChangesHistory
        public async Task<List<JSON_Entry>> ChangesHistory()
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/getfilehistory", parameters), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<List<JSON_Entry>>(JObject.Parse(result).SelectToken("entries").ToString(), JSONhandler);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region Thumbnail
        public async Task<byte[]> Thumbnail(string WIDTH_x_HEIGHT, ExtEnum Ext, bool Crop)
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() }, { "size", WIDTH_x_HEIGHT }, { "type", Ext.ToString() }, { "crop", Crop ? "1" : null } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getthumb"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters.RemoveEmptyValues());
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        throw new pCloudException(response.ReasonPhrase, (int)response.StatusCode);
                    }
                }
            }
        }
        #endregion

        #region ThumbnailUrl
        public async Task<string> ThumbnailUrl(string WIDTH_x_HEIGHT, ExtEnum Ext, bool Crop)
        {
            var parameters = new AuthDictionary() { { "fileid", FileID.ToString() }, { "size", WIDTH_x_HEIGHT }, { "type", Ext.ToString() }, { "crop", Crop ? "1" : null } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getthumblink"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters.RemoveEmptyValues());
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return string.Format("https://{0}{1}", JObject.Parse(result).SelectToken("hosts[0]").ToString(), JObject.Parse(result).SelectToken("path").ToString().Replace(@"\", string.Empty));
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion

        #region CopyThumbnail
        public async Task<JSON_FileMetadata> CopyThumbnail(long DestinationFolderID, string WIDTH_x_HEIGHT, ExtEnum Ext, bool Crop, bool AutoRename, string RenameTo = null)
        {
            var parameters = new AuthDictionary
            {
                { "fileid", FileID.ToString() },
                { "tofolderid", DestinationFolderID.ToString() },
                { "noover", AutoRename ? "1" : null },
                { "toname", RenameTo },
                { "type", Ext.ToString() },
                { "size", WIDTH_x_HEIGHT },
                { "crop", Crop ? "1" : null }
            };

            var encodedContent = new FormUrlEncodedContent(parameters.RemoveEmptyValues());

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/savethumb"));
                HtpReqMessage.Content = encodedContent;
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FileMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
        #endregion




    }
}
