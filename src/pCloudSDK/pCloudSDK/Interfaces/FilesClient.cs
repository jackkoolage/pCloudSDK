using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static pCloudSDK.Basic;
using static pCloudSDK.Utilitiez;

namespace pCloudSDK
{
    public class FilesClient : IFiles
    {


        private List<long> FileIDs { get; set; }
        public FilesClient(List<long> FileIDs)
        {
            this.FileIDs = FileIDs;
        }

        #region CopyFile
        public async Task<bool> Copy(long DestinationFolderID, bool AutoRename)
        {
            var parameters = new AuthDictionary
            {
                { "fileid", string.Join(",", FileIDs) },
                { "tofolderid", DestinationFolderID.ToString() },
                { "noover", AutoRename ? "1" : null }
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
                        return true;
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

        #region DownloadMultipleFilesAsZip
        public async Task DownloadAsZip(string FileSaveDir, string FileName, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default)
        {
            var parameters = new Dictionary<string, string>
            {
                { "username", Username },
                { "password", Password },
                { "fileids", string.Join(",", FileIDs) },
                { "forcedownload", "1" },
                { "filename", FileName }
            };

            ReportCls = ReportCls ?? new Progress<ReportStatus>();
            ReportCls.Report(new ReportStatus() { Finished = false, TextStatus = "Initializing..." });
            try
            {
                System.Net.Http.Handlers.ProgressMessageHandler progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler(new HCHandler());
                progressHandler.HttpReceiveProgress += (sender, e) => { ReportCls.Report(new ReportStatus() { ProgressPercentage = e.ProgressPercentage, BytesTransferred = e.BytesTransferred, TotalBytes = e.TotalBytes ?? 0, TextStatus = "Downloading..." }); };
                // ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                HttpClient localHttpClient = new HtpClient(progressHandler);
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Get, new pUri("/getzip", parameters));
                // ''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
                using (HttpResponseMessage ResPonse = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
                {
                    token.ThrowIfCancellationRequested();
                    if (ResPonse.IsSuccessStatusCode)
                    {
                        ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"[{FileName}] Downloaded successfully." });
                    }
                    else
                    {
                        ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"Error code: {ResPonse.ReasonPhrase}" });
                    }

                    ResPonse.EnsureSuccessStatusCode();
                    var stream_ = await ResPonse.Content.ReadAsStreamAsync();
                    string FPathname = System.IO.Path.Combine(FileSaveDir, FileName);
                    using (var fileStream = new System.IO.FileStream(FPathname, System.IO.FileMode.Append, System.IO.FileAccess.Write))
                    {
                        stream_.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportCls.Report(new ReportStatus() { Finished = true });
                if (ex.Message.ToString().ToLower().Contains("a task was canceled"))
                {
                    ReportCls.Report(new ReportStatus() { TextStatus = ex.Message });
                }
                else
                {
                    throw new pCloudException(ex.Message, 1001);
                }
            }
        }
        #endregion

        #region Compress
        public async Task<string> CompressAsync(long DestinationFolderID, string Filename = null)
        {
            var GeneJobID = RandomString(8);
            var parameters = new AuthDictionary
            {
                { "fileids", string.Join(",", FileIDs) },
                { "tofolderid", DestinationFolderID.ToString() },
                { "toname", Filename },
                { "progresshash", GeneJobID }
            };

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

        #region ThumbnailUrl
        public async Task<Dictionary<long, string>> ThumbnailUrl(string WIDTH_x_HEIGHT, ExtEnum Ext, bool Crop)
        {
            var parameters = new AuthDictionary() { { "fileids", string.Join(",", FileIDs) }, { "size", WIDTH_x_HEIGHT }, { "type", Ext.ToString() } };
            if (Crop) { parameters.Add("crop", "1"); }

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getthumbslinks"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        Dictionary<long, string> addationStatus = new Dictionary<long, string>();
                        JObject.Parse(result).SelectToken("thumbs").ToList().ForEach(x => addationStatus.Add(x.Value<long>("fileid"), x.Value<int>("result").Equals(0) ? string.Format("https://{0}{1}", x.SelectToken("hosts").ToList().First(), x.Value<string>("path")) : x.Value<string>("error")));
                        return addationStatus;
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
