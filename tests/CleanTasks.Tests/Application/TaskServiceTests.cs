using CleanTasks.Application.DTOs;
using CleanTasks.Application.Services;
using CleanTasks.Domain.Entities;
using CleanTasks.Domain.Enums;
using CleanTasks.Domain.Interfaces;
using Moq;

namespace CleanTasks.Tests.Application;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepo;
    private readonly Mock<ICategoryRepository> _mockCategoryRepo;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _mockTaskRepo = new Mock<ITaskRepository>();
        _mockCategoryRepo = new Mock<ICategoryRepository>();
        _service = new TaskService(_mockTaskRepo.Object, _mockCategoryRepo.Object);
    }

    // ── GetByIdAsync ──

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnDto()
    {
        var id = Guid.NewGuid();
        _mockTaskRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new TaskItem
            {
                Id = id,
                Title = "Test",
                Description = "Desc",
                Priority = TaskPriority.High,
                Status = TaskItemStatus.Pending,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Test", result.Title);
        Assert.Equal(TaskPriority.High, result.Priority);
        Assert.Equal(TaskItemStatus.Pending, result.Status);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        _mockTaskRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TaskItem?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    // ── GetAllAsync ──

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAsDtos()
    {
        _mockTaskRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<TaskItem>
            {
                new() { Id = Guid.NewGuid(), Title = "A" },
                new() { Id = Guid.NewGuid(), Title = "B" }
            });

        var result = (await _service.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
    }

    // ── GetByStatusAsync ──

    [Fact]
    public async Task GetByStatusAsync_ShouldDelegateToRepository()
    {
        _mockTaskRepo.Setup(r => r.GetByStatusAsync(TaskItemStatus.Completed))
            .ReturnsAsync(new List<TaskItem>
            {
                new() { Id = Guid.NewGuid(), Title = "Done", Status = TaskItemStatus.Completed }
            });

        var result = (await _service.GetByStatusAsync(TaskItemStatus.Completed)).ToList();

        Assert.Single(result);
        Assert.Equal(TaskItemStatus.Completed, result[0].Status);
    }

    // ── GetByPriorityAsync ──

    [Fact]
    public async Task GetByPriorityAsync_ShouldDelegateToRepository()
    {
        _mockTaskRepo.Setup(r => r.GetByPriorityAsync(TaskPriority.Critical))
            .ReturnsAsync(new List<TaskItem>
            {
                new() { Id = Guid.NewGuid(), Title = "Urgent", Priority = TaskPriority.Critical }
            });

        var result = (await _service.GetByPriorityAsync(TaskPriority.Critical)).ToList();

        Assert.Single(result);
        Assert.Equal(TaskPriority.Critical, result[0].Priority);
    }

    // ── GetByCategoryAsync ──

    [Fact]
    public async Task GetByCategoryAsync_ShouldDelegateToRepository()
    {
        var catId = Guid.NewGuid();
        _mockTaskRepo.Setup(r => r.GetByCategoryIdAsync(catId))
            .ReturnsAsync(new List<TaskItem>
            {
                new() { Id = Guid.NewGuid(), Title = "CatTask", CategoryId = catId }
            });

        var result = (await _service.GetByCategoryAsync(catId)).ToList();

        Assert.Single(result);
        Assert.Equal(catId, result[0].CategoryId);
    }

    // ── GetOverdueAsync ──

    [Fact]
    public async Task GetOverdueAsync_ShouldDelegateToRepositoryWithCurrentDate()
    {
        _mockTaskRepo.Setup(r => r.GetOverdueAsync(It.IsAny<DateTime>()))
            .ReturnsAsync(new List<TaskItem>
            {
                new() { Id = Guid.NewGuid(), Title = "Late", DueDate = DateTime.UtcNow.AddDays(-1) }
            });

        var result = (await _service.GetOverdueAsync()).ToList();

        Assert.Single(result);
        _mockTaskRepo.Verify(r => r.GetOverdueAsync(It.IsAny<DateTime>()), Times.Once);
    }

    // ── CreateAsync ──

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldCreateAndReturnDto()
    {
        _mockTaskRepo.Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        var request = new CreateTaskRequest
        {
            Title = "New Task",
            Description = "Do this",
            Priority = TaskPriority.Medium,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        var result = await _service.CreateAsync(request);

        Assert.NotNull(result);
        Assert.Equal("New Task", result.Title);
        Assert.Equal("Do this", result.Description);
        Assert.Equal(TaskPriority.Medium, result.Priority);
        Assert.Equal(TaskItemStatus.Pending, result.Status);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotEqual(default, result.CreatedAt);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyTitle_ShouldThrowArgumentException()
    {
        var request = new CreateTaskRequest { Title = "", Description = "No title" };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_WithWhitespaceTitle_ShouldThrowArgumentException()
    {
        var request = new CreateTaskRequest { Title = "   " };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_WithCategoryId_WhenCategoryExists_ShouldSucceed()
    {
        var catId = Guid.NewGuid();
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(catId))
            .ReturnsAsync(new Category { Id = catId, Name = "Work" });
        _mockTaskRepo.Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        var request = new CreateTaskRequest
        {
            Title = "Categorized Task",
            CategoryId = catId
        };

        var result = await _service.CreateAsync(request);

        Assert.Equal(catId, result.CategoryId);
    }

    [Fact]
    public async Task CreateAsync_WithCategoryId_WhenCategoryNotExists_ShouldThrowArgumentException()
    {
        var catId = Guid.NewGuid();
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(catId))
            .ReturnsAsync((Category?)null);

        var request = new CreateTaskRequest
        {
            Title = "Bad Category",
            CategoryId = catId
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_WithNullCategoryId_ShouldNotValidateCategory()
    {
        _mockTaskRepo.Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        var request = new CreateTaskRequest
        {
            Title = "No Category",
            CategoryId = null
        };

        var result = await _service.CreateAsync(request);

        Assert.Null(result.CategoryId);
        _mockCategoryRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    // ── UpdateAsync ──

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateAndReturnDto()
    {
        var id = Guid.NewGuid();
        _mockTaskRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new TaskItem { Id = id, Title = "Old", Status = TaskItemStatus.Pending });
        _mockTaskRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        var request = new UpdateTaskRequest
        {
            Title = "Updated",
            Description = "New desc",
            Priority = TaskPriority.High,
            Status = TaskItemStatus.InProgress
        };

        var result = await _service.UpdateAsync(id, request);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Title);
        Assert.Equal(TaskPriority.High, result.Priority);
        Assert.Equal(TaskItemStatus.InProgress, result.Status);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldReturnNull()
    {
        _mockTaskRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TaskItem?)null);

        var result = await _service.UpdateAsync(Guid.NewGuid(), new UpdateTaskRequest { Title = "X" });

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WithEmptyTitle_ShouldThrowArgumentException()
    {
        var id = Guid.NewGuid();
        _mockTaskRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new TaskItem { Id = id, Title = "Exists" });

        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UpdateAsync(id, new UpdateTaskRequest { Title = "" }));
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidCategoryId_ShouldThrowArgumentException()
    {
        var id = Guid.NewGuid();
        var badCatId = Guid.NewGuid();
        _mockTaskRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new TaskItem { Id = id, Title = "Exists" });
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(badCatId))
            .ReturnsAsync((Category?)null);

        var request = new UpdateTaskRequest { Title = "Valid", CategoryId = badCatId };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(id, request));
    }

    // ── CompleteAsync ──

    [Fact]
    public async Task CompleteAsync_WhenExists_ShouldSetStatusToCompletedAndReturnDto()
    {
        var id = Guid.NewGuid();
        _mockTaskRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new TaskItem { Id = id, Title = "To Complete", Status = TaskItemStatus.InProgress });
        _mockTaskRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        var result = await _service.CompleteAsync(id);

        Assert.NotNull(result);
        Assert.Equal(TaskItemStatus.Completed, result.Status);
    }

    [Fact]
    public async Task CompleteAsync_WhenNotExists_ShouldReturnNull()
    {
        _mockTaskRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((TaskItem?)null);

        var result = await _service.CompleteAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task CompleteAsync_WhenAlreadyCompleted_ShouldStillReturnDto()
    {
        var id = Guid.NewGuid();
        _mockTaskRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new TaskItem { Id = id, Title = "Already Done", Status = TaskItemStatus.Completed });
        _mockTaskRepo.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
            .ReturnsAsync((TaskItem t) => t);

        var result = await _service.CompleteAsync(id);

        Assert.NotNull(result);
        Assert.Equal(TaskItemStatus.Completed, result.Status);
    }

    // ── DeleteAsync ──

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldReturnTrue()
    {
        _mockTaskRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var result = await _service.DeleteAsync(Guid.NewGuid());

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_ShouldReturnFalse()
    {
        _mockTaskRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        var result = await _service.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }
}
