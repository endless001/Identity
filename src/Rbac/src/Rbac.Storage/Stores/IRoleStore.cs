using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.Storage.Stores
{
    public interface IRoleStore
    {
        Task<Role> FindRoleByIdAsync(int id);
    }
}
