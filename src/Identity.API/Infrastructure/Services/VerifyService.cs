using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Infrastructure.Services
{
    public class VerifyService : IVerifyService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public VerifyService(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }
        public async Task<bool> EmailVerifyAsync(string email, string code)
        {
            var verifyCode = (string)await _database.StringGetAsync($"Email{email}");
            return verifyCode == code;
        }

        public async Task<bool> SMSVerifyAsync(string phone, string code, string nationcode)
        {
            var verifyCode = (string)await _database.StringGetAsync($"SMS{nationcode}{phone}");
            return verifyCode == code;
        }

        private IServer GetServer()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
    }
}
