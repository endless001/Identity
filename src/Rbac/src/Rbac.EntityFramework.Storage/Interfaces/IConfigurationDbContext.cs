using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Rbac.EntityFramework.Storage.Entities;

namespace Rbac.EntityFramework.Storage.Interfaces
{
    public interface IConfigurationDbContext : IDisposable
    {
        DbSet<RoleAccess> RoleAccesss { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Resource> Resources { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        Task<int> SaveChangesAsync();
        EntityEntry Entry(object entity);
    }
}
