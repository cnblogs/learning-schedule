using Cnblogs.Academy.Repositories;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.Bootstrap
{
    public static class CapConfiguration
    {
        /// <summary>
        /// Must add cap after subscribers dependent
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCap(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCap(options =>
            {
                options.UseEntityFramework<AcademyContext>();
                options.Version = configuration.GetValue<string>("Cap:Version");
                var mq = new RabbitMQOptions();
                configuration.GetSection("RabbitMq").Bind(mq);
                options.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = mq.HostName;
                    cfg.Port = mq.Port;
                    cfg.UserName = mq.UserName;
                    cfg.Password = mq.Password;
                });
                options.UseDashboard(d =>
                {
                    d.PathMatch = "/academy/cap";
                });
            });
            return services;
        }
    }
}
