namespace Cnblogs.Academy.WebAPI.Model
{
    public class NotificationEvent
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
        
        public string Ip { get; set; }
    }
}
