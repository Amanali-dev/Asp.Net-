//namespace Restoran.Models
//{
//    public class OrderItem
//    {
//        public int OrderItemId { get; set; }
//        public int ProductId { get; set; }
//        public int OrderId { get; set; }

//        public string? Name { get; set; }

//        public string? Description { get; set; }

//        public decimal Price { get; set; }

//        public int Stock { get; set; }

//        public int CategoryId { get; set; }

//        public Category? Category { get; set; } // A product belongs to a category

//        public ICollection<OrderItem>? OrderItems { get; set; } // A product can be in many order items
//        public int Quantity { get; internal set; }
//        public Product Product { get; set; }  // <-- important

//    }




//}
using System;

namespace Restoran.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        // FK to Product
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        // FK to Order
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }

        // Quantity for this order item
        public int Quantity { get; set; }
    }
}