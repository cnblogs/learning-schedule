using System.Threading.Tasks;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.UcenterApi
{
    public interface IUcenterApi
    {
        [Get("/api/v2/user/email:{email}/loginNames")]
        Task<string[]> GetLoginNamesByEmail(string email);


    }
}
