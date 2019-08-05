using Cnblogs.Academy.Domain.Schedules;
using Cnblogs.Academy.DTO;
using Mapster;

namespace Cnblogs.Academy.WebAPI
{
    internal static class MapperConfig
    {
        internal static void Init()
        {
            TypeAdapterConfig<ScheduleItem, ScheduleItemDto>
                .NewConfig()
                .Map(x => x.Html, src => src.Html.Html);

            TypeAdapterConfig<ScheduleItem, ScheduleItemDetailDto>
                .NewConfig()
                .Map(x => x.Html, src => src.Html.Html);
        }
    }
}
