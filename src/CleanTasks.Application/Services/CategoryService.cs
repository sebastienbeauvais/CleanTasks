using CleanTasks.Application.DTOs;
using CleanTasks.Application.Interfaces;
using CleanTasks.Domain.Interfaces;

namespace CleanTasks.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<CategoryDto?> GetByIdAsync(Guid id)
    {
        // TODO: Implement - retrieve category by id and map to DTO
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        // TODO: Implement - retrieve all categories and map to DTOs
        throw new NotImplementedException();
    }

    public Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        // TODO: Implement - validate, create entity, persist, return DTO
        throw new NotImplementedException();
    }

    public Task<CategoryDto?> UpdateAsync(Guid id, UpdateCategoryRequest request)
    {
        // TODO: Implement - find existing, update fields, persist, return DTO
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        // TODO: Implement - delete category by id
        throw new NotImplementedException();
    }
}
