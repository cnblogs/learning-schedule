using System.Linq;
using Cnblogs.Academy.Application.CourseMallAppService.Dto;
using Cnblogs.Academy.Application.ProviderAppService;
using Cnblogs.Academy.Application.ScheduleAppService.Dto;
using Cnblogs.Academy.Domain.Books;
using Cnblogs.Academy.Domain.CourseMall;
using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.Dto;
using Cnblogs.Academy.DTO;
using Cnblogs.Academy.DTO.Books;
using Cnblogs.UCenter.DTO.Users;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.Bootstrap
{
    public static class MapperConfig
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            TypeAdapterConfig<ScheduleItem, ScheduleItemDto>
                    .NewConfig()
                    .Map(x => x.Html, src => src.Html.Html);

            TypeAdapterConfig<ScheduleItem, ScheduleItemDetailDto>
                    .NewConfig()
                    .Map(x => x.Html, src => src.Html.Html);

            TypeAdapterConfig<Schedule, ScheduleIntroDto>
                    .NewConfig()
                    .Ignore(x => x.Alias)
                    .Ignore(x => x.UserName);

            TypeAdapterConfig<ScheduleItem, SummaryDto>
                    .NewConfig()
                    .Map(x => x.ItemId, src => src.Id)
                    .Map(x => x.Note, src => src.SummaryNote)
                    .Map(x => x.Links, src => src.SummaryLinks);

            TypeAdapterConfig<ServiceAgent.BlogApi.PostDto, SummaryLinkDto>
                    .NewConfig()
                    .Map(x => x.PostId, src => src.Id)
                    .Map(x => x.Link, src => src.Url)
                    .Ignore(x => x.Id);

            return services;
        }
    }
}
