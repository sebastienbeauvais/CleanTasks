using CleanTasks.Application.DTOs;
using CleanTasks.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CleanTasks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        // TODO: Implement - return all categories
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> GetById(Guid id)
    {
        // TODO: Implement - return category by id, or 404
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryRequest request)
    {
        // TODO: Implement - create category, return 201 with location header
        throw new NotImplementedException();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CategoryDto>> Update(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        // TODO: Implement - update category, return updated or 404
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        // TODO: Implement - delete category, return 204 or 404
        throw new NotImplementedException();
    }
}
