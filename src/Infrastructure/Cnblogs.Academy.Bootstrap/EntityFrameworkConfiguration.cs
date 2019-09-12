using Cnblogs.Academy.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Bootstrap
{
    public static class EntityFrameworkConfiguration
    {
        public static IServiceCollection AddEF(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer().AddDbContextPool<AcademyContext>((serviceProvider, options) =>
                        {
                            options.UseSqlServer(configuration.GetConnectionString("Academy"));
                            options.UseInternalServiceProvider(serviceProvider);
                        }, poolSize: 64);
            return services;
        }
    }
}
