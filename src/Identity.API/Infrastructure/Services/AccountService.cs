using Identity.API.Models;
using System.Threading.Tasks;
using Account.API.Grpc;
using AutoMapper;
using Identity.API.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using static Account.API.Grpc.AccountGrpc;

namespace Identity.API.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly AccountGrpcClient _accountGrpcClient;

        public AccountService(ILogger<AccountService> logger,
             AccountGrpcClient accountGrpcClient)
        {
            _logger = logger;
            _accountGrpcClient = accountGrpcClient;
        }

        public async Task<AccountModel> PasswordSignInAsync(string accountName, string password)
        {
            var request = new AccountRequest()
            {
                AccountName = accountName,
                Password = password
            };
            var response = await _accountGrpcClient.PasswordSignInAsync(request);
            if (response is null)
            {
                return null;
            }
            return  MapResponse(response);
         }

        public async Task<AccountModel> RegisterEmailAsync(string email, string password)
        {
            var request = new AccountRequest()
            {
                Email = email,
                Password = password
            };
            var response = await _accountGrpcClient.RegisterEmailAsync(request);
            if (response is null)
            {
                return null;
            }
            return MapResponse(response);
        }

        private AccountModel MapResponse(AccountResponse account)
        {
            return new AccountModel()
            {
                AccountId = account.AccountId,
                Avatar = account.Avatar ?? string.Empty,
                AccountName = account.AccountName ?? string.Empty,
                Email = account.Email ?? string.Empty,
                Phone = account.Phone ?? string.Empty,
                Sex = account.Sex,
                Tel = account.Tel ?? string.Empty
            };
        }

        private AccountRequest MapRequest(AccountModel account)
        {
            return new AccountRequest()
            {
                AccountId = account.AccountId,
                Password = account.Password,
                Avatar = account.Avatar ?? string.Empty,
                AccountName = account.AccountName ?? string.Empty,
                Email = account.Email ?? string.Empty,
                Phone = account.Phone ?? string.Empty,
                Sex = account.Sex,
                Tel = account.Tel ?? string.Empty
            };
        }
    }
}
