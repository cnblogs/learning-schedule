using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.ServiceAgent.RelationService
{
    public static class RelationServiceCollectionExtensions
    {
        public static IServiceCollection AddUserRelationService(this IServiceCollection services,string uri="http://relation_api")
        {
            services.AddTransient<IUserRelationGroupService, UserRelationGroupService>()
                .AddHttpClient("relation_api", x => { x.BaseAddress = new Uri(uri); })
                .AddTypedClient(x => Refit.RestService.For<IUserRelationGroupApi>(x));
            return services;
        }
    }
}
