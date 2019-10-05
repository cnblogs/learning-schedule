using System;

namespace Cnblogs.Academy.Domain.Events
{
    public static class DomainEventExtensions
    {
        public static string GetEventName(this IDomainEvent e)
        {
            var attribute = (EventNameAttribute)Attribute.GetCustomAttribute(e.GetType(), typeof(EventNameAttribute));
            return attribute?.Name;
        }
    }
}
