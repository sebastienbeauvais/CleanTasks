using CleanTasks.Domain.Entities;
using CleanTasks.Domain.Enums;
using CleanTasks.Infrastructure.Repositories;

namespace CleanTasks.Tests.Infrastructure;

public class InMemoryTaskRepositoryTests
{
    private readonly InMemoryTaskRepository _repository;

    public InMemoryTaskRepositoryTests()
    {
        _repository = new InMemoryTaskRepository();
    }

    [Fact]
    public async Task AddAsync_ShouldStoreTask()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Test Task",
            Description = "A test",
            Priority = TaskPriority.Medium,
            Status = TaskItemStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _repository.AddAsync(task);

        Assert.NotNull(result);
        Assert.Equal(task.Id, result.Id);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnTask()
    {
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "Find Me" };
        await _repository.AddAsync(task);

        var result = await _repository.GetByIdAsync(task.Id);

        Assert.NotNull(result);
        Assert.Equal("Find Me", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "A" });
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "B" });

        var result = (await _repository.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnMatchingTasks()
    {
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Pending", Status = TaskItemStatus.Pending });
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Done", Status = TaskItemStatus.Completed });
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Also Pending", Status = TaskItemStatus.Pending });

        var result = (await _repository.GetByStatusAsync(TaskItemStatus.Pending)).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(TaskItemStatus.Pending, t.Status));
    }

    [Fact]
    public async Task GetByPriorityAsync_ShouldReturnMatchingTasks()
    {
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "High", Priority = TaskPriority.High });
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Low", Priority = TaskPriority.Low });
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Also High", Priority = TaskPriority.High });

        var result = (await _repository.GetByPriorityAsync(TaskPriority.High)).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(TaskPriority.High, t.Priority));
    }

    [Fact]
    public async Task GetByCategoryIdAsync_ShouldReturnMatchingTasks()
    {
        var categoryId = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Cat1", CategoryId = categoryId });
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Cat2", CategoryId = otherId });
        await _repository.AddAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Cat1 Again", CategoryId = categoryId });

        var result = (await _repository.GetByCategoryIdAsync(categoryId)).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(categoryId, t.CategoryId));
    }

    [Fact]
    public async Task GetOverdueAsync_ShouldReturnOverdueNonCompletedTasks()
    {
        var now = DateTime.UtcNow;
        await _repository.AddAsync(new TaskItem
        {
            Id = Guid.NewGuid(), Title = "Overdue",
            DueDate = now.AddDays(-1), Status = TaskItemStatus.Pending
        });
        await _repository.AddAsync(new TaskItem
        {
            Id = Guid.NewGuid(), Title = "Overdue but Done",
            DueDate = now.AddDays(-1), Status = TaskItemStatus.Completed
        });
        await _repository.AddAsync(new TaskItem
        {
            Id = Guid.NewGuid(), Title = "Future",
            DueDate = now.AddDays(5), Status = TaskItemStatus.Pending
        });
        await _repository.AddAsync(new TaskItem
        {
            Id = Guid.NewGuid(), Title = "No Due Date",
            DueDate = null, Status = TaskItemStatus.Pending
        });

        var result = (await _repository.GetOverdueAsync(now)).ToList();

        Assert.Single(result);
        Assert.Equal("Overdue", result[0].Title);
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateAndReturn()
    {
        var id = Guid.NewGuid();
        await _repository.AddAsync(new TaskItem { Id = id, Title = "Old", Priority = TaskPriority.Low });

        var updated = new TaskItem { Id = id, Title = "New", Priority = TaskPriority.High };
        var result = await _repository.UpdateAsync(updated);

        Assert.NotNull(result);
        Assert.Equal("New", result.Title);
        Assert.Equal(TaskPriority.High, result.Priority);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldReturnNull()
    {
        var result = await _repository.UpdateAsync(new TaskItem { Id = Guid.NewGuid(), Title = "Nope" });
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldReturnTrueAndRemove()
    {
        var id = Guid.NewGuid();
        await _repository.AddAsync(new TaskItem { Id = id, Title = "Delete Me" });

        var result = await _repository.DeleteAsync(id);

        Assert.True(result);
        Assert.Null(await _repository.GetByIdAsync(id));
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_ShouldReturnFalse()
    {
        var result = await _repository.DeleteAsync(Guid.NewGuid());
        Assert.False(result);
    }
}
