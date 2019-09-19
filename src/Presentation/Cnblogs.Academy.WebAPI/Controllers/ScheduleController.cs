using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cnblogs.Academy.Application.Commands;
using Cnblogs.Academy.Application.ItemCommands;
using Cnblogs.Academy.Application.Queries;
using Cnblogs.Academy.Application.ScheduleAppService;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.Commands.ItemCommands;
using Cnblogs.Academy.Commands.ScheduleCommands;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.DTO.InputModel;
using Cnblogs.Academy.ServiceAgent.BlogApi;
using Cnblogs.Domain.Abstract;
using Cnblogs.UCenter.DTO.Users;
using Cnblogs.UCenter.ServiceAgent;
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
        private readonly IBlogApiService _blogApiSvc;

        public ScheduleController(IScheduleService scheduleService,
                                  IUCenterService uCenterService,
                                  IMediator mediator,
                                  IScheduleQueries queries,
                                  IBlogApiService blogApiSvc)
        {
            _scheduleService = scheduleService;
            _uCenterService = uCenterService;
            _mediator = mediator;
            _queries = queries;
            _blogApiSvc = blogApiSvc;
        }

        #region Schedule
        [HttpPost]
        public async Task<BooleanResult> Post([FromBody]ScheduleInputModel input)
        {
            var command = new CreateScheduleCommand(input, UCenterUser);
            return await _mediator.Send(command);
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
            var command = new UpdateScheduleCommand(id, input, UserId);
            await _mediator.Send(command);
        }

        [HttpDelete("{id:long}")]
        public async Task Delete(long id)
        {
            var command = new DeleteScheduleCommand(id, UserId);
            await _mediator.Send(command);
        }

        [HttpGet("mine")]
        public async Task<IEnumerable<MyScheduleDto>> GetMySchedules(bool completed = false, int page = 1, int pageSize = 30)
        {
            var result = await _scheduleService.GetMySchedules(UserId, completed, page - 1, pageSize);
            Request.HttpContext.Response.Headers.Add("x-count", result.count.ToString());
            return result.list;
        }

        [HttpPatch("{scheduleId:long}/dateEnd")]
        public async Task<BooleanResult> CompleteSchedule(long scheduleId)
        {
            var command = new CompleteScheduleCommand(scheduleId, UCenterUser);
            return await _mediator.Send(command);
        }

        [HttpDelete("{scheduleId:long}/dateEnd")]
        public async Task<BooleanResult> CancelComplete(long scheduleId)
        {
            var command = new CancelScheduleCommand(scheduleId, UCenterUser);
            return await _mediator.Send(command);
        }

        // [AllowAnonymous]
        [HttpGet("withItems")]
        public async Task<PagedResults<ScheduleDetailDto>> ListWithItems(bool completed = false, string alias = "",
            int page = 1, int size = 30)
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
            return await _scheduleService.ListWithItemsAsync(user.UserId, hasPrivate, completed, page, size);
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

        [HttpPost("{id:long}/subscription")]
        public async Task<long> Subscription(long id)
        {
            return await _scheduleService.SubscribeAsync(id, UserId);
        }

        [HttpGet("{id:long}/following")]
        public async Task<IEnumerable<ScheduleFollowingDto>> Following(long id)
        {
            return await _queries.GetScheduleFollowings(id);
        }

        [HttpGet("options")]
        public async Task<List<KeyValuePair<long, string>>> Options(int page = 0, int size = 10)
        {
            return await this._queries.GetScheduleOptions(UserId, page, size);
        }

        #endregion

        #region ScheduleItem

        [HttpPost("{id:long}/items/[[markdown]]")]
        public async Task<long> MakrdownItem(long id, [FromBody]ScheduleItemMarkdownInput input)
        {
            var command = new AddItemCommand(id, input, UCenterUser);
            return await _mediator.Send(command);
        }

        [HttpPatch("item/{id:long}/title")]
        public async Task UpdateItemTitle(long id, [FromBody]ScheduleItemMarkdownInput item)
        {
            var command = new UpdateItemTitleCommand(id, item.Title, UserId);
            await _mediator.Send(command);
        }

        [HttpDelete("item/{id:long}")]
        public async Task DeleteItem(long id)
        {
            var command = new DeleteItemCommand(id, UserId);
            await _mediator.Send(command);
        }

        [HttpPost("{id:long}/items/{itemId:long}/todo")]
        [HttpPatch("{id:long}/items/{itemId:long}/completed")]
        public async Task<BooleanResult> CompleteItem(long id, long itemId)
        {
            var command = new CompleteItemCommand(id, itemId, UserId);
            return await _mediator.Send(command);
        }

        [HttpDelete("{scheduleId:long}/items/{itemId:long}/record")]
        [HttpDelete("{scheduleId:long}/items/{itemId:long}/completed")]
        public async Task<BooleanResult> UndoItem(long scheduleId, long itemId)
        {
            var command = new UndoItemCommand(scheduleId, itemId, UserId);
            return await _mediator.Send(command);
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
        #endregion

        #region Summary

        [HttpPost("item/{itemId:long}/summary/note")]
        public async Task<long> AddSummaryNote(long itemId, [FromBody]SummaryNoteInputModel inputModel)
        {
            var command = new AddSummaryNoteCommand(itemId, inputModel.Note, UCenterUser.UserId);
            return await _mediator.Send(command);
        }

        [HttpGet("item/{itemId:long}/summary")]
        public async Task<SummaryDto> GetSummary(long itemId)
        {
            return await _queries.GetSummary(itemId);
        }

        [HttpPut("item/{itemId:long}/summary/note")]
        public async Task UpdateSummaryNote(long itemId, [FromBody]SummaryNoteInputModel inputModel)
        {
            var command = new UpdateSummaryNoteCommand(itemId, inputModel.Id, inputModel.Note, UCenterUser.UserId);
            await _mediator.Send(command);
        }

        [HttpDelete("item/{itemId:long}/summary/note/{noteId:long}")]
        public async Task DeleteSummaryNote(long itemId, long noteId)
        {
            var command = new DeleteSummaryNoteCommand(itemId, noteId, UCenterUser.UserId);
            await _mediator.Send(command);
        }

        [HttpGet("summary/post/links/recent")]
        public async Task<IEnumerable<SummaryLinkDto>> RecentPostLinks(int page = 1, int size = 10)
        {
            if (UCenterUser.BlogId >= 0)
            {
                return await _blogApiSvc.RecentPostLinks(UCenterUser.BlogId, page, size);
            }
            return Array.Empty<SummaryLinkDto>();
        }

        [HttpPost("item/{itemId:long}/summary/links")]
        public async Task<long> SummaryLink(long itemId, [FromBody]SummaryLinkDto linkDto)
        {
            var command = new AddSummaryLinkCommand(itemId, UCenterUser.UserId, linkDto);
            return await _mediator.Send(command);
        }

        [HttpDelete("item/{itemId:long}/summary/links/{linkId:long}")]
        public async Task DeleteSummaryLink(long itemId, long linkId)
        {
            var command = new DeleteSummaryLinkCommand(itemId, linkId, UCenterUser.UserId);
            await _mediator.Send(command);
        }
        #endregion
    }
}
