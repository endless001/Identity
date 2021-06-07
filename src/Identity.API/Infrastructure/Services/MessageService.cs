using Message.API.Grpc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Message.API.Grpc.MessageGrpc;

namespace Identity.API.Infrastructure.Services
{
    public class MessageService : IMessageService
    {


        private readonly ILogger<MessageService> _logger;
        private readonly MessageGrpcClient _messageGrpcClient;

        public MessageService(ILogger<MessageService> logger,
             MessageGrpcClient messageGrpcClient)
        {
            _logger = logger;
            _messageGrpcClient = messageGrpcClient;
        }


        public async Task<bool> SendMail(string address, string content)
        {
            var request = new MailRequest
            {
                Address = address,
                Content = content
            };
            var response = await _messageGrpcClient.SendMailAsync(request);

            return true;
        }

        public Task<bool> SendSMS()
        {
            throw new NotImplementedException();
        }
    }
}
