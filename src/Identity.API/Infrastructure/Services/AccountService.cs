using Identity.API.Models;
using System.Threading.Tasks;
using Account.API.Grpc;
using AutoMapper;
using Identity.API.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Identity.API.Infrastructure.Services
{
  public class AccountService : IAccountService
  {
    private readonly ILogger<AccountService> _logger;
    private readonly UrlsConfig _urls;
    private readonly IMapper _mapper;

    public AccountService(ILogger<AccountService> logger,
      IOptions<UrlsConfig> urls,
      IMapper mapper)
    {
      _logger = logger;
      _urls = urls.Value;
      _mapper = mapper;
    }

    public async Task<AccountModel> PasswordSignInAsync(string accountName, string password)
    {
      var request = new AccountRequest()
      {
        AccountName = accountName,
        Password = password
      };
      var response = await GrpcCallerService.CallService(_urls.GrpcAccount, async channel =>
      {
        var client = new AccountGrpc.AccountGrpcClient(channel);;
         return await client.PasswordSignInAsync(request);
        
      });
      return _mapper.Map<AccountModel>(response);
    }
  }
}
