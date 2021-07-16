using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.EntityFramework.Storage.Entities
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// operate page
        /// </summary>
        /// 
        public int Type { get; set; }
        public int ParentId { get; set; }
        public string RouterUrl { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
    }
}
