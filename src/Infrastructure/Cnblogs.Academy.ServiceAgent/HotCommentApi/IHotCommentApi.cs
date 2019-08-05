using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    [Headers("X-Project:Academy", "X-ObjectName:ItemDoneRecord")]
    public interface IHotCommentApi
    {
        [Post("/api/comment")]
        Task<Guid> PublishAsync(CommentInput input);

        [Get("/api/comment/{objectId}")]
        Task<IEnumerable<CommentItem>> ListAsync(string objectId, Guid? parentId);

        [Delete("/api/comment/{id}")]
        Task DeleteAsync(Guid id, Guid userId);

        [Get("/api/comment/item/{id}")]
        Task<CommentItem> FetchCommentAsync(Guid id);
    }
}
