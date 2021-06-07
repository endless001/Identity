using System;
using Identity.API.Configuration;
using Identity.API.Infrastructure.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static Account.API.Grpc.AccountGrpc;
using static Message.API.Grpc.MessageGrpc;

namespace Identity.API.Infrastructure.Extensions
{
    public  static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrpcServices(this IServiceCollection services)
        {
            services.AddTransient<GrpcExceptionInterceptor>();

            services.AddGrpcClient<AccountGrpcClient>((services, options) =>
            {
                var url = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcAccount;
                options.Address = new Uri(url);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            services.AddGrpcClient<MessageGrpcClient>((services, options) =>
            {
                var url = services.GetRequiredService<IOptions<UrlsConfig>>().Value.GrpcMessage;
                options.Address = new Uri(url);
            }).AddInterceptor<GrpcExceptionInterceptor>();

            return services;
        }
    }
}
