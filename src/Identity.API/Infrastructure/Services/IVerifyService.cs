using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Infrastructure.Services
{
    public interface IVerifyService
    {
        Task<bool> SMSVerifyAsync(string phone, string code, string nationcode);
        Task<bool> EmailVerifyAsync(string email, string code);
    }
}
