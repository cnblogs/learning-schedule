using System.IO;
using System.Threading.Tasks;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using Microsoft.AspNetCore.Mvc;

namespace Cnblogs.Academy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class MarkdownController : AcademyControllerBase
    {
        private readonly IMarkdownApiService _svc;
        public MarkdownController(IMarkdownApiService svc)
        {
            _svc = svc;
        }

        [HttpPost]
        public async Task<IActionResult> ToHtml()
        {
            var text = string.Empty;
            using (var sr = new StreamReader(Request.Body))
            {
                text = sr.ReadToEnd();
            }
            var html = await _svc.ToHtml(text);
            return new JsonResult(html);
        }
    }
}
