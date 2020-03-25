using System.Collections.Generic;
using System.Threading.Tasks;

namespace pCloudSDK
{

    public interface IItems
    {
        IFile File(long FileID);
        IFiles Files(List<long> FileIDs);
        IFolder Folder(long FolderID);
        IFolders Folders(List<long> FolderIDs);


        Task<JSON.JSON_FolderMetadata> ListAll();
    }

}