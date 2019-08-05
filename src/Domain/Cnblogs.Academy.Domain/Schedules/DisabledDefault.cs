namespace Cnblogs.Academy.Domain.Schedules
{
    public abstract class DisabledDefault
    {
        public bool Deleted { get; set; }
        public Status Status { get; set; }

        public virtual void Disable()
        {
            Deleted = true;
            Status = Status.Disable;
        }

        public virtual void Restore()
        {
            Deleted = false;
            Status = Status.Normal;
        }
    }
}
