using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Cnblogs.Academy.WebAPI.Utils
{
    public static class RequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public static string GetAbsoluteUri(this HttpRequest request)
        {
            var scheme = request.Scheme;
            if (scheme == "http" && request.Headers["X-Forwarded-Proto"].Any(p => p == "https"))
            {
                scheme = "https";
            }

            return new StringBuilder()
                .Append(scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }
    }
}
