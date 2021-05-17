using Identity.API.Models;
using System.Threading.Tasks;

namespace Identity.API.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        public Task<AccountModel> PasswordSignInAsync(string accountName,string password)
        {
            throw new System.NotImplementedException();
        }
    }
}