using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.DTO.Books;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.ZzkApi
{
    public interface IZzkApi
    {
        [Post("/api/blogpost/zzkDocs")]
        Task<ZzkResponse> Search([Body]QueryModel model);
    }
}
