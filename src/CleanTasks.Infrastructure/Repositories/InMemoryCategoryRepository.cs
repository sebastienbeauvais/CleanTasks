using CleanTasks.Domain.Entities;
using CleanTasks.Domain.Interfaces;
using System.Collections.Concurrent;

namespace CleanTasks.Infrastructure.Repositories;

public class InMemoryCategoryRepository : ICategoryRepository
{
    // TODO: Add a thread-safe collection to store categories
    ConcurrentDictionary<string, Category> _categories = new ConcurrentDictionary<string, Category>();

    public Task<Category?> GetByIdAsync(Guid id)
    {
        // TODO: Implement - return category matching id, or null
        if(_categories.ContainsKey(id.ToString()))
        {
            return Task.FromResult(_categories.GetValueOrDefault(id.ToString()));
        }
        return Task.FromResult<Category?>(null);
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
        _categories.GetOrAdd(category.Id.ToString(), category);
        return Task.FromResult(category);
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
