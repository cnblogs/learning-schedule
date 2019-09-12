using System.Threading.Tasks;

namespace Cnblogs.Academy.ServiceAgent.MsgApi
{
    public interface IMsgApiService
    {
        Task<int> UnreadCount(int spaceUserId);
        Task NotifyAsync(Notification notification);
    }
}
