using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaraCafe.API.Entities;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string NameAr { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string NameEn { get; set; } = string.Empty;
    
   
    public string? DescriptionAr { get; set; }
    
    public string? DescriptionEn { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public string? ImageUrl { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Price { get; set; }
    
    [MaxLength(100)]
    public string? Calories { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    public int CategoryId { get; set; }

    public Product(string nameAr, string nameEn)
    {
        NameAr = nameAr;
        NameEn = nameEn;
    }
    
    public Product() { }
}