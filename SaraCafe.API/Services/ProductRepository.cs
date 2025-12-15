using Microsoft.EntityFrameworkCore;
using SaraCafe.API.DbContexts;
using SaraCafe.API.Entities;

namespace SaraCafe.API.Services;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.NameAr)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.NameAr)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await SaveChangesAsync();
        
        // Reload with category
        return await GetProductByIdAsync(product.Id) ?? product;
    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        var existingProduct = await GetProductByIdAsync(product.Id);
        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.NameAr = product.NameAr;
        existingProduct.NameEn = product.NameEn;
        existingProduct.DescriptionAr = product.DescriptionAr;
        existingProduct.DescriptionEn = product.DescriptionEn;
        existingProduct.IsActive = product.IsActive;
        existingProduct.ImageUrl = product.ImageUrl;
        existingProduct.CategoryId = product.CategoryId;

        await SaveChangesAsync();
        
        // Reload with category to ensure navigation property is up to date
        return await GetProductByIdAsync(existingProduct.Id) ?? existingProduct;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await GetProductByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        return await SaveChangesAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}

