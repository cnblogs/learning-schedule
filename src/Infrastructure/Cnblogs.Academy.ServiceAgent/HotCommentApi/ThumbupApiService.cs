using System.Threading.Tasks;
using Cnblogs.Academy.Common;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public class ThumbupApiService : IThumbupApiService
    {
        private readonly IThumbupApi _thumbupApi;

        public ThumbupApiService(IThumbupApi thumbupApi)
        {
            _thumbupApi = thumbupApi;
        }

        public async Task CancelAsync(ThumbCancellationInput thumbCancellationInput)
        {
            await _thumbupApi.CancelAsync(thumbCancellationInput);
        }

        public async Task<BooleanResult<byte>> UpAsync(ThumbupInput thumbupInput)
        {
            return await _thumbupApi.UpAsync(thumbupInput);
        }
    }
}
