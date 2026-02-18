using CleanTasks.Domain.Entities;
using CleanTasks.Domain.Enums;
using CleanTasks.Domain.Interfaces;

namespace CleanTasks.Infrastructure.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    // TODO: Add a thread-safe collection to store tasks

    public Task<TaskItem?> GetByIdAsync(Guid id)
    {
        // TODO: Implement - return task matching id, or null
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        // TODO: Implement - return all stored tasks
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskItemStatus status)
    {
        // TODO: Implement - return tasks matching the given status
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItem>> GetByPriorityAsync(TaskPriority priority)
    {
        // TODO: Implement - return tasks matching the given priority
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItem>> GetByCategoryIdAsync(Guid categoryId)
    {
        // TODO: Implement - return tasks belonging to the given category
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItem>> GetOverdueAsync(DateTime referenceDate)
    {
        // TODO: Implement - return tasks with DueDate before referenceDate and status != Completed
        throw new NotImplementedException();
    }

    public Task<TaskItem> AddAsync(TaskItem task)
    {
        // TODO: Implement - store the task and return it
        throw new NotImplementedException();
    }

    public Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        // TODO: Implement - update existing task if found, return updated or null
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        // TODO: Implement - remove task by id, return true if found and removed
        throw new NotImplementedException();
    }
}
