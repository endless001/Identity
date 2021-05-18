using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string Password { get; set; }
        public string LogoutId { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
