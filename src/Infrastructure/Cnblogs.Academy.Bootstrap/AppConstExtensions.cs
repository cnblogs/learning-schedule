using System.Text;
using Cnblogs.Academy.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.Bootstrap
{
    public static class AppConstExtensions
    {
        public static IServiceCollection AddAppConst(this IServiceCollection services, IConfiguration configuration)
        {
            AppConst.AppId = configuration["AppId"];
            AppConst.DomainAddress = configuration.GetValue<string>("DomainAddress");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return services;
        }
    }
}
