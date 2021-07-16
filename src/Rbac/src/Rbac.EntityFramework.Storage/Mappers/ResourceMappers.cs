using AutoMapper;
using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.EntityFramework.Storage.Mappers
{
    public static class ResourceMappers
    {
        internal static IMapper Mapper;

        static ResourceMappers()
        {
            Mapper = new MapperConfiguration(cfg =>
                    cfg.AddProfile<ResourceMapperProfile>())
                .CreateMapper();
        }

        public static Resource ToModel(this Entities.Resource entity)
        {
            return Mapper.Map<Resource>(entity);
        }

        public static Entities.Resource ToEntity(this Resource model)
        {
            return Mapper.Map<Entities.Resource>(model);
        }

    }
}
