using pCloudSDK.JSON;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pCloudSDK
{
    public interface IPlaylists
    {


        /// <summary>
        /// Changes the position of an item in a given colleciton
        /// </summary>
        Task<bool> ChangeFilePosition(long FileID, int OldPosition, int NewPosition);

        /// <summary>
        /// rename playlist
        /// </summary>
        Task<JSON_PlaylistMetadata> Rename(string NewName);

        /// <summary>
        /// list playlists
        /// </summary>
        /// <param name="OffSet">page 1, 2, 3...</param>
        Task<List<JSON_PlaylistMetadata>> List(Utilitiez.PlaylistTypeEnum PlaylistType, Utilitiez.OutputEnum Output, int Limit = 20, int OffSet = 1);

        /// <summary>
        /// deleta all files in the playlist
        /// </summary>
        Task<JSON_PlaylistMetadata> Clear();

        /// <summary>
        /// remove files from playlist
        /// </summary>
        Task<JSON_PlaylistMetadata> Remove(List<long> FileIDs);

        /// <summary>
        /// add files to playlist
        /// </summary>
        Task<Dictionary<long, string>> Add(List<long> FileIDs);

        /// <summary>
        /// get playlist metadata
        /// </summary>
        Task<JSON_PlaylistMetadata> Metadata();

        /// <summary>
        /// Delete a given collection owned by the current user.
        /// System collections could not be deleted. In this case error 2065 (Collection not found.) will be raised.
        /// </summary>
        Task<bool> Delete();

        /// <summary>
        /// Create a new collection for the current user.
        /// Optionally, files could be given for the collection to fill it.
        /// </summary>
        /// <param name="IncludeFiles">files to add to playlist</param>
        Task<Dictionary<long, string>> Create(string PlaylistName, Utilitiez.PlaylistTypeEnum PlaylistType, List<long> IncludeFiles = null);

        Task<JSON_PlaylistMetadata> Create(string PlaylistName, Utilitiez.PlaylistTypeEnum PlaylistType);

        /// <summary>
        /// Generates a public link to a collection, owned by the current user
        /// https://docs.pcloud.com/methods/public_links/getcollectionpublink.html
        /// </summary>
        /// <param name="MaxDownloads">Maximum number of downloads from this folder (even for the same file).</param>
        /// <param name="MaxTrafficInBytes">Maximum traffic that this link will consume (in bytes, started downloads will not be cut to fit in this limit)</param>
        Task<string> Public(int? MaxDownloads = null, int? MaxTrafficInBytes = null);

    }

}