using System;
using System.ComponentModel.DataAnnotations;


namespace Identity.API.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string VerifyCode { get; set; }
    }
}
