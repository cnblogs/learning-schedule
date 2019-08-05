using System.Reflection;
using Autofac;
using Cnblogs.Academy.Application.Commands;
using MediatR;

namespace Cnblogs.Academy.WebAPI.Setup
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterAssemblyTypes(typeof(CreateSubtaskCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
        }
    }
}
