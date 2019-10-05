using Cnblogs.Academy.WebAPI.Setup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using BeatPulse;
using Cnblogs.Academy.Bootstrap;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Cnblogs.Academy.ServiceAgent.RelationService;
using Cnblogs.Academy.ServiceAgent.FeedService;

namespace Cnblogs.Academy.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppConst(Configuration)
                    .AddJsonSettings()
                    .AddEF(Configuration)
                    .AddMapper()
                    .AddSingleton(Configuration)
                    .AddEnyimMemcached(Configuration)
                    .AddUCenter(Configuration.GetValue<string>("Host:UCenter"))
                    .AddUserRelationService(Configuration.GetValue<string>("Host:Relation"))
                    .AddFeedService(Configuration.GetValue<string>("Host:Feeds"))
                    .AddAcademyServices(Environment.IsDevelopment(), Configuration)
                    .AddCap(Configuration)
                    .AddMediatoR()
                    .AddBeatPulse(x =>
                    {
                        x.AddSqlServer(Configuration.GetConnectionString("Academy"));
                    })
                    .AddRedis(Configuration, out var redisConn, out var redisConnectionString)
                    .AddCnblogsAuthentication(redisConn, redisConnectionString, option =>
                    {
                        option.Events = new CnblogsCookieAuthenticationEvents();
                    })
                    .AddMvc(options =>
                    {
                        options.Filters.Add<ExceptionFilter>();
                        // options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication().UseUCenter();

            app.UseMvc();
        }
    }

    public static class RedisExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services,
                                                       IConfiguration configuration,
                                                       out ConnectionMultiplexer redisConn,
                                                       out string redisConnectionString)
        {
            redisConn = RedisManager.Connect(configuration.GetSection("redis"), out redisConnectionString);
            services.AddSingleton(redisConn);
            return services;
        }
    }
}
