using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static pCloudSDK.Basic;
using static pCloudSDK.Utilitiez;

namespace pCloudSDK
{
    public class PlaylistsClient : IPlaylists// 'Collection
    {
        private long? PlaylistID { get; set; } = default;
        public PlaylistsClient(long PlaylistID)
        {
            this.PlaylistID = PlaylistID;
        }
        public PlaylistsClient()
        {
            PlaylistID = default;
        }


        #region PublicPlaylist
        public async Task<string> Public(int? MaxDownloads = null, int? MaxTrafficInBytes = null)
        {
            var parameters = new AuthDictionary
            {
                { "collectionid", PlaylistID.ToString()??throw new pCloudException("this function requires providing <PlaylistID>, exp: [client.Playlists(12345).Public]",404) },
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

        #region CreatePlaylist
        public async Task<JSON_PlaylistMetadata> Create(string PlaylistName, PlaylistTypeEnum PlaylistType)
        {
            var parameters = new AuthDictionary
            {
                { "name", PlaylistName },
                { "type", Convert.ToString((int)PlaylistType) }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_create"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_PlaylistMetadata>(JObject.Parse(result).SelectToken("collection").ToString(), JSONhandler);
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

        #region CreateNewFolder
        public async Task<Dictionary<long, string>> Create(string PlaylistName, PlaylistTypeEnum PlaylistType, List<long> IncludeFiles = null)
        {
            var parameters = new AuthDictionary();
            if (IncludeFiles != null) { parameters.Add("fileids", string.Join(",", IncludeFiles)); }
            parameters.Add("name", PlaylistName);
            parameters.Add("type", Convert.ToString((int)PlaylistType));
            var encodedContent = new FormUrlEncodedContent(parameters);

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_create"));
                HtpReqMessage.Content = encodedContent;
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        Dictionary<long, string> addationStatus = new Dictionary<long, string>();
                        JObject.Parse(result).SelectToken("linkresult").ToList().ForEach(x => addationStatus.Add(x.Value<long>("fileid"), x.Value<int>("result").Equals(0) ? "Added" : x.Value<string>("message")));
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

        #region DeletePlaylist
        public async Task<bool> Delete()
        {
            var parameters = new AuthDictionary() { { "collectionid", PlaylistID.ToString() ?? throw new pCloudException("this function requires providing <PlaylistID>, exp: [client.Playlists(12345).Delete]", 404) } };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_delete"));
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

        #region Metadata
        public async Task<JSON_PlaylistMetadata> Metadata()
        {
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_details"));
                HtpReqMessage.Content = new FormUrlEncodedContent(new AuthDictionary() { { "collectionid", PlaylistID.ToString() ?? throw new pCloudException("this function requires providing <PlaylistID>, exp: [client.Playlists(12345).Metadata]", 404) } });
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_PlaylistMetadata>(JObject.Parse(result).SelectToken("collection").ToString(), JSONhandler);
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

        #region Add
        public async Task<Dictionary<long, string>> Add(List<long> FileIDs)
        {
            var parameters = new AuthDictionary
            {
                { "fileids", string.Join(",", FileIDs) },
                { "collectionid", PlaylistID.ToString() ?? throw new pCloudException("this function requires providing <PlaylistID>, exp: [client.Playlists(12345).Add]", 404) }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_linkfiles"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        Dictionary<long, string> addationStatus = new Dictionary<long, string>();
                        JObject.Parse(result).SelectToken("linkresult").ToList().ForEach(x => addationStatus.Add(x.Value<long>("fileid"), x.Value<int>("result").Equals(0) ? "Added" : x.Value<string>("message")));
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

        #region Remove
        public async Task<JSON_PlaylistMetadata> Remove(List<long> FileIDs)
        {
            var parameters = new AuthDictionary
            {
                { "fileids", string.Join(",", FileIDs) },
                { "collectionid", PlaylistID.ToString() ?? throw new pCloudException("this function requires providing <PlaylistID>, exp: [client.Playlists(12345).Remove]", 404) }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_unlinkfiles"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_PlaylistMetadata>(JObject.Parse(result).SelectToken("collection").ToString(), JSONhandler);
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

        #region Clear
        public async Task<JSON_PlaylistMetadata> Clear()
        {
            var parameters = new AuthDictionary
            {
                { "all", "1" },
                { "collectionid", PlaylistID.ToString() ?? throw new pCloudException("this function requires providing <PlaylistID>, exp: [client.Playlists(12345).Clear]", 404) }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_unlinkfiles"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_PlaylistMetadata>(JObject.Parse(result).SelectToken("collection").ToString(), JSONhandler);
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

        #region List
        public async Task<List<JSON_PlaylistMetadata>> List(PlaylistTypeEnum PlaylistType, OutputEnum Output, int Limit = 20, int OffSet = 1)
        {
            var parameters = new AuthDictionary();
            parameters.Add("pagesize", Limit.ToString());
            parameters.Add("page", OffSet.ToString());
            if (Output == OutputEnum.PlaylistsWithFiles) { parameters.Add("showfiles", "1"); }

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_list"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<List<JSON_PlaylistMetadata>>(JObject.Parse(result).SelectToken("collections").ToString(), JSONhandler);
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

        #region Rename
        public async Task<JSON_PlaylistMetadata> Rename(string NewName)
        {
            var parameters = new AuthDictionary
            {
                { "name", NewName },
               { "collectionid", PlaylistID.ToString() ?? throw new pCloudException("this function requires providing <PlaylistID>, exp: [client.Playlists(12345).Rename]", 404) }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_rename"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_PlaylistMetadata>(JObject.Parse(result).SelectToken("collection").ToString(), JSONhandler);
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

        #region ChangeFilePosition
        public async Task<bool> ChangeFilePosition(long FileID, int OldPosition, int NewPosition)
        {
            var parameters = new AuthDictionary
            {
                { "fileid", FileID.ToString() },
                { "collectionid", PlaylistID.ToString() ?? throw new pCloudException("this function requires providing <PlaylistID>, exp: [client.Playlists(12345).ChangeFilePosition]", 404) },
                { "item", OldPosition.ToString() },
                { "position", NewPosition.ToString() }
            };

            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/collection_move"));
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


    }

}