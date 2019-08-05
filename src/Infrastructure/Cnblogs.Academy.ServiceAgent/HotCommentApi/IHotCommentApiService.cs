using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public interface IHotCommentApiService
    {
        Task<Guid> PublishAsync(CommentInput input);
        Task<IEnumerable<CommentItem>> ListAsync(string objectId, Guid? parentId);
        Task DeleteAsync(Guid id, Guid userId);
        Task<CommentItem> FetchCommentAsync(Guid id);
    }
}
