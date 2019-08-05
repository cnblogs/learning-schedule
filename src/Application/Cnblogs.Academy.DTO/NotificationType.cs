using System;

namespace Cnblogs.Academy.DTO
{
    [Flags]
    public enum NotificationType
    {
        None = 0,
        Mail = 1,
        SiteMessage = 2,
    }
}
