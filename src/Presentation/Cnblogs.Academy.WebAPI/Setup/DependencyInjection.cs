using System;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.ServiceAgent.HotCommentApi;
using Cnblogs.Academy.ServiceAgent.MsgApi;
using Cnblogs.Academy.Repositories.Repositories;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using Cnblogs.Academy.ServiceAgent.FeedsApi;
using Cnblogs.Academy.Application.FeedsAppService;
using Cnblogs.Academy.Application.CategoryAppService;
using Cnblogs.Academy.Domain.Categories;
using Cnblogs.Academy.ServiceAgent.UcenterApi;
using Cnblogs.Academy.Application.AccountEventSubscriber;
using Cnblogs.Academy.ServiceAgent.RelationService;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Cnblogs.Academy.Subscriber;
using Enyim.Caching;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.WebAPI.Setup
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAcademy(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfiguration config)
        {
            services.AddSingleton<IConfiguration>(config);
            services.AddEnyimMemcached(config);

            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<ICategoryService, CategoryService>();

            services.AddTransient<IMarkdownApiService, MarkdownService>();
            services.AddUCenter();

            if (env.IsDevelopment())
            {
                services.AddTransient<IMsgApiService, FakeMsgApiService>();
                services.AddTransient<IHotCommentApiService, HotCommentApiService>()
                    .AddHttpClient(
                        "comment",
                        cfg => { cfg.BaseAddress = new Uri(config.GetSection("Host")["Comment"]); })
                    .AddTypedClient(Refit.RestService.For<IHotCommentApi>);

                services.AddTransient<IThumbupApiService, ThumbupApiService>()
                    .AddHttpClient("vote", cfg => { cfg.BaseAddress = new Uri(config.GetSection("Host")["Vote"]); })
                    .AddTypedClient(Refit.RestService.For<IThumbupApi>);

                services.AddTransient<IFeedsAppService, FeedsAppService>()
                    .AddHttpClient("feeds", cfg => { cfg.BaseAddress = new Uri(config.GetSection("Host")["Feeds"]); })
                    .AddTypedClient(Refit.RestService.For<IFeedsApi>);

                services.AddTransient<IUserRelationGroupService, UserRelationGroupService>()
                    .AddHttpClient(
                        "relation",
                        cfg =>
                        {
                            cfg.BaseAddress = new Uri(config.GetSection("Host")["Relation"]);
                        })
                    .AddTypedClient(Refit.RestService.For<IUserRelationGroupApi>);
            }
            else
            {
                services.AddTransient<IHotCommentApiService, HotCommentApiService>()
                    .AddHttpClient("comment", cfg => { cfg.BaseAddress = new Uri("http://comment_api"); })
                    .AddTypedClient(Refit.RestService.For<IHotCommentApi>);

                services.AddTransient<IThumbupApiService, ThumbupApiService>()
                    .AddHttpClient("vote", cfg => { cfg.BaseAddress = new Uri("http://vote_api"); })
                    .AddTypedClient(Refit.RestService.For<IThumbupApi>);

                services.AddTransient<IMsgApiService, MsgApiService>()
                    .AddHttpClient("msg", cfg => { cfg.BaseAddress = new Uri("http://msg_api"); })
                    .AddTypedClient(Refit.RestService.For<IMsgApi>);

                services.AddTransient<IFeedsAppService, FeedsAppService>()
                    .AddHttpClient("feeds", cfg => { cfg.BaseAddress = new Uri("http://feeds_api"); })
                    .AddTypedClient(Refit.RestService.For<IFeedsApi>);

                services.AddTransient<IUserRelationGroupService, UserRelationGroupService>()
                    .AddHttpClient(
                        "relation",
                        cfg =>
                        {
                            cfg.BaseAddress = new Uri("http://relation_api");
                        })
                    .AddTypedClient(Refit.RestService.For<IUserRelationGroupApi>);
            }

            services.AddHttpClient(
                "ucenter",
                cfg =>
                {
                    cfg.BaseAddress = new Uri("http://ucenter_api");
                }).AddTypedClient(Refit.RestService.For<IUcenterApi>);

            services.AddTransient<IAccountEventSubscriber, AccountEventSubscriber>();
            services.AddTransient<IScheduleEventHandler, ScheduleEventHandler>();
            services.AddTransient<IFeedbackEventHandler, FeedbackEventHandler>();

            return services;
        }
    }
}
