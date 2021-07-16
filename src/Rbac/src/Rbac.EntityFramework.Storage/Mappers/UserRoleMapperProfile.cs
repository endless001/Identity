using AutoMapper;
using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.EntityFramework.Storage.Mappers
{
    public class UserRoleMapperProfile: Profile
    {
        public UserRoleMapperProfile()
        {
            CreateMap<Entities.UserRole, UserRole>();

        }
    }
}
