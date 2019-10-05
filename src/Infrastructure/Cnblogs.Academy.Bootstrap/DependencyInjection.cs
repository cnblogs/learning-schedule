using System;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using Cnblogs.Academy.ServiceAgent.UcenterApi;
using Cnblogs.Academy.Application.Queries;
using Cnblogs.Academy.ServiceAgent.BlogApi;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Repositories;
using Cnblogs.Domain.Abstract;
using Cnblogs.Academy.Application.ScheduleAppService;

namespace Cnblogs.Academy.Bootstrap
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAcademyServices(this IServiceCollection services, bool isDev, IConfiguration config)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IScheduleQueries>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .FromAssemblyOf<IScheduleService>()
                .AddClasses()
                .AsImplementedInterfaces()
                .WithTransientLifetime()
                .FromAssembliesOf(typeof(BaseEntity), typeof(AcademyContext))
                .AddClasses(x => x.AssignableTo(typeof(IRepository<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // services.AddTransient<IMarkdownApiService, MarkdownApiService>()
            //     .AddHttpClient("markdown", c => { c.BaseAddress = new Uri("http://markdown_api"); })
            //     .AddTypedClient(c => Refit.RestService.For<IMarkdownApi>(c));

            services.AddTransient<AutoLinkTitleService>();
            services.AddTransient<IMarkdownApiService, MarkdownService>();

            if (isDev)
            {
                services.AddTransient<IMsgApiService, FakeMsgApiService>();
                // services.AddSingleton<IMemcachedClient, NullMemcachedClient>();

                services.AddTransient<IBlogApiService, FakeBlogApiService>();
            }
            else
            {
                services.AddTransient<IMsgApiService, MsgApiService>()
                        .AddHttpClient("msg", cfg => { cfg.BaseAddress = new Uri("http://msg_api"); })
                        .AddTypedClient(c => Refit.RestService.For<IMsgApi>(c));

                services.AddTransient<IBlogApiService, BlogApiService>()
                        .AddHttpClient("blog", cfg => { cfg.BaseAddress = new Uri("http://blog_api"); })
                        .AddTypedClient(c => Refit.RestService.For<IBlogApi>(c));
            }

            services.AddHttpClient("ucenter", cfg => { cfg.BaseAddress = new Uri("http://ucenter_api"); })
                    .AddTypedClient(c => Refit.RestService.For<IUcenterApi>(c));

            return services;
        }
    }
}
