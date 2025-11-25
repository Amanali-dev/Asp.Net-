using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restoran.Models
{
    public class Product
    {   public Product()
        {
            ProductIngredients = new List<ProductIngredient>();
        }

        public int ProductId { get; set; }  

        public string Name { get; set; }
       
        public string Description { get; set; } 
        public decimal Price { get; set; }  
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string ImageUrl { get; set; }

        [ValidateNever]
        public Category? category  { get; set; }

        [ValidateNever]

        public ICollection<Ingredient> Ingredients { get; set; }
        [ValidateNever]
        public ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
        //public ICollection<ProductIngredient> ProductIgredients { get; set; }

        [ValidateNever]
        public ICollection<OrderItem> OrderItems { get; set; }
        //public string ImagePath { get; internal set; }
        public string? ImagePath { get; set; }

        //public object ProductIngredient { get; internal set; }


    }
}