using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace Cnblogs.Academy.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseBeatPulse(x =>
                {
                    x.ConfigurePath("alive")
                    .ConfigureTimeout(1500)
                    .ConfigureDetailedOutput();
                })
                .UseStartup<Startup>()
                .UseSerilog((ctx, conf) =>
                {
                    if (ctx.HostingEnvironment.IsDevelopment())
                    {
                        conf.WriteTo.Console(LogEventLevel.Information);
                    }
                    else
                    {
                        conf.ReadFrom.Configuration(ctx.Configuration);
                    }
                });
    }
}
