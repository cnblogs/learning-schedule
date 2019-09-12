using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.ServiceAgent.MarkdownApi;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cnblogs.Academy.Commands.ItemCommands
{
    public class AddItemCommandHandler : IRequestHandler<AddItemCommand, long>
    {
        private readonly IMarkdownApiService _markdownApi;
        private readonly IScheduleRepository _repository;

        public AddItemCommandHandler(IMarkdownApiService markdownApi, IScheduleRepository repository)
        {
            _markdownApi = markdownApi;
            _repository = repository;
        }

        public async Task<long> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            var html = await _markdownApi.ToHtml(request.Model.Title);
            var schedule = await _repository.Schedules.FirstOrDefaultAsync(x => x.Id == request.ScheduleId)
                ?? throw new ValidationException("无效的请求");

            var item = ScheduleItem.CreateMarkdownItem(request.ScheduleId, request.Model.Title, request.User.UserId, html);
            schedule.AddItem(item);
            await _repository.UnitOfWork.SaveEntitiesAsync();
            return item.Id;
        }
    }
}
