using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.Commands;
using Cnblogs.Academy.Application.Queries;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.Application.ScheduleAppService.InputModel;
using Cnblogs.Academy.Common;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.DTO.InputModel;
using Cnblogs.Academy.ServiceAgent.UCenterService;
using Cnblogs.Domain.Abstract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cnblogs.Academy.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Route("api/schedules")]
    public class ScheduleController : AcademyControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IUCenterService _uCenterService;
        private readonly IMediator _mediator;
        private readonly IScheduleQueries _queries;

        public ScheduleController(IScheduleService scheduleService, IUCenterService uCenterService, IMediator mediator, IScheduleQueries queries)
        {
            _scheduleService = scheduleService;
            _uCenterService = uCenterService;
            _mediator = mediator;
            _queries = queries;
        }

        [HttpPost]
        public async Task<BooleanResult> Post([FromBody]ScheduleInputModel input)
        {
            return await _scheduleService.AddAsync(input, UCenterUser);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<ScheduleDto>> Get(Stage? stage, int page = 1, int size = 10)
        {
            return await _scheduleService.GetRecentScheduleAsync(stage, page, size);
        }

        [HttpPatch("{id:long}")]
        public async Task Patch(long id, [FromBody]ScheduleInputModel input)
        {
            await _scheduleService.UpdateScheduleAsync(id, input, UserId);
        }

        [HttpDelete("{id:long}")]
        public async Task Delete(long id)
        {
            await _scheduleService.DeleteScheduleAsync(id, UserId);
        }

        [HttpPost("{id:long}/items/[[markdown]]")]
        public async Task<long> MakrdownItem(long id, [FromBody]ScheduleItemMarkdownInput input)
        {
            return await _scheduleService.AddItem(id, input, UCenterUser);
        }

        [HttpPatch("item/{id:long}/title")]
        public async Task UpdateItemTitle(long id, [FromBody]ScheduleItemMarkdownInput item)
        {
            await _scheduleService.UpdateItemTitleWithMarkdown(id, item.Title, UserId);
        }

        [HttpDelete("item/{id:long}")]
        public async Task DeleteItem(long id)
        {
            await _scheduleService.DeleteItem(id, UserId);
        }

        [HttpPost("{id:long}/items/{itemId:long}/todo")]
        public async Task<BooleanResult> ToDo(long id, long itemId, [FromBody]ItemDoneRecordInputModel im)
        {
            var command = new ToDoItemCommand(id, itemId, UCenterUser);
            return await _mediator.Send(command);
        }

        [HttpPost("{id:long}/following")]
        public async Task<BooleanResult> Following(long id)
        {
            return await _scheduleService.AddFollowingAsync(id, UserId);
        }

        [HttpGet("mine")]
        public async Task<IEnumerable<MyScheduleDto>> GetMySchedules(bool completed = false, int page = 1, int pageSize = 30)
        {
            var result = await _scheduleService.GetMySchedules(UserId, completed, page - 1, pageSize);
            Request.HttpContext.Response.Headers.Add("x-count", result.count.ToString());
            return result.list;
        }

        [HttpDelete("{scheduleId:long}/items/{itemId:long}/record")]
        public async Task<BooleanResult> UndoItem(long scheduleId, long itemId)
        {
            var command = new UndoItemCommand(scheduleId, itemId, UserId);
            return await _mediator.Send(command);
        }

        [HttpPatch("{scheduleId:long}/dateEnd")]
        public async Task<BooleanResult> CompleteSchedule(long scheduleId)
        {
            return await _scheduleService.CompleteSchedule(scheduleId, UCenterUser);
        }

        [HttpDelete("{scheduleId:long}/dateEnd")]
        public async Task<BooleanResult> CancelComplete(long scheduleId)
        {
            return await _scheduleService.CompleteSchedule(scheduleId, UCenterUser, cancel: true);
        }

        // [AllowAnonymous]
        [HttpGet("withItems")]
        public async Task<PagedResults<ScheduleDetailDto>> ListWithItems(bool completed = false, string alias = "",
            bool teachOnly = false, int page = 1, int size = 30)
        {
            UserDto user;
            if (string.IsNullOrEmpty(alias))
            {
                user = UCenterUser;
            }
            else
            {
                user = await _uCenterService.GetUser(x => x.Alias, alias);
            }
            if (user == null) return PagedResults<ScheduleDetailDto>.Empty();

            var hasPrivate = false;
            if (IsAuthenticated)
            {
                hasPrivate = user.UserId == UCenterUser.UserId;
            }
            return await _scheduleService.ListWithItemsAsync(user.UserId, hasPrivate, completed, teachOnly, page, size);
        }

        // [AllowAnonymous]
        [HttpGet("{scheduleId}/detail")]
        public async Task<ScheduleDetailDto> Detail(long scheduleId, string alias)
        {
            var user = await _uCenterService.GetUser(x => x.Alias, alias);
            if (user == null) return null;

            var isOwner = false;
            if (IsAuthenticated)
            {
                isOwner = user.UserId == UCenterUser.UserId;
            }
            return await _scheduleService.GetScheduleDetailAsync(scheduleId, isOwner);
        }

        [HttpGet("{scheduleId:long}/private/record")]
        public async Task<DateTimeOffset?> LastUpdatePrivateRecord(long scheduleId)
        {
            return await _scheduleService.LastPrivateUpdateTime(scheduleId);
        }

        [HttpPut("{scheduleId:long}/private")]
        public async Task UpdatePrivate(long scheduleId, bool to)
        {
            await _scheduleService.UpdatePrivateAsync(scheduleId, to, UserId);
        }

        [AllowAnonymous]
        [HttpGet("item/{itemId:long}/detail")]
        public async Task<ScheduleItemDetailDto> ScheduleItemDetail(long itemId)
        {
            var loginUserId = Guid.Empty;
            if (IsAuthenticated)
            {
                loginUserId = UserId;
            }
            return await _queries.GetScheduleItemDetailAsync(itemId, loginUserId);
        }

        [HttpPost("item/{itemId:long}/subtasks")]
        public async Task<long> ScheduleItemSubTasks(long itemId, [FromBody]SubTaskInputModel im)
        {
            var command = new CreateSubtaskCommand(itemId, im.Content, UserId);
            return await _mediator.Send(command);
        }

        [HttpPut("item/{itemId:long}/subtasks/{subtaskId:long}")]
        public async Task AccomplishSubtask(long itemId, long subtaskId, bool completed = true)
        {
            var command = new AccomplishSubtaskCommand(itemId, subtaskId, UserId, completed);
            await _mediator.Send(command);
        }

        [HttpPut("item/{itemId:long}/subtasks/{subtaskId:long}/content")]
        public async Task UpdateSubtask(long itemId, long subtaskId, [FromBody]SubTaskInputModel im)
        {
            var command = new UpdateSubtaskCommand(itemId, subtaskId, UserId, im.Content);
            await _mediator.Send(command);
        }

        [HttpDelete("item/subtasks/{subtaskId:long}")]
        public async Task DeleteSubtask(long subtaskId)
        {
            var command = new DeleteSubtaskCommand(subtaskId, UserId);
            await _mediator.Send(command);
        }

        [HttpPost("item/{itemId:long}/references")]
        public async Task<long> ScheduleItemReferences(long itemId, [FromBody]ReferenceInputModel im)
        {
            var command = new CreateReferenceCommand(itemId, im.Url, UserId);
            return await _mediator.Send(command);
        }

        [HttpDelete("item/references/{refId:long}")]
        public async Task DeleteReference(long refId)
        {
            var command = new DeleteReferenceCommand(refId, UserId);
            await _mediator.Send(command);
        }

        [HttpPut("item/{itemId:long}/references/{refId:long}/url")]
        public async Task UpdateReference(long itemId, long refId, [FromBody]ReferenceInputModel im)
        {
            var command = new UpdateReferenceCommand(itemId, refId, im.Url, UserId);
            await _mediator.Send(command);
        }

        [HttpPut("item/feedback")]
        public async Task<long> PutFeedback(FeedbackInputModel im)
        {
            var command = new PutFeedbackCommand(im.Id, im.ItemId, im.Content, im.Difficulty, UserId);
            return await _mediator.Send(command);
        }
    }
}
