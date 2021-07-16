using Rbac.Storage.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rbac.Storage.Stores
{
    public interface IRoleAccessStore
    {
        Task<bool> AllowAccessAsync(int roleId, int resourceId);

        Task<List<RoleAccess>> GetRoleAccessAsync(int roleId);
    }
}
