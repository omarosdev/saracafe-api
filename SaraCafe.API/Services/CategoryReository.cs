using Microsoft.EntityFrameworkCore;
using SaraCafe.API.DbContexts;
using SaraCafe.API.Entities;

namespace SaraCafe.API.Services;

public class CategoryReository : ICategoryRepository
{
    private readonly AppDbContext _context;
    
    public CategoryReository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.OrderBy(c => c.NameAr).ToListAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateCategoryAsync(Category category)
    {
        var existingCategory = await GetCategoryByIdAsync(category.Id);
        if (existingCategory == null)
        {
            return null;
        }

        existingCategory.NameAr = category.NameAr;
        existingCategory.NameEn = category.NameEn;
        existingCategory.IsActive = category.IsActive;

        await SaveChangesAsync();
        return existingCategory;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await GetCategoryByIdAsync(id);
        if (category == null)
        {
            return false;
        }

        _context.Categories.Remove(category);
        return await SaveChangesAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}