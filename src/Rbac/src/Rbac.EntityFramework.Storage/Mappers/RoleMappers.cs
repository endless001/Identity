using AutoMapper;
using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.EntityFramework.Storage.Mappers
{
    public static class RoleMappers
    {
        internal static IMapper Mapper;

        static RoleMappers()
        {
            Mapper = new MapperConfiguration(cfg =>
                    cfg.AddProfile<RoleMapperProfile>())
                .CreateMapper();
        }

        public static Role ToModel(this Entities.Role entity)
        {
            return Mapper.Map<Role>(entity);
        }

        public static Entities.Role ToEntity(this Role model)
        {
            return Mapper.Map<Entities.Role>(model);
        }
    }
}
