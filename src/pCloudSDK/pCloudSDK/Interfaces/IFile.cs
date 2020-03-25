using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pCloudSDK
{
  public   interface IFile
    {

        /// <summary>
        /// Creates and return a public link to a file
        /// https://docs.pcloud.com/methods/public_links/getfilepublink.html
        /// </summary>
        /// <param name="MaxDownloads">Maximum number of downloads for this file</param>
        /// <param name="MaxTrafficInBytes">Maximum traffic that this link will consume (in bytes, started downloads will not be cut to fit in this limit)</param>
        Task<string> Public(int? MaxDownloads = null, long? MaxTrafficInBytes = null);

        /// <summary>
        /// Get a thumbnail of a file
        /// Takes the same parameters as getthumblink, but returns the thumbnail over the current API connection.
        /// Getting thumbnails from API servers is generally NOT faster than getting them from storage servers.
        /// It makes sense only if you are reusing the (possibly expensive to open SSL) API connection.
        /// </summary>
        /// <param name="WIDTH_x_HEIGHT">The parameter size MUST be provided, in the format WIDTHxHEIGHT. The width MUST be between 16 and 2048, and divisible by either 4 or 5. The height MUST be between 16 and 1024, and divisible by either 4 or 5. By default the thumb will have the same aspect ratio as the original image, so the resulting thumbnail width or height (but not both) might be less than requested. If you want thumbnail exactly the size specified, you can set crop parameter. With crop, thumbnails will still have the right aspect ratio, but if needed some rows or cols (but not both) will be cropped from both sides. So if you have 1024x768 image and are trying to create 128x128 thumbnail, first the image will be converted to 768x768 by cutting 128 columns from both sides and then resized to 128x128. To create a rectangular thumb from 4:3 image exactly 1/8 is cropped from each side. By default the thumbnail is in jpeg format.</param>
        Task<byte[]> Thumbnail(string WIDTH_x_HEIGHT, Utilitiez.ExtEnum Ext, bool Crop);

        /// <summary>
        /// Get a link to a thumbnail of a file
        /// https://docs.pcloud.com/methods/thumbnails/getthumblink.html
        /// </summary>
        /// <param name="WIDTH_x_HEIGHT">The parameter size MUST be provided, in the format WIDTHxHEIGHT. The width MUST be between 16 and 2048, and divisible by either 4 or 5. The height MUST be between 16 and 1024, and divisible by either 4 or 5. By default the thumb will have the same aspect ratio as the original image, so the resulting thumbnail width or height (but not both) might be less than requested. If you want thumbnail exactly the size specified, you can set crop parameter. With crop, thumbnails will still have the right aspect ratio, but if needed some rows or cols (but not both) will be cropped from both sides. So if you have 1024x768 image and are trying to create 128x128 thumbnail, first the image will be converted to 768x768 by cutting 128 columns from both sides and then resized to 128x128. To create a rectangular thumb from 4:3 image exactly 1/8 is cropped from each side. By default the thumbnail is in jpeg format.</param>
        Task<string> ThumbnailUrl(string WIDTH_x_HEIGHT, Utilitiez.ExtEnum Ext, bool Crop);


        Task<JSON_FileMetadata> CopyThumbnail(long DestinationFolderID, string WIDTH_x_HEIGHT, Utilitiez.ExtEnum Ext, bool Crop, bool AutoRename, string RenameTo = null);

        Task<Uri> VideoToMp3(int AudioBitRate);

        /// <summary>
        /// Get a streaming link for audio file
        /// https://docs.pcloud.com/methods/streaming/getaudiolink.html
        /// </summary>
        /// <param name="AudioBitRate">audio bit rate in kilobits, from 16 to 320</param>
        Task<Uri> AudioDirectUrl(int AudioBitRate);

        Task<List<JSON_Entry>> ChangesHistory();

        /// <summary>
        /// Create a zip file in the user's filesystem
        /// </summary>
        Task<string> CompressAsync(long SaveIntoFolderID, string Filename = null);

        Task<bool> UnCompressTaskProgress(string JobID);
        /// <summary>
        /// Extracts archive file from the user's filesystem.
        /// </summary>
        /// <param name="DestinationFolderID">extraction output path</param>
        /// <param name="ArchivePassword">password to use to extract a password protected archive</param>
        /// <param name="IfExists">what to do if file to extract already exists in the folder</param>
        /// <returns>job id</returns>
        Task<string> UnCompressAsync(long DestinationFolderID, string ArchivePassword = null, Utilitiez.IfExistsEnum IfExists = Utilitiez.IfExistsEnum.rename);
        
        /// <summary>
        /// four hours direct url
        /// https://docs.pcloud.com/methods/streaming/getfilelink.html
        /// </summary>
        Task<string> DirectUrl();

        /// <summary>
        /// four hours video stream url
        /// https://docs.pcloud.com/methods/streaming/getvideolink.html
        /// </summary>
        Task<string> VideoDirectUrl();

        /// <summary>
        /// Returns [variants] array of different quality/resolution versions of a video
        /// https://docs.pcloud.com/methods/streaming/getvideolinks.html
        /// </summary>
        Task<List<JSON_VideoResolutions>> VideoResolutionUrls();

        /// <summary>
        /// Calculate checksums of a given file [md5 - sha1]
        /// https://docs.pcloud.com/methods/file/checksumfile.html
        /// </summary>
        Task<JSON_Checksum> Checksum();

        /// <summary>
        /// moves a file
        /// https://docs.pcloud.com/methods/file/renamefile.html
        /// </summary>
        Task<JSON_FileMetadata> Move(long DestinationFolderID, string RenameTo = null);

        /// <summary>
        /// Renames a file
        /// https://docs.pcloud.com/methods/file/renamefile.html
        /// </summary>
        Task<JSON_FileMetadata> Rename(string RenameTo);

        /// <summary>
        /// Delete a file
        /// https://docs.pcloud.com/methods/file/deletefile.html
        /// </summary>
        Task<bool> Delete();

        /// <summary>
        /// Takes one file and copies it as another folder in the user's filesystem
        /// https://docs.pcloud.com/methods/file/copyfile.html
        /// </summary>
        /// <param name="AutoRename">If file with the specified name already exists, no overwriting will be preformed</param>
        /// <param name="RenameTo">name of the destination file. If omitted, then the original filename is used</param>
        Task<JSON_FileMetadata> Copy(long DestinationFolderID, bool AutoRename, string RenameTo = null);


    }
}
