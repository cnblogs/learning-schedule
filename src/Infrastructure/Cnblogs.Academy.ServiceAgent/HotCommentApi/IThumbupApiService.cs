using System.Threading.Tasks;
using Cnblogs.Academy.Common;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public interface IThumbupApiService
    {
        Task<BooleanResult<byte>> UpAsync(ThumbupInput thumbupInput);
        Task CancelAsync(ThumbCancellationInput thumbCancellationInput);
    }
}
