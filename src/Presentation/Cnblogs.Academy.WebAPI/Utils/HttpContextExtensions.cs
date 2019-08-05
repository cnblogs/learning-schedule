using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Cnblogs.Academy.WebAPI.Utils
{
    public static class HttpContextExtensions
    {
        public static string GetUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress?.ToString();
            }
            return GetSingleIP(ip);
        }

        private static string GetSingleIP(string ip)
        {
            if (!string.IsNullOrEmpty(ip))
            {
                var commaIndex = ip.LastIndexOf(",");
                if (commaIndex >= 0)
                {
                    ip = ip.Substring(commaIndex + 1);
                }
            }
            return ip;
        }

    }
}
