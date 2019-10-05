using System.Threading.Tasks;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.ServiceAgent.FeedsApi;

namespace Cnblogs.Academy.ServiceAgent.FeedService
{
    public interface IFeedServiceAgent
    {
        Task PublishAsync(FeedInputModel feedInputModel);
        Task DeleteAsync(FeedDeletedInput model);
        Task UpdateAsync(FeedUpdateModel model);
    }

    public class FeedServiceAgent : IFeedServiceAgent
    {
        private readonly IFeedsApi _api;

        public FeedServiceAgent(IFeedsApi api)
        {
            _api = api;
        }

        public async Task DeleteAsync(FeedDeletedInput model)
        {
            await _api.Delete(model);
        }

        public async Task PublishAsync(FeedInputModel feedInputModel)
        {
            await _api.Publish(feedInputModel);
        }

        public async Task UpdateAsync(FeedUpdateModel model)
        {
            await _api.Update(model);
        }
    }
}
