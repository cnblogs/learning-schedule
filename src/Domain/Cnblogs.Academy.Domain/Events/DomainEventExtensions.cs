using System;
using System.ComponentModel;

namespace Cnblogs.Academy.Domain.Events
{
    public static class DomainEventExtensions
    {
        public static string GetEventName(this IDomainEvent e)
        {
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(e.GetType(), typeof(DescriptionAttribute));
            return attribute?.Description;
        }
    }
}
