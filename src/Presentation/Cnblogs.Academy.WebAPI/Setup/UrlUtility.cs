using Microsoft.AspNetCore.Server.HttpSys;

namespace Cnblogs.Academy.WebAPI.Setup
{
    public class UrlUtility
    {
        public static string RetrieveHostUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var uri = UrlPrefix.Create(url);
                if (uri.Host == "+" || uri.Host == "*")
                {
                    return uri.Scheme + "://localhost:" + uri.Port;
                }
            }
            return url;
        }
    }
}
