using Rbac.Storage.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rbac.Storage.Stores
{
    public interface IResourceStore
    {
        Task<Resource> GetResourceAsync(string path,string method);
        Task<List<Resource>> GetResourcesAsync();
    }
}
