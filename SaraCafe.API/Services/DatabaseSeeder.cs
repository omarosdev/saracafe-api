using Microsoft.EntityFrameworkCore;
using SaraCafe.API.DbContexts;
using SaraCafe.API.Entities;

namespace SaraCafe.API.Services;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;

    public DatabaseSeeder(AppDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task SeedAsync()
    {
        // Ensure database is created
        await _context.Database.MigrateAsync();

        // Seed Categories
       

        // Seed Products
     /*
      if (!await _context.Products.AnyAsync())
        {
            await SeedProductsAsync();
        }
        
         if (!await _context.Categories.AnyAsync())
               {
                   await SeedCategoriesAsync();
               }
      */   

        // Seed Admin User
        if (!await _context.Users.AnyAsync())
        {
            await SeedUsersAsync();
        }
    }

    private async Task SeedCategoriesAsync()
    {
        var categories = new List<Category>
        {
            new Category("قهوة", "Coffee") { IsActive = true },
            new Category("مشروبات ساخنة", "Hot Beverages") { IsActive = true },
            new Category("مشروبات باردة", "Cold Beverages") { IsActive = true },
            new Category("حلويات", "Desserts") { IsActive = true },
            new Category("وجبات خفيفة", "Snacks") { IsActive = true }
        };

        _context.Categories.AddRange(categories);
        await _context.SaveChangesAsync();
    }

    private async Task SeedProductsAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        
        var coffeeCategory = categories.FirstOrDefault(c => c.NameEn == "Coffee");
        var hotBeveragesCategory = categories.FirstOrDefault(c => c.NameEn == "Hot Beverages");
        var coldBeveragesCategory = categories.FirstOrDefault(c => c.NameEn == "Cold Beverages");
        var dessertsCategory = categories.FirstOrDefault(c => c.NameEn == "Desserts");
        var snacksCategory = categories.FirstOrDefault(c => c.NameEn == "Snacks");

        var products = new List<Product>();

        // Coffee products
        if (coffeeCategory != null)
        {
            products.AddRange(new[]
            {
                new Product("إسبرسو", "Espresso")
                {
                    DescriptionAr = "قهوة إسبرسو إيطالية قوية",
                    DescriptionEn = "Strong Italian espresso",
                    IsActive = true,
                    CategoryId = coffeeCategory.Id
                },
                new Product("كابتشينو", "Cappuccino")
                {
                    DescriptionAr = "قهوة إسبرسو مع حليب رغوي",
                    DescriptionEn = "Espresso with foamed milk",
                    IsActive = true,
                    CategoryId = coffeeCategory.Id
                },
                new Product("لاتيه", "Latte")
                {
                    DescriptionAr = "قهوة إسبرسو مع حليب ساخن",
                    DescriptionEn = "Espresso with steamed milk",
                    IsActive = true,
                    CategoryId = coffeeCategory.Id
                },
                new Product("قهوة عربية", "Arabic Coffee")
                {
                    DescriptionAr = "قهوة عربية أصيلة",
                    DescriptionEn = "Traditional Arabic coffee",
                    IsActive = true,
                    CategoryId = coffeeCategory.Id
                }
            });
        }

        // Hot Beverages
        if (hotBeveragesCategory != null)
        {
            products.AddRange(new[]
            {
                new Product("شاي أحمر", "Black Tea")
                {
                    DescriptionAr = "شاي أحمر ساخن",
                    DescriptionEn = "Hot black tea",
                    IsActive = true,
                    CategoryId = hotBeveragesCategory.Id
                },
                new Product("شاي أخضر", "Green Tea")
                {
                    DescriptionAr = "شاي أخضر صحي",
                    DescriptionEn = "Healthy green tea",
                    IsActive = true,
                    CategoryId = hotBeveragesCategory.Id
                },
                new Product("شاي أعشاب", "Herbal Tea")
                {
                    DescriptionAr = "شاي أعشاب مهدئ",
                    DescriptionEn = "Calming herbal tea",
                    IsActive = true,
                    CategoryId = hotBeveragesCategory.Id
                },
                new Product("شوكولاتة ساخنة", "Hot Chocolate")
                {
                    DescriptionAr = "شوكولاتة ساخنة لذيذة",
                    DescriptionEn = "Delicious hot chocolate",
                    IsActive = true,
                    CategoryId = hotBeveragesCategory.Id
                }
            });
        }

        // Cold Beverages
        if (coldBeveragesCategory != null)
        {
            products.AddRange(new[]
            {
                new Product("عصير برتقال", "Orange Juice")
                {
                    DescriptionAr = "عصير برتقال طازج",
                    DescriptionEn = "Fresh orange juice",
                    IsActive = true,
                    CategoryId = coldBeveragesCategory.Id
                },
                new Product("عصير تفاح", "Apple Juice")
                {
                    DescriptionAr = "عصير تفاح منعش",
                    DescriptionEn = "Refreshing apple juice",
                    IsActive = true,
                    CategoryId = coldBeveragesCategory.Id
                },
                new Product("آيس كوفي", "Iced Coffee")
                {
                    DescriptionAr = "قهوة مثلجة منعشة",
                    DescriptionEn = "Refreshing iced coffee",
                    IsActive = true,
                    CategoryId = coldBeveragesCategory.Id
                },
                new Product("ليموناضة", "Lemonade")
                {
                    DescriptionAr = "ليموناضة منعشة",
                    DescriptionEn = "Refreshing lemonade",
                    IsActive = true,
                    CategoryId = coldBeveragesCategory.Id
                }
            });
        }

        // Desserts
        if (dessertsCategory != null)
        {
            products.AddRange(new[]
            {
                new Product("تشيز كيك", "Cheesecake")
                {
                    DescriptionAr = "تشيز كيك كريمي لذيذ",
                    DescriptionEn = "Creamy delicious cheesecake",
                    IsActive = true,
                    CategoryId = dessertsCategory.Id
                },
                new Product("براوني", "Brownie")
                {
                    DescriptionAr = "براوني بالشوكولاتة",
                    DescriptionEn = "Chocolate brownie",
                    IsActive = true,
                    CategoryId = dessertsCategory.Id
                },
                new Product("آيس كريم", "Ice Cream")
                {
                    DescriptionAr = "آيس كريم بنكهات متعددة",
                    DescriptionEn = "Ice cream with multiple flavors",
                    IsActive = true,
                    CategoryId = dessertsCategory.Id
                }
            });
        }

        // Snacks
        if (snacksCategory != null)
        {
            products.AddRange(new[]
            {
                new Product("ساندويتش دجاج", "Chicken Sandwich")
                {
                    DescriptionAr = "ساندويتش دجاج مشوي",
                    DescriptionEn = "Grilled chicken sandwich",
                    IsActive = true,
                    CategoryId = snacksCategory.Id
                },
                new Product("ساندويتش جبنة", "Cheese Sandwich")
                {
                    DescriptionAr = "ساندويتش جبنة طبيعية",
                    DescriptionEn = "Natural cheese sandwich",
                    IsActive = true,
                    CategoryId = snacksCategory.Id
                },
                new Product("كرواسون", "Croissant")
                {
                    DescriptionAr = "كرواسون فرنسي طازج",
                    DescriptionEn = "Fresh French croissant",
                    IsActive = true,
                    CategoryId = snacksCategory.Id
                }
            });
        }

        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@saracafe.com",
            PasswordHash = _authService.HashPassword("Admin@123"),
            FirstName = "Admin",
            LastName = "User"
        };

        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync();
    }
}

