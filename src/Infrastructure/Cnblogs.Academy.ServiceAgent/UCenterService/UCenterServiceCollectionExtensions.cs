using System;
using Polly;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.ServiceAgent.UCenterService
{
    public static class UCenterServiceCollectionExtensions
    {
        public static IServiceCollection AddUCenter(this IServiceCollection services, string hostname = "ucenter_api")
        {
            services.AddHttpClient<IUCenterService, UCenterService>(c =>
                {
                    c.BaseAddress = new Uri($"http://{hostname}");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(500)));

            return services;
        }
    }

}
