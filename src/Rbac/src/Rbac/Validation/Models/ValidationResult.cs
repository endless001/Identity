using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rbac.Validation.Models
{
    public class ValidationResult
    {
        public bool IsError { get; set; } = true;
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
    }
}
