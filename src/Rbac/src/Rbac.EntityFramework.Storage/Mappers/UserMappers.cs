using AutoMapper;
using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.EntityFramework.Storage.Mappers
{
    public static class UserMappers
    {
        internal static IMapper Mapper;

        static UserMappers()
        {
            Mapper = new MapperConfiguration(cfg =>
                    cfg.AddProfile<UserMapperProfile>())
                .CreateMapper();
        }

        public static User ToModel(this Entities.User entity)
        {
            return Mapper.Map<User>(entity);
        }

        public static Entities.User ToEntity(this User model)
        {
            return Mapper.Map<Entities.User>(model);
        }

    }
}
