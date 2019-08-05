using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cnblogs.Academy.ServiceAgent.HotCommentApi
{
    public class HotCommentApiService : IHotCommentApiService
    {
        private readonly IHotCommentApi _hotCommentApi;

        public HotCommentApiService(IHotCommentApi hotCommentApi)
        {
            _hotCommentApi = hotCommentApi;
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            await _hotCommentApi.DeleteAsync(id, userId);
        }

        public async Task<CommentItem> FetchCommentAsync(Guid id)
        {
            return await _hotCommentApi.FetchCommentAsync(id);
        }

        public async Task<IEnumerable<CommentItem>> ListAsync(string objectId, Guid? parentId)
        {
            return await _hotCommentApi.ListAsync(objectId, parentId);
        }

        public async Task<Guid> PublishAsync(CommentInput input)
        {
            return await _hotCommentApi.PublishAsync(input);
        }
    }
}
