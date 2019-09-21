using System.Threading.Tasks;
using Cnblogs.Academy.DTO;

namespace Cnblogs.Academy.ServiceAgent.FeedService
{
    public interface IFeedServiceAgent
    {
        Task PublishAsync(FeedInputModel feedInputModel);
    }
}