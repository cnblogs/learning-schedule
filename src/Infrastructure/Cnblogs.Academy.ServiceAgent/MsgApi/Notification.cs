namespace Cnblogs.Academy.ServiceAgent.MsgApi
{
    public class Notification
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 收件人ID
        /// </summary>
        public int RecipientId { get; set; }

        // public Guid? RecipientGuid { get; set; }
        /// <summary>
        /// 是否重要通知(会使用不同邮件服务器)
        /// </summary>
        public bool IsImportant { get; set; } = false;
        /// <summary>
        /// 通知类型
        /// </summary>
        // public NotificationType NotificationType { get; set; } = NotificationType.None;

        public string Ip { get; set; }
    }
}
