using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaraCafe.API.DTOs;
using SaraCafe.API.Entities;
using SaraCafe.API.Services;

namespace SaraCafe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // GET: api/categories
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _categoryRepository.GetCategoriesAsync();
        var categoryDtos = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            NameAr = c.NameAr,
            NameEn = c.NameEn,
            IsActive = c.IsActive
        });

        return Ok(categoryDtos);
    }

    // GET: api/categories/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        
        if (category == null)
        {
            return NotFound();
        }

        var categoryDto = new CategoryDto
        {
            Id = category.Id,
            NameAr = category.NameAr,
            NameEn = category.NameEn,
            IsActive = category.IsActive
        };

        return Ok(categoryDto);
    }

    // POST: api/categories
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var category = new Category
        {
            NameAr = createCategoryDto.NameAr,
            NameEn = createCategoryDto.NameEn,
            IsActive = createCategoryDto.IsActive
        };

        var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
        await _categoryRepository.SaveChangesAsync();

        var categoryDto = new CategoryDto
        {
            Id = createdCategory.Id,
            NameAr = createdCategory.NameAr,
            NameEn = createdCategory.NameEn,
            IsActive = createdCategory.IsActive
        };

        return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, categoryDto);
    }

    // PUT: api/categories/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        category.NameAr = updateCategoryDto.NameAr;
        category.NameEn = updateCategoryDto.NameEn;
        category.IsActive = updateCategoryDto.IsActive;

        var updatedCategory = await _categoryRepository.UpdateCategoryAsync(category);
        if (updatedCategory == null)
        {
            return NotFound();
        }

        var categoryDto = new CategoryDto
        {
            Id = updatedCategory.Id,
            NameAr = updatedCategory.NameAr,
            NameEn = updatedCategory.NameEn,
            IsActive = updatedCategory.IsActive
        };

        return Ok(categoryDto);
    }

    // DELETE: api/categories/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await _categoryRepository.DeleteCategoryAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
