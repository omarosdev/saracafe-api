using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaraCafe.API.DTOs;
using SaraCafe.API.Entities;
using SaraCafe.API.Services;

namespace SaraCafe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IImageService _imageService;

    public ProductsController(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IImageService imageService)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _imageService = imageService;
    }

    // GET: api/products
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] int? categoryId)
    {
        IEnumerable<Product> products;
        
        if (categoryId.HasValue)
        {
            products = await _productRepository.GetProductsByCategoryIdAsync(categoryId.Value);
        }
        else
        {
            products = await _productRepository.GetProductsAsync();
        }

        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            NameAr = p.NameAr,
            NameEn = p.NameEn,
            DescriptionAr = p.DescriptionAr,
            DescriptionEn = p.DescriptionEn,
            IsActive = p.IsActive,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
            Calories = p.Calories,
            CategoryId = p.CategoryId,
            CategoryNameAr = p.Category?.NameAr,
            CategoryNameEn = p.Category?.NameEn
        });

        return Ok(productDtos);
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }

        var productDto = new ProductDto
        {
            Id = product.Id,
            NameAr = product.NameAr,
            NameEn = product.NameEn,
            DescriptionAr = product.DescriptionAr,
            DescriptionEn = product.DescriptionEn,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
            Calories = product.Calories,
            CategoryId = product.CategoryId,
            CategoryNameAr = product.Category?.NameAr,
            CategoryNameEn = product.Category?.NameEn
        };

        return Ok(productDto);
    }

    // GET: api/products/category/{categoryId}
    [HttpGet("category/{categoryId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        var products = await _productRepository.GetProductsByCategoryIdAsync(categoryId);
        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            NameAr = p.NameAr,
            NameEn = p.NameEn,
            DescriptionAr = p.DescriptionAr,
            DescriptionEn = p.DescriptionEn,
            IsActive = p.IsActive,
            ImageUrl = p.ImageUrl,
            Price = p.Price,
            Calories = p.Calories,
            CategoryId = p.CategoryId,
            CategoryNameAr = p.Category?.NameAr,
            CategoryNameEn = p.Category?.NameEn
        });

        return Ok(productDtos);
    }

    // POST: api/products
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductDto createProductDto, [FromForm] IFormFile? imageFile)
    {
        // Validate category exists
        var category = await _categoryRepository.GetCategoryByIdAsync(createProductDto.CategoryId);
        if (category == null)
        {
            return BadRequest("Category not found");
        }

        string? imageUrl = null;
        if (imageFile != null && imageFile.Length > 0)
        {
            try
            {
                imageUrl = await _imageService.SaveImageAsync(imageFile);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error uploading image: {ex.Message}");
            }
        }

        var product = new Product
        {
            NameAr = createProductDto.NameAr,
            NameEn = createProductDto.NameEn,
            DescriptionAr = createProductDto.DescriptionAr,
            DescriptionEn = createProductDto.DescriptionEn,
            IsActive = createProductDto.IsActive,
            ImageUrl = imageUrl,
            Price = createProductDto.Price,
            Calories = createProductDto.Calories,
            CategoryId = createProductDto.CategoryId
        };

        var createdProduct = await _productRepository.CreateProductAsync(product);

        var productDto = new ProductDto
        {
            Id = createdProduct.Id,
            NameAr = createdProduct.NameAr,
            NameEn = createdProduct.NameEn,
            DescriptionAr = createdProduct.DescriptionAr,
            DescriptionEn = createdProduct.DescriptionEn,
            IsActive = createdProduct.IsActive,
            ImageUrl = createdProduct.ImageUrl,
            CategoryId = createdProduct.CategoryId,
            CategoryNameAr = createdProduct.Category?.NameAr,
            CategoryNameEn = createdProduct.Category?.NameEn
        };

        return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromForm] UpdateProductDto updateProductDto, [FromForm] IFormFile? imageFile)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Validate category exists
        var category = await _categoryRepository.GetCategoryByIdAsync(updateProductDto.CategoryId);
        if (category == null)
        {
            return BadRequest("Category not found");
        }

        // Handle image update
        string? imageUrl = product.ImageUrl;
        if (imageFile != null && imageFile.Length > 0)
        {
            // Delete old image if exists
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                _imageService.DeleteImage(product.ImageUrl);
            }

            try
            {
                imageUrl = await _imageService.SaveImageAsync(imageFile);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error uploading image: {ex.Message}");
            }
        }

        product.NameAr = updateProductDto.NameAr;
        product.NameEn = updateProductDto.NameEn;
        product.DescriptionAr = updateProductDto.DescriptionAr;
        product.DescriptionEn = updateProductDto.DescriptionEn;
        product.IsActive = updateProductDto.IsActive;
        product.ImageUrl = imageUrl;
        product.Price = updateProductDto.Price;
        product.Calories = updateProductDto.Calories;
        product.CategoryId = updateProductDto.CategoryId;

        var updatedProduct = await _productRepository.UpdateProductAsync(product);
        if (updatedProduct == null)
        {
            return NotFound();
        }

        var productDto = new ProductDto
        {
            Id = updatedProduct.Id,
            NameAr = updatedProduct.NameAr,
            NameEn = updatedProduct.NameEn,
            DescriptionAr = updatedProduct.DescriptionAr,
            DescriptionEn = updatedProduct.DescriptionEn,
            IsActive = updatedProduct.IsActive,
            ImageUrl = updatedProduct.ImageUrl,
            CategoryId = updatedProduct.CategoryId,
            CategoryNameAr = updatedProduct.Category?.NameAr,
            CategoryNameEn = updatedProduct.Category?.NameEn
        };

        return Ok(productDto);
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        // Delete associated image
        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            _imageService.DeleteImage(product.ImageUrl);
        }

        var deleted = await _productRepository.DeleteProductAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
