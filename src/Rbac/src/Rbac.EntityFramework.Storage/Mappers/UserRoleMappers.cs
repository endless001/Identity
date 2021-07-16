using AutoMapper;
using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.EntityFramework.Storage.Mappers
{
    public static class UserRoleMappers
    {
        internal static IMapper Mapper;

        static UserRoleMappers()
        {
            Mapper = new MapperConfiguration(cfg =>
                    cfg.AddProfile<UserRoleMapperProfile>())
                .CreateMapper();
        }

        public static UserRole ToModel(this Entities.UserRole entity)
        {
            return Mapper.Map<UserRole>(entity);
        }

        public static Entities.UserRole ToEntity(this UserRole model)
        {
            return Mapper.Map<Entities.UserRole>(model);
        }

    }
}
