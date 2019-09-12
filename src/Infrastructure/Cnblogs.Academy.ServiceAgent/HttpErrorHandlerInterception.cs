using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Cnblogs.Academy.ServiceAgent
{
    public class HttpErrorHandlerInterception : HttpClientHandler
    {
        private readonly ILogger _logger;
        public HttpErrorHandlerInterception(ILogger logger)
        {
            _logger = logger;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                _logger.LogError($"{response.RequestMessage.RequestUri} Response ({response.StatusCode}) isn't success. {msg}");

                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new ValidationException(msg);
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    return response;
                else
                    throw new ValidationException("服务异常，请稍后重试");
            }
            return response;
        }
    }
}