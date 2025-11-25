using System.Collections.Generic;

namespace Restoran.Models
{
    public class OrderViewModel
    {
        public List<OrderItemViewModel> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
        public List<Product> Products { get; set; } // Add this property
    }
}
