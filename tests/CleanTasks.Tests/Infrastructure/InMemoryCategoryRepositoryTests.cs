using CleanTasks.Domain.Entities;
using CleanTasks.Infrastructure.Repositories;

namespace CleanTasks.Tests.Infrastructure;

public class InMemoryCategoryRepositoryTests
{
    private readonly InMemoryCategoryRepository _repository;

    public InMemoryCategoryRepositoryTests()
    {
        _repository = new InMemoryCategoryRepository();
    }

    [Fact]
    public async Task AddAsync_ShouldStoreCategory()
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Work",
            Description = "Work-related tasks"
        };

        var result = await _repository.AddAsync(category);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
        Assert.Equal("Work", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnCategory()
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Personal",
            Description = "Personal tasks"
        };
        await _repository.AddAsync(category);

        var result = await _repository.GetByIdAsync(category.Id);

        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
        Assert.Equal("Personal", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories()
    {
        await _repository.AddAsync(new Category { Id = Guid.NewGuid(), Name = "A" });
        await _repository.AddAsync(new Category { Id = Guid.NewGuid(), Name = "B" });
        await _repository.AddAsync(new Category { Id = Guid.NewGuid(), Name = "C" });

        var result = (await _repository.GetAllAsync()).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_WhenEmpty_ShouldReturnEmptyCollection()
    {
        var result = (await _repository.GetAllAsync()).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByNameAsync_WhenExists_ShouldReturnCategory()
    {
        await _repository.AddAsync(new Category { Id = Guid.NewGuid(), Name = "Work" });

        var result = await _repository.GetByNameAsync("Work");

        Assert.NotNull(result);
        Assert.Equal("Work", result.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldBeCaseInsensitive()
    {
        await _repository.AddAsync(new Category { Id = Guid.NewGuid(), Name = "Work" });

        var result = await _repository.GetByNameAsync("work");

        Assert.NotNull(result);
        Assert.Equal("Work", result.Name);
    }

    [Fact]
    public async Task GetByNameAsync_WhenNotExists_ShouldReturnNull()
    {
        var result = await _repository.GetByNameAsync("NonExistent");

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WhenExists_ShouldUpdateAndReturn()
    {
        var id = Guid.NewGuid();
        await _repository.AddAsync(new Category { Id = id, Name = "Old Name", Description = "Old" });

        var updated = new Category { Id = id, Name = "New Name", Description = "New" };
        var result = await _repository.UpdateAsync(updated);

        Assert.NotNull(result);
        Assert.Equal("New Name", result.Name);
        Assert.Equal("New", result.Description);

        var fetched = await _repository.GetByIdAsync(id);
        Assert.Equal("New Name", fetched!.Name);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotExists_ShouldReturnNull()
    {
        var updated = new Category { Id = Guid.NewGuid(), Name = "Nope" };

        var result = await _repository.UpdateAsync(updated);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldReturnTrueAndRemove()
    {
        var id = Guid.NewGuid();
        await _repository.AddAsync(new Category { Id = id, Name = "ToDelete" });

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
