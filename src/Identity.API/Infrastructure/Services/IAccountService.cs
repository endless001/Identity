using Identity.API.Models;
using System.Threading.Tasks;

namespace Identity.API.Infrastructure.Services
{
    public interface IAccountService
    {
        Task<AccountModel> PasswordSignInAsync(string accountName,string password);
    }
}