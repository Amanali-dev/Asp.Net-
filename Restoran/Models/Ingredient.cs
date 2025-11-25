using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Restoran.Models
{
    public class Ingredient
    {
        public int  IngredientId { get; set; }
        public string Name { get; set; }

        [ValidateNever]
        public ICollection<Product> Products { get; set; }
        public ICollection<ProductIngredient>? ProductIngredients { get; set; }
    }
}