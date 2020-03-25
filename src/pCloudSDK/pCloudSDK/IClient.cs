namespace pCloudSDK
{
    public   interface IClient
    {

        IAccount Account();
        IItems Items();
        IPlaylists Playlists(long PlaylistID);
        IPlaylists Playlists();
        IPublics Publics();



    }
}
