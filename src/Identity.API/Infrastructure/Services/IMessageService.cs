using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Infrastructure.Services
{
    public interface IMessageService
    {
        Task<bool> SendSMS();
        Task<bool> SendMail(string address,string content);
    }
}
