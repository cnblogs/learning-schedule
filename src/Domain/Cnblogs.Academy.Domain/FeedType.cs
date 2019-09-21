namespace Cnblogs.Academy.Domain
{
    public enum FeedType
    {
        /// <summary>
        /// 新建学习目标
        /// </summary>
        ScheduleNew = 20,

        /// <summary>
        /// 新建学习任务
        /// </summary>
        ScheduleItemNew = 21,

        /// <summary>
        /// 完成学习任务
        /// </summary>
        ScheduleItemDone = 22,

        /// <summary>
        /// 完成学习目标
        /// </summary>
        ScheduleCompleted = 23
    }
}
