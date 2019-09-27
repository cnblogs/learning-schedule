using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.ServiceAgent.FeedService
{
   public static class FeedServiceCollectionExtensions
    {
        public static IServiceCollection AddFeedService(this IServiceCollection services)
        {
            return services;
        }
    }
}
