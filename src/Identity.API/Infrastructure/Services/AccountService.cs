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
        private readonly IMapper _mapper;

        public AccountService(ILogger<AccountService> logger,
             AccountGrpcClient accountGrpcClient,
          IMapper mapper)
        {
            _logger = logger;
            _accountGrpcClient = accountGrpcClient;
            _mapper = mapper;
        }

        public async Task<AccountModel> PasswordSignInAsync(string accountName, string password)
        {
            var request = new AccountRequest()
            {
                AccountName = accountName,
                Password = password
            };
            var response = await _accountGrpcClient.PasswordSignInAsync(request);

            return new AccountModel()
            {
                AccountId = response.AccountId,
                AccountName = response.AccountName,
                Avatar = response.Avatar,
                Email = response.Email,
                Phone = response.Phone,
                Sex = response.Sex,
                Tel = response.Tel
            };
         }
    }
}
