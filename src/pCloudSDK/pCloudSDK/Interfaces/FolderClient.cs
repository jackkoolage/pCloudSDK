

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pCloudSDK.JSON;
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
    public class FolderClient : IFolder
    {
        private long FolderID { get; set; }
        public FolderClient(long FolderID)
        {
            this.FolderID = FolderID;
        }


        #region PublicFolder
        public async Task<string> Public(int? MaxDownloads = null, int? MaxTrafficInBytes = null)
        {
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "maxdownloads", MaxDownloads.HasValue?MaxDownloads.Value.ToString():null },
                { "maxtraffic", MaxTrafficInBytes.HasValue  ? MaxTrafficInBytes.Value.ToString() : null }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getfolderpublink"));
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

        #region CopyFolder
        public async Task<JSON_FolderMetadata> Copy(string DestinationFolderID, bool NoOverwriting = true)
        {
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "tofolderid", DestinationFolderID },
                { "noover", NoOverwriting ? "1" : null}
            };
            var encodedContent = new FormUrlEncodedContent(parameters.RemoveEmptyValues());

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/copyfolder"));
                HtpReqMessage.Content = encodedContent;
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
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

        #region MoveFolder
        public async Task<JSON_FolderMetadata> Move(string DestinationFolderID, string RenameTo = null)
        {
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "tofolderid", DestinationFolderID },
                { "toname", RenameTo }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/renamefolder"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
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

        #region DeleteFolder
        public async Task<bool> Delete()
        {
            var parameters = new AuthDictionary { { "folderid", FolderID.ToString() } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/deletefolderrecursive"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
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

        #region CreateNewFolder
        public async Task<JSON_FolderMetadata> Create(string FolderName)
        {
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "name", FolderName }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/createfolder"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
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

        #region CreateNewFolderIfNotExists
        public async Task<JSON_FolderMetadata> CreateIfNotExists(string FolderName)
        {
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "name", FolderName }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/createfolderifnotexists"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
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

        #region RenameFolder
        public async Task<bool> Rename(string RenameTo)
        {
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "toname", RenameTo }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/renamefolder"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
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

        #region ListFolder
        public async Task<JSON_ListFolder> List()
        {
            var parameters = new AuthDictionary { { "folderid", FolderID.ToString() } };
            // parameters.Add("nofiles", 1)
            // parameters.Add("noshares", 1)

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/listfolder"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        var finLst = JObject.Parse(result).SelectToken("metadata").SelectToken("contents").ToList();
                        var files = (from x in finLst where x.Value<bool>("isfolder") == false select JsonConvert.DeserializeObject<JSON_FileMetadata>(x.ToString(), JSONhandler)).ToList();
                        var dirs = (from x in finLst where x.Value<bool>("isfolder") == true select JsonConvert.DeserializeObject<JSON_FolderMetadata>(x.ToString(), JSONhandler)).ToList();
                        return new JSON_ListFolder() { Files = files, FilesCount = files.Count, Folders = dirs, FoldersCount = dirs.Count };
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

        #region ListFolderWithoutFiles
        public async Task<JSON_ListFolder> ListWithoutFiles()
        {
            var parameters = new AuthDictionary { { "folderid", FolderID.ToString() }, { "nofiles", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/listfolder"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        var finLst = JObject.Parse(result).SelectToken("metadata").SelectToken("contents").ToList();
                        var files = (from x in finLst where x.Value<bool>("isfolder") == false select JsonConvert.DeserializeObject<JSON_FileMetadata>(x.ToString(), JSONhandler)).ToList();
                        var dirs = (from x in finLst where x.Value<bool>("isfolder") == true select JsonConvert.DeserializeObject<JSON_FolderMetadata>(x.ToString(), JSONhandler)).ToList();
                        return new JSON_ListFolder() { Files = files, FilesCount = files.Count, Folders = dirs, FoldersCount = dirs.Count };
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

        #region ListWithoutShared
        public async Task<JSON_ListFolder> ListWithoutShared()
        {
            var parameters = new AuthDictionary { { "folderid", FolderID.ToString() }, { "noshares", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/listfolder"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        var finLst = JObject.Parse(result).SelectToken("metadata").SelectToken("contents").ToList();
                        var files = (from x in finLst where x.Value<bool>("isfolder") == false select JsonConvert.DeserializeObject<JSON_FileMetadata>(x.ToString(), JSONhandler)).ToList();
                        var dirs = (from x in finLst where x.Value<bool>("isfolder") == true select JsonConvert.DeserializeObject<JSON_FolderMetadata>(x.ToString(), JSONhandler)).ToList();
                        return new JSON_ListFolder() { Files = files, FilesCount = files.Count, Folders = dirs, FoldersCount = dirs.Count };
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

        #region ListSubFoldersTree
        public async Task<List<JSON_FolderMetadata>> ListSubFoldersTree()
        {
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "nofiles", "1" },
                { "recursive", "1" }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/listfolder"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<List<JSON_FolderMetadata>>(JObject.Parse(result).SelectToken("metadata").SelectToken("contents").ToString(), JSONhandler); // Linq.JObject.Parse(result)("metadata").Value(Of List(Of JSON_FolderMetadata))("contents")
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

        #region UploadRemote [Async] 
        public async Task<string> UploadRemoteAsync(string UrlToUP, string Filename = null)
        {
            var GeneJobID = RandomString(8);
            var parameters = new AuthDictionary
            {
                { "url", UrlToUP },
                { "folderid", FolderID.ToString() },
                { "target", Filename },
                { "progresshash", GeneJobID }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/downloadfileasync"));
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

        #region UploadRemoteFile
        public async Task<JSON_FileMetadata> UploadRemote(string UrlToUP, string Filename = null)
        {
            var parameters = new AuthDictionary
            {
                { "url", UrlToUP },
                { "folderid", FolderID.ToString() },
                { "target", Filename }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/downloadfile", parameters), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JObject.Parse(result).Value<JSON_FileMetadata>("metadata");
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

        #region UploadRemoteMultiple [Async] 
        public async Task<bool> UploadRemoteMultipleAsync(List<string> UrlsToUP)
        {
            var parameters = new AuthDictionary
            {
                 { "url", string.Join(" ", UrlsToUP) },
                { "folderid", FolderID.ToString() }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/downloadfileasync"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
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

        #region UploadRemoteMultipleFile
        public async Task<bool> UploadRemoteMultiple(List<string> UrlsToUP)
        {
            var parameters = new AuthDictionary
            {
                { "url", string.Join(" ", UrlsToUP) },
                { "folderid", FolderID.ToString() }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new pUri("/downloadfile", parameters), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
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

        #region UploadLocalFile
        public async Task<List<JSON_FileMetadata>> UploadLocal(object FileToUP, SentType TheUpType, string Filename, bool AutoRename = true, IProgress<ReportStatus> ReportCls = null, CancellationToken token = default)
        {
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "filename", Filename },
                { "nopartial", "1" },
                { "renameifexists", AutoRename ? "1" : null }
            };

            ReportCls = ReportCls ?? new Progress<ReportStatus>();
            ReportCls.Report(new ReportStatus() { Finished = false, TextStatus = "Initializing..." });
            try
            {
                System.Net.Http.Handlers.ProgressMessageHandler progressHandler = new System.Net.Http.Handlers.ProgressMessageHandler(new HCHandler());
                progressHandler.HttpSendProgress += (sender, e) => { ReportCls.Report(new ReportStatus() { ProgressPercentage = e.ProgressPercentage, BytesTransferred = e.BytesTransferred, TotalBytes = e.TotalBytes ?? 0, TextStatus = "Uploading..." }); };
                using (HttpClient localHttpClient = new HtpClient(progressHandler))
                {
                    HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/uploadfile", parameters.RemoveEmptyValues()));
                    // '''''''''''''''''''''''''''''''''
                    HttpContent streamContent = null;
                    switch (TheUpType)
                    {
                        case SentType.filepath:
                            streamContent = new StreamContent(new System.IO.FileStream(FileToUP.ToString(), System.IO.FileMode.Open, System.IO.FileAccess.Read));
                            break;
                        case SentType.memorystream:
                            streamContent = new StreamContent((System.IO.Stream)FileToUP);
                            break;
                        case SentType.bytesArray:
                            streamContent = new StreamContent(new System.IO.MemoryStream((byte[])FileToUP));
                            break;
                    }
                    streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    HtpReqMessage.Content = streamContent;
                    // ''''''''''''''''will write the whole content to H.D WHEN download completed'''''''''''''''''''''''''''''
                    using (HttpResponseMessage ResPonse = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
                    {
                        string result = await ResPonse.Content.ReadAsStringAsync();

                        token.ThrowIfCancellationRequested();
                        if (ResPonse.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                        {
                            ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = "Upload completed successfully" });
                            return JsonConvert.DeserializeObject<List<JSON_FileMetadata>>(JObject.Parse(result).SelectToken("metadata").ToString(), JSONhandler);
                        }
                        else
                        {
                            ReportCls.Report(new ReportStatus() { Finished = true, TextStatus = $"The request returned with HTTP status code {ResPonse.ReasonPhrase}" });
                            ShowError(result);
                            return null;
                        }
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
                return null;
            }
        }
        #endregion

        #region UploadRemoteReportProgress
        public async Task<JSON_JobProgress> UploadRemoteReportProgress(string JobID)
        {
            IClient client = new PClient(Username, Password, ConnectionSetting);
            var TheDomin = (await client.Account().GetCurrentServer()).hostname;

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new Uri(string.Format("https://{0}/uploadprogress", TheDomin) + AsQueryString(new AuthDictionary() { { "progresshash", JobID } })), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_JobProgress>(result);
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

        #region Compress
        public async Task<string> CompressAsync(string SaveIntoFolderID, string Filename = null)
        {
            var GeneJobID = RandomString(8);
            var parameters = new AuthDictionary
            {
                { "folderid", FolderID.ToString() },
                { "tofolderid", SaveIntoFolderID },
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

        #region CompressReportProgress
        public async Task<JSON_JobProgress> CompressTaskProgress(string JobID)
        {
            IClient client = new PClient(Username, Password, ConnectionSetting);
            var TheDomin = (await client.Account().GetCurrentServer()).hostname;

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                using (HttpResponseMessage response = await localHttpClient.GetAsync(new Uri(string.Format("https://{0}/savezipprogress", TheDomin) + AsQueryString(new AuthDictionary() { { "progresshash", JobID } })), HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_JobProgress>(result, JSONhandler);
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

        #region DirectZipUrl
        public async Task<string> DirectZipUrl(string ZipName)
        {
            var parameters = new AuthDictionary() { { "folderid", FolderID.ToString() }, { "forcedownload", "1" }, { "filename", ZipName } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getziplink"));
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

        #region ZipBytesArray
        public async Task<byte[]> ZipBytesArray()
        {
            var parameters = new AuthDictionary() { { "folderid", FolderID.ToString() }, { "forcedownload", "1" } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/getzip", parameters));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                return await localHttpClient.GetByteArrayAsync(new pUri("/getzip", parameters)).ConfigureAwait(false);
            }
        }
        #endregion


    }

}