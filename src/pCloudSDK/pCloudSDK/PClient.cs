using System;
using System.Net;
using static pCloudSDK.Basic;

namespace pCloudSDK
{
    public class PClient : IClient
    {

        public PClient(string _username_, string _password_, ConnectionSettings Settings = null)
        {
            ServicePointManager.Expect100Continue = true; ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            Username = _username_;
            Password = _password_;
            ConnectionSetting = Settings;

            if (Settings == null)
            {
                m_proxy = null;
            }
            else
            {
                m_proxy = Settings.Proxy;
                m_CloseConnection = Settings.CloseConnection ?? true;
                m_TimeOut = Settings.TimeOut ?? TimeSpan.FromMinutes(60);
            }
        }


        public IAccount Account() => new AccountClient();
        public IItems Items() => new ItemsClient();
        public IPlaylists Playlists(long PlaylistID) => new PlaylistsClient(PlaylistID);
        public IPlaylists Playlists() => new PlaylistsClient();
        public IPublics Publics() => new PublicsClient();

        public readonly long RootFolderID = 0;


    }
}
