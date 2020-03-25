using pCloudSDK.JSON;
using System.Collections.Generic;
using System.Threading.Tasks;
using static pCloudSDK.Basic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace pCloudSDK
{
    public class ItemsClient : IItems
    {

        public IFile File(long FileID) => new FileClient(FileID);
        public IFiles Files(List<long> FileIDs) => new FilesClient(FileIDs);
        public IFolder Folder(long FolderID) => new FolderClient(FolderID);
        public IFolders Folders(List<long> FolderIDs) => new FoldersClient(FolderIDs);






        public async Task<JSON_FolderMetadata> ListAll()
        {
            var parameters = new AuthDictionary
            {
                { "folderid", "0" },
                { "recursive", "1" }
            };
            // parameters.Add("nofiles", 1)
            using (HtpClient localHttpClient = new HtpClient(new HCHandler()))
            {
                HttpRequestMessage HtpReqMessage = new HttpRequestMessage(HttpMethod.Post, new pUri("/listfolder"));
                HtpReqMessage.Content = new FormUrlEncodedContent(parameters);
                using (HttpResponseMessage response = await localHttpClient.SendAsync(HtpReqMessage, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                {
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK && JObject.Parse(result).Value<int>("result").Equals(0))
                    {
                        return JsonConvert.DeserializeObject<JSON_FolderMetadata>(JObject.Parse(result).SelectToken ("metadata").ToString(), JSONhandler);
                    }
                    else
                    {
                        ShowError(result);
                        return null;
                    }
                }
            }
        }
    }

}