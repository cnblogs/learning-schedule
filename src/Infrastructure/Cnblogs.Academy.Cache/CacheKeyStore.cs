using System;

namespace Cnblogs.Academy.Cache
{
    public class CacheKeyStore
    {
        public static string ScheduleFollowings(long scheduleId) => $"academy_schedule_followings_{scheduleId}";
        public static string HomeFeeds(int page = 1) => $"cnblogs_academy_home_feeds_{page}";
        public static string IndexFeeds(int page, Guid userId, Guid? groupId) => $"academy_index_feeds_{userId}_{page}_{groupId}";
    }
}
