using CleanTasks.Domain.Enums;

namespace CleanTasks.Application.DTOs;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime? DueDate { get; set; }
}
