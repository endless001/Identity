using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rbac.Storage.Models;
using Rbac.Storage.Stores;
using Rbac.ViewModels;


namespace Rbac.Services.Default
{
    public class ResourceService : IResourceService
    {
        private readonly IRoleAccessStore _roleAccessStore;
        private readonly IResourceStore _resourceStore;

        public ResourceService(IRoleAccessStore roleAccessStore, IResourceStore resourceStore)
        {
            _roleAccessStore = roleAccessStore;
            _resourceStore = resourceStore;
        }
        public async Task<IEnumerable<ResourceViewModel>> GetRoleResourcesAsync(int roleId)
        {
            var accesss = await _roleAccessStore.GetRoleAccessAsync(roleId);
            var resources = await _resourceStore.GetResourcesAsync();
            return BuildResources(resources, -1);
        }

        private IEnumerable<ResourceViewModel> BuildResources(IEnumerable<Resource> resources, int parentId)
        {

            Func<int, IEnumerable<ResourceViewModel>> func = null;

            func = new Func<int, IEnumerable<ResourceViewModel>>(m =>
            {
                List<ResourceViewModel> result = new List<ResourceViewModel>();
                foreach (var resource in resources.Where(a => a.ParentId == m))
                {
                    var childrens = func(resource.Id);
                    result.Add(new ResourceViewModel()
                    {
                        Resource = resource,
                        Children = childrens.ToList()
                    });
                }
                return result;
            });

            return func(parentId);
        }
    }
}
