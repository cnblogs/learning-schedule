using System.Linq;
using System.Threading.Tasks;

namespace Cnblogs.Academy.ServiceAgent.MsgApi
{
    public class MsgApiService : IMsgApiService
    {
        private IMsgApi _api;
        public MsgApiService(IMsgApi api)
        {
            _api = api;
        }

        public async Task NotifyAsync(Notification notification)
        {
            await _api.Notify(notification);
        }

        public async Task<int> UnreadCount(int spaceUserId)
        {
            var response = await _api.UnreadCount(spaceUserId);
            if (response.IsSuccessStatusCode)
            {
                if (int.TryParse(response.Headers?.GetValues("X-COUNT").FirstOrDefault(), out var unreadCount))
                {
                    return unreadCount;
                }
            }
            return 0;
        }
    }
}
