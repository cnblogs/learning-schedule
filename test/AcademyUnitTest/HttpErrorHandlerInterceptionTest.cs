using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using Cnblogs.Academy.ServiceAgent;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AcademyUnitTest
{
    public class HttpErrorHandlerInterceptionTest
    {
        [Fact(Skip = "No endpoint was found")]
        public async void SendTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var http = new HttpClient(new HttpErrorHandlerInterception(NullLogger.Instance))
            {
                BaseAddress = new Uri("http://localhost")
            };
            await Assert.ThrowsAsync<ValidationException>(async () =>
             {
                 await http.GetAsync("api/foo");
             });
        }
    }
}
