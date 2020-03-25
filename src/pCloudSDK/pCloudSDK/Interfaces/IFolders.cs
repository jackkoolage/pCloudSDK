
using System.Threading.Tasks;

namespace pCloudSDK
{
    public interface IFolders
    {
        Task<string> CompressAsync(long DestinationFolderID, string Filename = null);
    }

}