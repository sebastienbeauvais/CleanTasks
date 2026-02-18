using CleanTasks.Domain.Entities;
using CleanTasks.Domain.Interfaces;

namespace CleanTasks.Infrastructure.Repositories;

public class InMemoryCategoryRepository : ICategoryRepository
{
    // TODO: Add a thread-safe collection to store categories

    public Task<Category?> GetByIdAsync(Guid id)
    {
        // TODO: Implement - return category matching id, or null
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Category>> GetAllAsync()
    {
        // TODO: Implement - return all stored categories
        throw new NotImplementedException();
    }

    public Task<Category?> GetByNameAsync(string name)
    {
        // TODO: Implement - return category matching name (case-insensitive), or null
        throw new NotImplementedException();
    }

    public Task<Category> AddAsync(Category category)
    {
        // TODO: Implement - store the category and return it
        throw new NotImplementedException();
    }

    public Task<Category?> UpdateAsync(Category category)
    {
        // TODO: Implement - update existing category if found, return updated or null
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        // TODO: Implement - remove category by id, return true if found and removed
        throw new NotImplementedException();
    }
}
