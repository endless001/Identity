using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.Storage.Models
{
    public class RoleAccess
    {
        public int Id { get; set; }
        public int RoleId{ get; set; }
        public int ResourceId { get; set; }
    }
}
