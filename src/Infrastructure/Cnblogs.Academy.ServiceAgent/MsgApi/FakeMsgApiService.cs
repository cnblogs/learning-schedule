using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cnblogs.Academy.ServiceAgent.MsgApi
{
    public class FakeMsgApiService : IMsgApiService
    {
        private readonly ILogger<FakeMsgApiService> _logger;
        public FakeMsgApiService(ILogger<FakeMsgApiService> logger)
        {
            _logger = logger;
        }

        public Task NotifyAsync(Notification notification)
        {
            _logger.LogInformation(notification.Content);
            return Task.CompletedTask;
        }

        public Task<int> UnreadCount(int spaceUserId)
        {
            var count = new Random().Next(0, 100);
            _logger.LogInformation($"Get user ({spaceUserId}) unread msg count:{count}");
            return Task.FromResult(count);
        }
    }
}
