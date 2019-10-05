using JsonNet.ContractResolvers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Cnblogs.Academy.Bootstrap
{
    public static class JsonConfiguration
    {
        public static IServiceCollection AddJsonSettings(this IServiceCollection services)
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new PrivateSetterAndCtorCamelCasePropertyNamesContractResolver();
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return settings;
            };
            return services;
        }
    }
}
