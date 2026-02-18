using CleanTasks.Application.Interfaces;
using CleanTasks.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTasks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}
