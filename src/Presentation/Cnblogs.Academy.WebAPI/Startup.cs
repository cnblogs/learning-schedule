using System;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BeatPulse;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.WebAPI.Setup;
using Cnblogs.CAP.RedisUtility;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Repositories;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            MapperConfig.Init();
            AppConst.AppId = Configuration["AppId"];
            AppConst.DomainAddress = Configuration.GetValue<string>("DomainAddress");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.AddEntityFrameworkSqlServer().AddDbContextPool<AcademyContext>(
                (serviceProvider, options) =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("Academy"), x => x.UseRowNumberForPaging());
                    options.UseApplicationServiceProvider(serviceProvider);
                },
                poolSize: 64);

            services.AddAcademy(Environment, Configuration).AddCap(
                options =>
                {
                    options.UseEntityFramework<AcademyContext>(x => { x.UseSqlServer2008(); });

                    options.Version = Configuration.GetValue<string>("Cap:Version");

                    var mq = new RabbitMQOptions();
                    Configuration.GetSection("RabbitMq").Bind(mq);
                    options.UseRabbitMQ(
                        cfg =>
                        {
                            cfg.HostName = mq.HostName;
                            cfg.Port = mq.Port;
                            cfg.UserName = mq.UserName;
                            cfg.Password = mq.Password;
                        });

                    options.UseDashboard(
                        d =>
                        {
                            d.PathMatch = "/academy/cap";
                        });
                });

            services.AddBeatPulse(
                x =>
                {
                    x.AddSqlServer(Configuration.GetConnectionString("Academy"));
                });

            services.AddCnblogsAuthentication(Configuration);

            services.AddMvc(
                options =>
                {
                    options.Filters.Add<ExceptionFilter>();
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<ServiceAgent.MarkdownApi.AutoLinkTitleService>();

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // TODO: add custom error pages
//                app.UseCustomErrorPages();
            }

            app.UseStaticFiles();

            app.UseAuthentication().UseUCenter();

            app.UseMvc();
        }
    }
}
