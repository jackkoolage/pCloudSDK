using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;
using static pCloudSDK.Basic;
using static pCloudSDK.Utilitiez;

namespace pCloudSDK
{
    public class PublicsClient : IPublics
    {

        #region File or Folder Metadata
        public async Task<JSON_PublicMetadata> Metadata(Uri publiclink)
        {
            var parameters = new AuthDictionary { { "code", publiclink.GetParameterInUrl("code") } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getfilepublink"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters.RemoveEmptyValues());
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_PublicMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
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

        #region Single File Download Url
        public async Task<string> SingleFileDirectUrl(Uri publiclink)
        {
            var parameters = new AuthDictionary { { "code", publiclink.GetParameterInUrl("code") }, { "forcedownload", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getfilepublink"));
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

        #region  File In Collocation Download Url
        public async Task<string> FileInFolderDirectUrl(Uri folderpubliclink, long fileid)
        {
            var parameters = new AuthDictionary { { "code", folderpubliclink.GetParameterInUrl("code") }, { "fileid", fileid.ToString() }, { "forcedownload", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getfilepublink"));
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

        #region  Save to My Account 
        public async Task<JSON_FileMetadata> SaveToMyAccount(Uri publiclink, long SaveIntoFolderID)
        {
            var parameters = new AuthDictionary { { "code", publiclink.GetParameterInUrl("code") }, { "tofolderid", SaveIntoFolderID.ToString() } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getfilepublink"));
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

        #region  Save to My Account [Collocation]
        public async Task<string> SaveToMyAccount(Uri folderpubliclink, long fileid, long SaveIntoFolderID)
        {
            var parameters = new AuthDictionary { { "code", folderpubliclink.GetParameterInUrl("code") }, { "fileid", fileid.ToString() }, { "tofolderid", SaveIntoFolderID.ToString() } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getfilepublink"));
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

        #region List Public Links With Metadata
        public async Task<List<JSON_PublicLinkMetadata>> List()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Get, new pUri("/listpublinks", new AuthDictionary()));
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<List<JSON_PublicLinkMetadata>>(JObject.Parse(result).SelectToken("publinks").ToString(), JSONhandler);
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

        #region List Public Links Without Metadata
        public async Task<List<JSON_PublicLinkMetadata>> ListWithoutMetadata()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Get, new pUri("/listplshort", new AuthDictionary()));
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<List<JSON_PublicLinkMetadata>>(JObject.Parse(result).SelectToken("publinks").ToString(), JSONhandler);
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

        #region Download PublicLink As Zip
        public async Task DownloadAsZip(Uri publiclink, string FileSaveDir, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default)
        {
            string FileName = $"pCloudPublicLink_{Utilitiez.RandomString(3)}.zip";
            ReportCls = ReportCls ?? new Progress<ReportStatus>();
            ReportCls.Report(new ReportStatus { Finished = false, TextStatus = "Initializing..." });
            try
            {
                var progressHandler = new ProgressMessageHandler(new HCHandler());
                progressHandler.HttpReceiveProgress += (sender, e) => { ReportCls.Report(new ReportStatus { ProgressPercentage = e.ProgressPercentage, BytesTransferred = e.BytesTransferred, TotalBytes = e.TotalBytes ?? 0, TextStatus = "Downloading..." }); };
                var localHttpClient = new HtpClient(progressHandler);
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Get, new pUri("/getpubzip", new AuthDictionary() { { "code", publiclink.GetParameterInUrl("code") }, { "filename", FileName } }));

                using (HttpResponseMessage ResPonse = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
                {
                    if (ResPonse.IsSuccessStatusCode)
                    {
                        ReportCls.Report(new ReportStatus { Finished = true, TextStatus = $"[{FileName}] Downloaded successfully." });
                    }
                    else
                    {
                        ReportCls.Report(new ReportStatus { Finished = true, TextStatus = $"Error code: {ResPonse.StatusCode}" });
                    }
                    ResPonse.EnsureSuccessStatusCode();
                    Stream stream_ = await ResPonse.Content.ReadAsStreamAsync();
                    var FPathname = Path.Combine(FileSaveDir, FileName);
                    using (FileStream fileStream = new FileStream(FPathname, FileMode.Append, FileAccess.Write))
                    {
                        stream_.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportCls.Report(new ReportStatus { Finished = true });
                if (!ex.Message.ToString().ToLower().Contains("a task was canceled"))
                {
                    throw new pCloudException(ex.Message, 1001);
                }
                ReportCls.Report(new ReportStatus { TextStatus = ex.Message });
            }
        }
        #endregion

        #region DirectZipUrl
        public async Task<string> DirectZipUrl(Uri publiclink, string ZipName)
        {
            var parameters = new AuthDictionary() { { "code", publiclink.GetParameterInUrl("code") }, { "filename", ZipName.ToLower().EndsWith(".zip") ? ZipName : $"{ZipName}.zip" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getpubziplink"));
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

        #region Thumbnail
        public async Task<byte[]> Thumbnail(Uri publiclink, string WIDTH_x_HEIGHT, ExtEnum Ext, bool Crop)
        {
            var parameters = new AuthDictionary() { { "code", publiclink.GetParameterInUrl("code") }, { "size", WIDTH_x_HEIGHT }, { "type", Ext.ToString() }, { "crop", Crop ? "1" : null } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getpubthumb"));
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
        public async Task<string> ThumbnailUrl(Uri publiclink, string WIDTH_x_HEIGHT, ExtEnum Ext, bool Crop)
        {
            var parameters = new AuthDictionary() { { "code", publiclink.GetParameterInUrl("code") }, { "size", WIDTH_x_HEIGHT }, { "type", Ext.ToString() }, { "crop", Crop ? "1" : null } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getpubthumblink"));
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
        public async Task<JSON_FileMetadata> CopyThumbnail(Uri publiclink, long DestinationFolderID, string WIDTH_x_HEIGHT, ExtEnum Ext, bool Crop, bool AutoRename, string RenameTo = null)
        {
            var parameters = new AuthDictionary
            {
                { "code", publiclink.GetParameterInUrl("code")},
                { "tofolderid", DestinationFolderID.ToString() },
                { "noover", AutoRename ? "1" : null },
                { "toname", RenameTo },
                { "type", Ext.ToString() },
                { "size", WIDTH_x_HEIGHT },
                { "crop", Crop ? "1" : null }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/savepubthumb"));
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

        #region Copy PublicLink as Zip to my account
        public async Task<JSON_FileMetadata> CopyZip(Uri publiclink, long DestinationFolderID, string RenameTo = null)
        {
            var parameters = new AuthDictionary { { "code", publiclink.GetParameterInUrl("code") }, { "tofolderid", DestinationFolderID.ToString() } };
            if (!string.IsNullOrEmpty(RenameTo)) { parameters.Add("toname", RenameTo.ToLower().EndsWith(".zip") ? RenameTo : $"{RenameTo}.zip"); }

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/savepubzip"));
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

        #region VideoResolutionUrls
        public async Task<List<JSON_VideoResolutions>> VideoResolutionUrls(Uri publiclink)
        {
            var parameters = new AuthDictionary() { { "code", publiclink.GetParameterInUrl("code") }, { "forcedownload", "1" }, { "skipfilename", "false" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getpubvideolinks"));
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

        #region VideoResolutionUrls [Folder]
        public async Task<List<JSON_VideoResolutions>> VideoResolutionUrls(Uri publicfolderlink, long FileID)
        {
            var parameters = new AuthDictionary() { { "code", publicfolderlink.GetParameterInUrl("code") }, { "fileid", FileID.ToString() }, { "forcedownload", "1" }, { "skipfilename", "false" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getpubvideolinks"));
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
        public async Task<Uri> AudioDirectUrl(Uri publiclink, string AudioBitRate)
        {
            var parameters = new AuthDictionary() { { "code", publiclink.GetParameterInUrl("code") }, { "abitrate", AudioBitRate }, { "forcedownload", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getpubaudiolink"));
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

        #region AudioDirectUrl [Folder]
        public async Task<Uri> AudioDirectUrl(Uri publicfolderlink, long FileID, string AudioBitRate)
        {
            var parameters = new AuthDictionary() { { "code", publicfolderlink.GetParameterInUrl("code") }, { "fileid", FileID.ToString() }, { "abitrate", AudioBitRate }, { "forcedownload", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getpubaudiolink"));
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

    }
}
