using System.Net.Http;
using System.Threading.Tasks;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.MsgApi
{
    public interface IMsgApi
    {
        [Get("/api/sitemessages/user-{spaceUserId}/unread/count")]
        Task<HttpResponseMessage> UnreadCount(int spaceUserId);

        [Post("/api/notifications")]
        Task Notify([Body]Notification notification);
    }
}
