using System.Linq;
using System.Threading.Tasks;
using Cnblogs.Academy.Domain;
using Cnblogs.Academy.Domain.Events;
using DotNetCore.CAP;

namespace Cnblogs.Academy.Repositories
{
    public static class CapPublisherExtensions
    {
        public static async Task DispatchDomianEventAsync(this ICapPublisher bus, AcademyContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                           .Entries<Entity>()
                           .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(domainEvent => bus.PublishAsync(domainEvent.GetEventName(), domainEvent));

            await Task.WhenAll(tasks);
        }
    }
}
