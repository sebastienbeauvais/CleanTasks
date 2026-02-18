using CleanTasks.Application.DTOs;
using CleanTasks.Application.Interfaces;
using CleanTasks.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CleanTasks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAll()
    {
        // TODO: Implement - return all tasks
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
    {
        // TODO: Implement - return task by id, or 404
        throw new NotImplementedException();
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetByStatus(TaskItemStatus status)
    {
        // TODO: Implement - return tasks filtered by status
        throw new NotImplementedException();
    }

    [HttpGet("priority/{priority}")]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetByPriority(TaskPriority priority)
    {
        // TODO: Implement - return tasks filtered by priority
        throw new NotImplementedException();
    }

    [HttpGet("category/{categoryId:guid}")]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetByCategory(Guid categoryId)
    {
        // TODO: Implement - return tasks filtered by category
        throw new NotImplementedException();
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetOverdue()
    {
        // TODO: Implement - return tasks that are past their due date
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> Create([FromBody] CreateTaskRequest request)
    {
        // TODO: Implement - create task, return 201 with location header
        throw new NotImplementedException();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaskItemDto>> Update(Guid id, [FromBody] UpdateTaskRequest request)
    {
        // TODO: Implement - update task, return updated or 404
        throw new NotImplementedException();
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<ActionResult<TaskItemDto>> Complete(Guid id)
    {
        // TODO: Implement - mark task as completed, return updated or 404
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        // TODO: Implement - delete task, return 204 or 404
        throw new NotImplementedException();
    }
}
