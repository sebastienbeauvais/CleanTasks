using CleanTasks.Domain.Interfaces;
using CleanTasks.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTasks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
        services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
        return services;
    }
}
