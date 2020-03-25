using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pCloudSDK
{

    public interface IFolder
    {
        Task<byte[]> ZipBytesArray();

        /// <summary>
        /// Creates and return a public link to a file
        /// https://docs.pcloud.com/methods/public_links/getfilepublink.html
        /// </summary>
        /// <param name="MaxDownloads">Maximum number of downloads for this file</param>
        /// <param name="MaxTrafficInBytes">Maximum traffic that this link will consume (in bytes, started downloads will not be cut to fit in this limit)</param>
        Task<string> Public(int? MaxDownloads = null, int? MaxTrafficInBytes = null);


        /// <summary>
        /// get four hour direct zip url of folder
        /// https://docs.pcloud.com/methods/archiving/getziplink.html
        /// </summary>
        /// <param name="ZipName">zip filename</param>
        Task<string> DirectZipUrl(string ZipName);

        /// <summary>
        /// compress folder and save it as zip file
        /// </summary>
        /// <param name="SaveIntoFolderID">save to folder</param>
        /// <param name="Filename">save as</param>
        /// <returns>job id</returns>
        Task<string> CompressAsync(string SaveIntoFolderID, string Filename = null);

        /// <summary>
        /// for [CompressAsync] Function
        /// </summary>
        Task<JSON_JobProgress> CompressTaskProgress(string JobID);




        Task<List<JSON_FolderMetadata>> ListSubFoldersTree();
        Task<JSON_ListFolder> List();
        Task<JSON_ListFolder> ListWithoutFiles();
        Task<JSON_ListFolder> ListWithoutShared();

        /// <summary>
        /// Creates a folder
        /// https://docs.pcloud.com/methods/folder/createfolder.html
        /// </summary>
        Task<JSON_FolderMetadata> Create( string FolderName);

        /// <summary>
        /// Creates a folder if the folder doesn't exist or returns the existing folder's metadata
        /// https://docs.pcloud.com/methods/folder/createfolderifnotexists.html
        /// </summary>
        Task<JSON_FolderMetadata> CreateIfNotExists( string FolderName);

        /// <summary>
        /// Renames folder
        /// https://docs.pcloud.com/methods/folder/renamefolder.html
        /// </summary>
        Task<bool> Rename(string RenameTo);

        /// <summary>
        /// Copies a folder
        /// https://docs.pcloud.com/methods/folder/copyfolder.html
        /// </summary>
        /// <param name="NoOverwriting">If it is set and files with the same name already exist, no overwriting will be preformed and error 2004 will be returned</param>
        Task<JSON_FolderMetadata> Copy(string DestinationFolderID, bool NoOverwriting = true);

        /// <summary>
        /// Deletes a folder
        /// https://docs.pcloud.com/methods/folder/deletefolder.html
        /// </summary>
        Task<bool> Delete();
        Task<JSON_FolderMetadata> Move(string DestinationFolderID, string RenameTo = null);

        /// <summary>
        /// Downloads one or more files from links suplied in the url parameter (links separated by any amount of whitespace) to the folder identified by either path or folderid (or to the root folder if both are omitted). The response will be recieved when the files are queued for download, not when they are downloaded
        /// https://docs.pcloud.com/methods/file/downloadfileasync.html
        /// </summary>
        /// <returns>JobID</returns>
        Task<string> UploadRemoteAsync(string UrlToUP, string Filename = null);

        /// <summary>
        /// Downloads one or more files from links suplied in the url parameter (links separated by any amount of whitespace) to the folder
        /// https://docs.pcloud.com/methods/file/downloadfile.html
        /// </summary>
        Task<JSON_FileMetadata> UploadRemote(string UrlToUP, string Filename = null);

        Task<bool> UploadRemoteMultipleAsync(List<string> UrlsToUP);
        Task<bool> UploadRemoteMultiple(List<string> UrlsToUP);

        /// <summary>
        /// for [UploadRemoteMultipleAsync] Function
        /// </summary>
        Task<JSON_JobProgress> UploadRemoteReportProgress(string JobID);

        /// <summary>
        /// Upload a file
        /// https://docs.pcloud.com/methods/file/uploadfile.html
        /// </summary>
        Task<List<JSON_FileMetadata>> UploadLocal(object FileToUP, Utilitiez.SentType TheUpType, string Filename, bool AutoRename = true, IProgress<ReportStatus> ReportCls = null, System.Threading.CancellationToken token = default);
    }

}