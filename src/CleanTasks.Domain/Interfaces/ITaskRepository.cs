using CleanTasks.Domain.Entities;
using CleanTasks.Domain.Enums;

namespace CleanTasks.Domain.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskItemStatus status);
    Task<IEnumerable<TaskItem>> GetByPriorityAsync(TaskPriority priority);
    Task<IEnumerable<TaskItem>> GetByCategoryIdAsync(Guid categoryId);
    Task<IEnumerable<TaskItem>> GetOverdueAsync(DateTime referenceDate);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem?> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(Guid id);
}
