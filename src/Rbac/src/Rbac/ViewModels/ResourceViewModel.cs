using Rbac.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.ViewModels
{
   public class ResourceViewModel
    {
        public Resource Resource { set; get; }
        public List<ResourceViewModel> Children { set; get; }
    }
}
