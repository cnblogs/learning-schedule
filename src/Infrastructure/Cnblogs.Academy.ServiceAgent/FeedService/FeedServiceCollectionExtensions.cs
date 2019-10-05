using System;
using System.Collections.Generic;
using System.Text;
using Cnblogs.Academy.ServiceAgent.FeedsApi;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.ServiceAgent.FeedService
{
   public static class FeedServiceCollectionExtensions
    {
        public static IServiceCollection AddFeedService(this IServiceCollection services, string uri = "http://feeds_api")
        {
            services.AddHttpClient("feeds_api",x=>{
                x.BaseAddress = new Uri(uri);
            }).AddTypedClient(x=>Refit.RestService.For<IFeedsApi>(x))
            .Services.AddTransient<IFeedServiceAgent, FeedServiceAgent>();
            return services;
        }
    }
}
