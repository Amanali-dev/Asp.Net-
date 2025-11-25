
namespace Restoran.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }

        internal static async Task<dynamic> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
} 