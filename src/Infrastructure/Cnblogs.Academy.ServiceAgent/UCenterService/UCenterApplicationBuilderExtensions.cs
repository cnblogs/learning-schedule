using Microsoft.AspNetCore.Builder;

namespace Cnblogs.Academy.ServiceAgent.UCenterService
{
    public static class UCenterApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUCenter(this IApplicationBuilder app)
        {
            app.UseMiddleware<UCenterMiddleware>();
            return app;
        }
    }
}
