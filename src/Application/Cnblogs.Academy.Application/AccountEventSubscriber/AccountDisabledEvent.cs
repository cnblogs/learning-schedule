using System;

namespace Cnblogs.Academy.Application.AccountEventSubscriber
{
    public class AccountDisabledEvent
    {
        public Guid UserId { get; set; }
    }

    public class AccountActivatedEvent : AccountDisabledEvent
    {
    }
}
