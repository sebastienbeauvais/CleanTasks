using CleanTasks.Domain.Enums;

namespace CleanTasks.Application.DTOs;

public class UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
    public TaskItemStatus Status { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime? DueDate { get; set; }
}
