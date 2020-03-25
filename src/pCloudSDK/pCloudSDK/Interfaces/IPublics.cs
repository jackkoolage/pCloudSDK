using pCloudSDK.JSON;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace pCloudSDK
{
    public interface IPublics
    {

        /// <summary>
        /// https://docs.pcloud.com/methods/public_links/showpublink.html
        /// </summary>
        Task<JSON_PublicMetadata> Metadata(Uri publiclink);

        /// <summary>
        /// Returns link(s) where the file can be downloaded
        /// https://docs.pcloud.com/methods/public_links/getpublinkdownload.html
        /// </summary>
        Task<string> SingleFileDirectUrl(Uri publiclink);

        /// <summary>
        /// Returns link(s) where the file can be downloaded
        /// https://docs.pcloud.com/methods/public_links/getpublinkdownload.html
        /// </summary>
        Task<string> FileInFolderDirectUrl(Uri folderpubliclink, long fileid);

        /// <summary>
        /// Copies the file from the public link to the current user's filesystem
        /// https://docs.pcloud.com/methods/public_links/copypubfile.html
        /// </summary>
        Task<JSON_FileMetadata> SaveToMyAccount(Uri publiclink, long SaveIntoFolderID);

        /// <summary>
        /// Copies the file from the public link to the current user's filesystem
        /// https://docs.pcloud.com/methods/public_links/copypubfile.html
        /// </summary>
        Task<string> SaveToMyAccount(Uri folderpubliclink, long fileid, long SaveIntoFolderID);

        /// <summary>
        /// Return a list of current user's public links
        /// https://docs.pcloud.com/methods/public_links/listpublinks.html
        /// </summary>
        Task<List<JSON_PublicLinkMetadata>> List();

        /// <summary>
        /// Return a list of current user's public links listpublinks There is no metadata for each link
        /// https://docs.pcloud.com/methods/public_links/listplshort.html
        /// </summary>
        Task<List<JSON_PublicLinkMetadata>> ListWithoutMetadata();

        /// <summary>
        /// Create a zip archive file of a public link file and download it
        /// https://docs.pcloud.com/methods/public_links/getpubzip.html
        /// </summary>
        Task DownloadAsZip(Uri publiclink, string FileSaveDir, IProgress<ReportStatus> ReportCls = null, System.Threading.CancellationToken token = default);

        /// <summary>
        /// Create a link to a zip archive file of a public link file
        /// https://docs.pcloud.com/methods/public_links/getpubziplink.html
        /// </summary>
        /// <param name="ZipName">Filename is passed unaltered, so it MUST include the .zip extension</param>
        Task<string> DirectZipUrl(Uri publiclink, string ZipName);

        /// <summary>
        /// Get a thumbnail of a public file
        /// https://docs.pcloud.com/methods/public_links/getpubthumb.html
        /// </summary>
        /// <param name="WIDTH_x_HEIGHT">The parameter size MUST be provided, in the format WIDTHxHEIGHT. The width MUST be between 16 and 2048, and divisible by either 4 or 5. The height MUST be between 16 and 1024, and divisible by either 4 or 5. By default the thumb will have the same aspect ratio as the original image, so the resulting thumbnail width or height (but not both) might be less than requested. If you want thumbnail exactly the size specified, you can set crop parameter. With crop, thumbnails will still have the right aspect ratio, but if needed some rows or cols (but not both) will be cropped from both sides. So if you have 1024x768 image and are trying to create 128x128 thumbnail, first the image will be converted to 768x768 by cutting 128 columns from both sides and then resized to 128x128. To create a rectangular thumb from 4:3 image exactly 1/8 is cropped from each side. By default the thumbnail is in jpeg format.</param>
        Task<byte[]> Thumbnail(Uri publiclink, string WIDTH_x_HEIGHT, Utilitiez.ExtEnum Ext, bool Crop);

        /// <summary>
        /// Get a link to a thumbnatil of a public file
        /// https://docs.pcloud.com/methods/public_links/getpubthumblink.html
        /// </summary>
        /// <param name="WIDTH_x_HEIGHT">The parameter size MUST be provided, in the format WIDTHxHEIGHT. The width MUST be between 16 and 2048, and divisible by either 4 or 5. The height MUST be between 16 and 1024, and divisible by either 4 or 5. By default the thumb will have the same aspect ratio as the original image, so the resulting thumbnail width or height (but not both) might be less than requested. If you want thumbnail exactly the size specified, you can set crop parameter. With crop, thumbnails will still have the right aspect ratio, but if needed some rows or cols (but not both) will be cropped from both sides. So if you have 1024x768 image and are trying to create 128x128 thumbnail, first the image will be converted to 768x768 by cutting 128 columns from both sides and then resized to 128x128. To create a rectangular thumb from 4:3 image exactly 1/8 is cropped from each side. By default the thumbnail is in jpeg format.</param>
        Task<string> ThumbnailUrl(Uri publiclink, string WIDTH_x_HEIGHT, Utilitiez.ExtEnum Ext, bool Crop);

        /// <summary>
        /// Create a thumbnail of a public link file and save it in the current user's filesystem
        /// https://docs.pcloud.com/methods/public_links/savepubthumb.html
        /// </summary>
        Task<JSON_FileMetadata> CopyThumbnail(Uri publiclink, long DestinationFolderID, string WIDTH_x_HEIGHT, Utilitiez.ExtEnum Ext, bool Crop, bool AutoRename, string RenameTo = null);

        /// <summary>
        /// Create a zip archive file of a public link file in the current user filesystem
        /// https://docs.pcloud.com/methods/public_links/savepubzip.html
        /// </summary>
        Task<JSON_FileMetadata> CopyZip(Uri publiclink, long DestinationFolderID, string RenameTo = null);

        /// <summary>
        /// Returns [variants] array of different quality/resolution versions of a video in a public link
        /// https://docs.pcloud.com/methods/public_links/getpubvideolinks.html
        /// </summary>
        Task<List<JSON_VideoResolutions>> VideoResolutionUrls(Uri publiclink);

        /// <summary>
        /// Returns [variants] array of different quality/resolution versions of a video in a public link
        /// https://docs.pcloud.com/methods/public_links/getpubvideolinks.html
        /// </summary>
        Task<List<JSON_VideoResolutions>> VideoResolutionUrls(Uri publicfolderlink, long FileID);

        /// <summary>
        /// Create a link to a audio file of a public link file. The link could be used for streaming
        /// https://docs.pcloud.com/methods/public_links/getpubaudiolink.html
        /// </summary>
        /// <param name="AudioBitRate">audio bit rate in kilobits, from 16 to 320</param>
        Task<Uri> AudioDirectUrl(Uri publiclink, string AudioBitRate);

        /// <summary>
        /// Create a link to a audio file of a public link file. The link could be used for streaming
        /// https://docs.pcloud.com/methods/public_links/getpubaudiolink.html
        /// </summary>
        /// <param name="AudioBitRate">audio bit rate in kilobits, from 16 to 320</param>
        Task<Uri> AudioDirectUrl(Uri publicfolderlink, long FileID, string AudioBitRate);



    }
}
