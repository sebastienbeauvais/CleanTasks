using CleanTasks.Application.DTOs;
using CleanTasks.Application.Interfaces;
using CleanTasks.Domain.Enums;
using CleanTasks.Domain.Interfaces;

namespace CleanTasks.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
    }

    public Task<TaskItemDto?> GetByIdAsync(Guid id)
    {
        // TODO: Implement - retrieve task by id and map to DTO
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItemDto>> GetAllAsync()
    {
        // TODO: Implement - retrieve all tasks and map to DTOs
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItemDto>> GetByStatusAsync(TaskItemStatus status)
    {
        // TODO: Implement - retrieve tasks by status and map to DTOs
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItemDto>> GetByPriorityAsync(TaskPriority priority)
    {
        // TODO: Implement - retrieve tasks by priority and map to DTOs
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItemDto>> GetByCategoryAsync(Guid categoryId)
    {
        // TODO: Implement - retrieve tasks by category and map to DTOs
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TaskItemDto>> GetOverdueAsync()
    {
        // TODO: Implement - retrieve tasks past due date that are not completed
        throw new NotImplementedException();
    }

    public Task<TaskItemDto> CreateAsync(CreateTaskRequest request)
    {
        // TODO: Implement - validate request, create entity, persist, return DTO
        throw new NotImplementedException();
    }

    public Task<TaskItemDto?> UpdateAsync(Guid id, UpdateTaskRequest request)
    {
        // TODO: Implement - find existing, validate, update fields, persist, return DTO
        throw new NotImplementedException();
    }

    public Task<TaskItemDto?> CompleteAsync(Guid id)
    {
        // TODO: Implement - find task, set status to Completed, persist, return DTO
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        // TODO: Implement - delete task by id
        throw new NotImplementedException();
    }
}
