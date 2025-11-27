using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Restoran.Models;

namespace Restoran.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<ProductIngredient> ProductIngredients { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            base.OnModelCreating(ModelBuilder);
            // Define composite key and relationships for productIngresients
            ModelBuilder.Entity<ProductIngredient>()
                .HasKey(pi => new { pi.ProductId, pi.IngredientId });

            ModelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .HasForeignKey(pi => pi.ProductId);

            ModelBuilder.Entity<ProductIngredient>()
                .HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .HasForeignKey(pi => pi.IngredientId);

            ModelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
            ModelBuilder.Entity<OrderItem>().Property(oi => oi.Price).HasPrecision(18, 2);
            ModelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);

            ModelBuilder.Entity<Product>()
              .HasOne(p => p.Category)
              .WithMany(c => c.Products)
              .HasForeignKey(p => p.CategoryId)
              .OnDelete(DeleteBehavior.Restrict); // IMPORTANT: no cascade



            /// seed data
            ModelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Appetizer"},
                new Category { CategoryId = 2, Name = "Entree" },
                new Category { CategoryId = 3, Name = "side Dish" },
                new Category { CategoryId = 4, Name = "Dessert" },
                new Category { CategoryId = 5, Name = "Beverage" }
                );
            // Aad Mexican restaurant ingredients here
            ModelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { IngredientId = 1, Name = "Beef" },
                new Ingredient { IngredientId = 2, Name = "Chicken" },
                new Ingredient { IngredientId = 3, Name = "Fish" },
                new Ingredient { IngredientId = 4, Name = "Tortilla" },
                new Ingredient { IngredientId = 5, Name = "Tomato" }
                );

            ModelBuilder.Entity<Product>().HasData(
               
              //  Add mexican Restaurant Food Entries here
              new Product
              {
                  ProductId = 1,
                  Name = "Beef Taco",
                  Description = "A Delicious",
                  Price = 400m,
                  Stock = 100,
                  CategoryId = 2,
                  ImageUrl = "default.jpg"

              },
                new Product
                {
                    ProductId = 2,
                    Name = "Chicken Taco",
                    Description = "A Delicious",
                    Price = 350m,
                    Stock = 100,
                    CategoryId = 2,
                    ImageUrl = "default.jpg"
                },
                  new Product
                  {
                      ProductId = 3,
                      Name = "Fish Taco",
                      Description = "A Delicious",
                      Price = 550m,
                      Stock = 100,
                      CategoryId = 2,
                      ImageUrl = "default.jpg"
                  },
                    new Product
                    {
                        ProductId = 4,
                        Name = "Tortilla Taco",
                        Description = "A Delicious",
                        Price = 600m,
                        Stock = 100,
                        CategoryId = 2,
                        ImageUrl = "default.jpg"
                    }







          

             );

            ModelBuilder.Entity<ProductIngredient>().HasData(
                new ProductIngredient { ProductId = 1, IngredientId = 1 },
                new ProductIngredient { ProductId = 1, IngredientId = 4 },
                new ProductIngredient { ProductId = 1, IngredientId = 5 },
                new ProductIngredient { ProductId = 2, IngredientId = 2 },
                new ProductIngredient { ProductId = 2, IngredientId = 4 },
                new ProductIngredient { ProductId = 2, IngredientId = 5 },
                new ProductIngredient { ProductId = 3, IngredientId = 3 },
                new ProductIngredient { ProductId = 3, IngredientId = 4 },
                new ProductIngredient { ProductId = 3, IngredientId = 5 },
                new ProductIngredient { ProductId = 4, IngredientId = 2 },
                new ProductIngredient { ProductId = 4, IngredientId = 4 },
                new ProductIngredient { ProductId = 4, IngredientId = 5 }


                 );


        }





    }
}
