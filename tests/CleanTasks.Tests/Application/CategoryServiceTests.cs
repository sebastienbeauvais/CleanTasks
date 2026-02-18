using CleanTasks.Application.DTOs;
using CleanTasks.Application.Services;
using CleanTasks.Domain.Entities;
using CleanTasks.Domain.Interfaces;
using Moq;

namespace CleanTasks.Tests.Application;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _service = new CategoryService(_mockRepo.Object);
    }

    // ── GetByIdAsync ──

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnDto()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new Category { Id = id, Name = "Work", Description = "Work stuff" });

        var result = await _service.GetByIdAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Work", result.Name);
        Assert.Equal("Work stuff", result.Description);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Category?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    // ── GetAllAsync ──

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAsDtos()
    {
        _mockRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "A" },
                new() { Id = Guid.NewGuid(), Name = "B" }
            });

        var result = (await _service.GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
    }

    // ── CreateAsync ──

    [Fact]
    public async Task CreateAsync_WithValidRequest_ShouldCreateAndReturnDto()
    {
        _mockRepo.Setup(r => r.GetByNameAsync("Work"))
            .ReturnsAsync((Category?)null);
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category c) => c);

        var request = new CreateCategoryRequest { Name = "Work", Description = "Work tasks" };

        var result = await _service.CreateAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Work", result.Name);
        Assert.Equal("Work tasks", result.Description);
        Assert.NotEqual(Guid.Empty, result.Id);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmptyName_ShouldThrowArgumentException()
    {
        var request = new CreateCategoryRequest { Name = "", Description = "No name" };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_WithWhitespaceName_ShouldThrowArgumentException()
    {
        var request = new CreateCategoryRequest { Name = "   ", Description = "Whitespace" };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ShouldThrowInvalidOperationException()
    {
        _mockRepo.Setup(r => r.GetByNameAsync("Work"))
            .ReturnsAsync(new Category { Id = Guid.NewGuid(), Name = "Work" });

        var request = new CreateCategoryRequest { Name = "Work", Description = "Duplicate" };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(request));
    }

    // ── UpdateAsync ──

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateAndReturnDto()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new Category { Id = id, Name = "Old", Description = "Old desc" });
        _mockRepo.Setup(r => r.GetByNameAsync("New"))
            .ReturnsAsync((Category?)null);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category c) => c);

        var request = new UpdateCategoryRequest { Name = "New", Description = "New desc" };

        var result = await _service.UpdateAsync(id, request);

        Assert.NotNull(result);
        Assert.Equal("New", result.Name);
        Assert.Equal("New desc", result.Description);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldReturnNull()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Category?)null);

        var result = await _service.UpdateAsync(Guid.NewGuid(), new UpdateCategoryRequest { Name = "X" });

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WithEmptyName_ShouldThrowArgumentException()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new Category { Id = id, Name = "Exists" });

        var request = new UpdateCategoryRequest { Name = "" };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(id, request));
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateName_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new Category { Id = id, Name = "Original" });
        _mockRepo.Setup(r => r.GetByNameAsync("Taken"))
            .ReturnsAsync(new Category { Id = otherId, Name = "Taken" });

        var request = new UpdateCategoryRequest { Name = "Taken" };

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(id, request));
    }

    [Fact]
    public async Task UpdateAsync_KeepingSameName_ShouldNotThrowDuplicate()
    {
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new Category { Id = id, Name = "Same" });
        _mockRepo.Setup(r => r.GetByNameAsync("Same"))
            .ReturnsAsync(new Category { Id = id, Name = "Same" });
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category c) => c);

        var request = new UpdateCategoryRequest { Name = "Same", Description = "Updated desc" };

        var result = await _service.UpdateAsync(id, request);

        Assert.NotNull(result);
        Assert.Equal("Updated desc", result.Description);
    }

    // ── DeleteAsync ──

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldReturnTrue()
    {
        _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var result = await _service.DeleteAsync(Guid.NewGuid());

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_ShouldReturnFalse()
    {
        _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        var result = await _service.DeleteAsync(Guid.NewGuid());

        Assert.False(result);
    }
}
