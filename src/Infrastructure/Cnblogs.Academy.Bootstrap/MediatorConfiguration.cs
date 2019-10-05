using Cnblogs.Academy.Application.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cnblogs.Academy.Bootstrap
{
    public static class MediatorConfiguration
    {
        public static IServiceCollection AddMediatoR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CompleteItemCommand).Assembly);
            return services;
        }
    }
}
