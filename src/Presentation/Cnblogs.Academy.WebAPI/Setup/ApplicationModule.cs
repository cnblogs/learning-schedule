using Autofac;
using Cnblogs.Academy.Application.Queries;

namespace Cnblogs.Academy.WebAPI.Setup
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ScheduleQueries>()
                .As<IScheduleQueries>()
                .InstancePerLifetimeScope();
        }
    }
}
