using System.Threading.Tasks;
using Cnblogs.Academy.Common;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    [Headers("X-Project:Academy", "X-ObjectName:ItemDoneRecord")]
    public interface IThumbupApi
    {
        [Post("/api/thumbup")]
        Task<BooleanResult<byte>> UpAsync([Body]ThumbupInput thumbupInput);

        [Delete("/api/thumbup")]
        Task CancelAsync([Body]ThumbCancellationInput thumbCancellationInput);
    }
}
