using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaraCafe.API.Entities;

public class Category
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

    public bool IsActive { get; set; } = true;

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public Category(string nameAr, string nameEn)
    {
        NameAr = nameAr;
        NameEn = nameEn;
    }
    
    public Category() { }
}