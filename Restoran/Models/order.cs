//namespace Restoran.Models
//{
//    public class Order
//    {
//        public int OrderId { get; set; }
//        public  DateTime OrderDate { get; set; }
//        public string UserId { get; set; }
//        public ApplicationUser User { get; set; }
//        public decimal TotalAmount { get; set; }
//        public ICollection<OrderItem> OrderItem { get; set; }


//        public decimal TotalFee { get; set; } = 0;
//        public object OrderItems { get; internal set; }

//    }

//}
using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        // Make UserId nullable to reflect DB nullability if needed.
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public decimal TotalAmount { get; set; }

        // Fix: use a concrete collection type for EF navigation and initialize it.
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public decimal TotalFee { get; set; }
    }
}