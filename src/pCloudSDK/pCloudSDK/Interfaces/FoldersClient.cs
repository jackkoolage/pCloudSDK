
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static pCloudSDK.Basic;

namespace pCloudSDK
{
    public class FoldersClient : IFolders
    {
        private List<long> FolderIDs { get; set; }
        public FoldersClient(List<long> FolderIDs)
        {
            this.FolderIDs = FolderIDs;
        }

        #region Compress
        public async Task<string> CompressAsync(long DestinationFolderID, string Filename = null)
        {
            var GeneJobID = Utilitiez.RandomString(8);
            var parameters = new AuthDictionary
            {
                { "folderids", string.Join(",", FolderIDs) },
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

                    if (response.StatusCode == System.Net.HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
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
    }

}