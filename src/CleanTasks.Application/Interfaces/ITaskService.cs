using CleanTasks.Application.DTOs;
using CleanTasks.Domain.Enums;

namespace CleanTasks.Application.Interfaces;

public interface ITaskService
{
    Task<TaskItemDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TaskItemDto>> GetAllAsync();
    Task<IEnumerable<TaskItemDto>> GetByStatusAsync(TaskItemStatus status);
    Task<IEnumerable<TaskItemDto>> GetByPriorityAsync(TaskPriority priority);
    Task<IEnumerable<TaskItemDto>> GetByCategoryAsync(Guid categoryId);
    Task<IEnumerable<TaskItemDto>> GetOverdueAsync();
    Task<TaskItemDto> CreateAsync(CreateTaskRequest request);
    Task<TaskItemDto?> UpdateAsync(Guid id, UpdateTaskRequest request);
    Task<TaskItemDto?> CompleteAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
