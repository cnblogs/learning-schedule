using System;

namespace Cnblogs.Academy.Domain.Events
{
    public class EventNameAttribute : Attribute
    {
        public EventNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
