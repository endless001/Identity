using AutoMapper;
using Rbac.Storage.Models;

namespace Rbac.EntityFramework.Storage.Mappers
{
    public static class RoleAccessMappers
    {
        internal static IMapper Mapper;

        static RoleAccessMappers()
        {
            Mapper = new MapperConfiguration(cfg =>
                    cfg.AddProfile<RoleAccessMapperProfile>())
                .CreateMapper();
        }

        public static RoleAccess ToModel(this Entities.RoleAccess entity)
        {
            return Mapper.Map<RoleAccess>(entity);
        }

        public static Entities.RoleAccess ToEntity(this RoleAccess model)
        {
            return Mapper.Map<Entities.RoleAccess>(model);
        }
    }
}
