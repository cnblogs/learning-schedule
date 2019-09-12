using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Commands.ItemCommands
{
    public class UpdateItemTitleCommandHandler : IRequestHandler<UpdateItemTitleCommand, bool>
    {
        private readonly IScheduleRepository _repository;
        private readonly IMarkdownApiService _markdownApi;

        public UpdateItemTitleCommandHandler(IScheduleRepository repository, IMarkdownApiService markdownApi)
        {
            _repository = repository;
            _markdownApi = markdownApi;
        }

        public async Task<bool> Handle(UpdateItemTitleCommand request, CancellationToken cancellationToken)
        {
            var html = await _markdownApi.ToHtml(request.Title);
            var item = await _repository.ScheduleItems.Include(i => i.Html)
                                        .FirstOrDefaultAsync(i => i.Id == request.Id && i.UserId == request.UserId)
                                        ?? throw new ValidationException("非法的修改请求");

            item.UpdateTitle(request.Title, TextType.Markdown, html, request.UserId);
            await _repository.UnitOfWork.SaveEntitiesAsync();
            return true;
        }
    }
}
