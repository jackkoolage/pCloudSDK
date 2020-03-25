using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pCloudSDK
{
    public  interface IFiles
    {

        Task<Dictionary<long, string>> ThumbnailUrl(string WIDTH_x_HEIGHT, Utilitiez.ExtEnum Ext, bool Crop);

        /// <summary>
        /// Create a zip file in the user's filesystem
        /// </summary>
        Task<string> CompressAsync(long DestinationFolderID, string Filename = null);

        Task<bool> Copy(long DestinationFolderID, bool AutoRename);
        Task DownloadAsZip(string FileSaveDir, string FileName, IProgress<ReportStatus> ReportCls = null, System.Threading.CancellationToken token = default);



    }
}
